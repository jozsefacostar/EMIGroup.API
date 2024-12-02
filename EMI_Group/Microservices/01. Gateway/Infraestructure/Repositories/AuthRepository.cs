using Application;
using Application.Responses;
using Domain.Entities;
using Infraestructure.Configurations;
using Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Infraestructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtSettings _jwtSettings;
        public AuthRepository(ApplicationDbContext context, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        #region Public Methods
        /* Función que crea un usuario  */
        public async Task<AuthResponseDTO> CreateUser(string username, string password)
        {
            var (successValidation, resultValidation) = await ValidateUserCreate(username, password);
            if (successValidation)
            {
                await AddUser(new User
                {
                    Username = username,
                    PasswordHash = password
                });
                return new AuthResponseDTO(true, "Usuario agregado correctamente", username, null, null);
            }
            return new AuthResponseDTO(false, resultValidation, username, null, null);
        }
        /* Función que permite el login del usuario */
        public async Task<AuthResponseDTO> Login(string username, string password)
        {
            var user = await GetByUserName(username);
            if (user == null)
                return new AuthResponseDTO(false, "No existe el usuario en el sistema", username, null, null);
            var isCorrectPassword = VerifyPassword(password, user.PasswordHash);
            if (!isCorrectPassword)
            {
                return new AuthResponseDTO(false, "La contraseña no es correcta", username, null, null);
            }
            DateTime TokenExpiryDate = DateTime.Now.AddMinutes(_jwtSettings.ExpiresMinutes);
            var newToken = CreateJwt(user, TokenExpiryDate);
            user.TokenExpiryDate = TokenExpiryDate;
            user.Token = newToken;
            await _context.SaveChangesAsync();
            return new AuthResponseDTO(true, "Login exitoso", username, newToken, user.TokenExpiryDate);
        }
        #endregion

        #region Private Methods

        /* Función que permite agrega la entidad a la base de datos  */
        private async Task AddUser(User user)
        {
            user.PasswordHash = HashPassword(user.PasswordHash);
            /* Siempre se crean con el rol de usuario */
            user.UserRole = 2;
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        /* Función que valida si la contraseña es correcta */
        private bool VerifyPassword(string providedPassword, string storedHash)
        {
            // Decodificar el hash en base64 a un arreglo de bytes
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // El salt es el primer segmento del hash almacenado (SaltSize)
            byte[] salt = new byte[_jwtSettings.SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, _jwtSettings.SaltSize);

            // El hash es el segundo segmento del hash almacenado (HashSize)
            byte[] storedPasswordHash = new byte[_jwtSettings.HashSize];
            Array.Copy(hashBytes, _jwtSettings.SaltSize, storedPasswordHash, 0, _jwtSettings.HashSize);

            // Derivar el hash de la contraseña proporcionada usando el mismo salt y las mismas iteraciones
            using (var key = new Rfc2898DeriveBytes(providedPassword, salt, _jwtSettings.Iterations, HashAlgorithmName.SHA256))
            {
                byte[] providedPasswordHash = key.GetBytes(_jwtSettings.HashSize);

                // Comparar el hash generado con el almacenado
                return storedPasswordHash.SequenceEqual(providedPasswordHash);
            }
        }
        /* Función que valida si un usuario existe en la base de datos */
        private async Task<User> GetByUserName(string username)
        {
            if (string.IsNullOrEmpty(username)) return null;

            var result = await _context.Users.Include(x => x.UserRoleNavigation)
                 .Where(x => x.Username.Equals(username))
                 .FirstOrDefaultAsync();

            return result;
        }
        /* Función que valida las condiciones de la contraseña */
        private static string CheckPasswordStrength(string pass)
        {
            StringBuilder sb = new StringBuilder();
            if (pass.Length < 9)
                sb.Append("El tamaño minimo de la contraseña debe ser 8" + Environment.NewLine);
            if (!(Regex.IsMatch(pass, "[a-z]") && Regex.IsMatch(pass, "[A-Z]") && Regex.IsMatch(pass, "[0-9]")))
                sb.Append("La contraseña debe tener combinaciones entre mayusculas y minusculas" + Environment.NewLine);
            if (!Regex.IsMatch(pass, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
                sb.Append("La contraseña debe contener un caracter especial" + Environment.NewLine);
            return sb.ToString();
        }
        /* Función que le aplica Hash a la contraseña  */
        private string HashPassword(string password)
        {
            byte[] salt = new byte[_jwtSettings.SaltSize];

            // Generar el salt usando RandomNumberGenerator
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Derivar la clave usando Rfc2898DeriveBytes
            using (var key = new Rfc2898DeriveBytes(password, salt, _jwtSettings.Iterations, HashAlgorithmName.SHA256))
            {
                var hash = key.GetBytes(_jwtSettings.HashSize);

                // Combinar el salt y el hash en un solo arreglo de bytes
                var hashBytes = new byte[_jwtSettings.SaltSize + _jwtSettings.HashSize];
                Array.Copy(salt, 0, hashBytes, 0, _jwtSettings.SaltSize);
                Array.Copy(hash, 0, hashBytes, _jwtSettings.SaltSize, _jwtSettings.HashSize);

                // Convertir el resultado a Base64 para almacenamiento
                var base64Hash = Convert.ToBase64String(hashBytes);

                return base64Hash;
            }
        }
        /* Función que crea un JWT cuando la contraseña es correcta */
        private string CreateJwt(User user, DateTime DateExpires)
        {
            // Definir los Claims (información que contiene el token)
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,$"{user.Username}"),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.UserRoleNavigation.Code),
                new Claim(JwtRegisteredClaimNames.Aud, _jwtSettings.Audience),
            };

            // Crear la clave de seguridad (secreto compartido) que se usará para firmar el token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Crear el token JWT
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiresMinutes),
                signingCredentials: creds
            );

            // Generar el JWT en forma de cadena
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
        /* Función que valida si el usuario existe en la base de datos */
        private async Task<(bool success, string result)> ValidateUserCreate(string username, string password)
        {
            bool success = false;
            string result = string.Empty;

            var user = await GetByUserName(username);
            if (user != null)
                return (false, "El usuario ya existe en el sistema");

            var passMessage = CheckPasswordStrength(password);
            if (!string.IsNullOrEmpty(passMessage))
                result = passMessage.ToString();
            else
                success = true;
            return (success, result);
        }

        #endregion
    }
}
