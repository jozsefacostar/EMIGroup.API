using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Web.Employee.API.Middlewares.Authetication
{
    public class AuthenticationMiddlewareAPI
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddlewareAPI(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authorizationHeader = context.Request.Headers["Authorization"].ToString();

            if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                var userId = ValidateTokenAndGetUserId(token);

                if (userId != null)
                {
                    // Almacenar el ID del usuario en el contexto de la solicitud
                    context.Items["UserId"] = userId;
                }
                else
                {
                    context.Response.StatusCode = 401; // No autorizado
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }
            }

            await _next(context);
        }

        private int ValidateTokenAndGetUserId(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Verifica la validez del token aquí (por ejemplo, usando claves públicas o privadas)

                var userIdClaim = jwtToken?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null) 
                    return Convert.ToInt32(userIdClaim.Value);

            }
            catch (Exception ex)
            {
                // Si hay un error, consideramos que el token no es válido
            }

            return 0;
        }
    }
}
