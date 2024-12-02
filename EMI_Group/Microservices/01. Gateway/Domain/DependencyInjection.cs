using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Auth.DomainGateway
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthenticationLib(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("Jwt");
            var issuer = jwtConfig.GetValue<string>("Issuer");
            var audience = jwtConfig.GetValue<string>("Audience");
            var keyEncript = jwtConfig.GetValue<string>("SecretKey");

            // Configuramos la autenticación JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyEncript))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ADM", policy => policy.RequireRole("ADM"));
                options.AddPolicy("USR", policy => policy.RequireRole("USR"));
            });
            return services;
        }
    }
}
