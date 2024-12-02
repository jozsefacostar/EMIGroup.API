
using Auth.DomainGateway;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using Web.Employee.API.Middlewares.GlobalExceptions;

namespace Web.Employee.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder => builder.WithOrigins("http://localhost:4200")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });
            services.AddControllers();
            services.AddSwaggerConf();
            services.AddAuthenticationLib(configuration);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddTransient<GlobalExceptionHandlingMiddleware>();
            return services;
        }
        private static IServiceCollection AddSwaggerConf(this IServiceCollection services)
        {

            // Agregamos servicios para Swagger
            services.AddSwaggerGen(options =>
            {
                // Configuramos la seguridad del token JWT en Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Ingrese 'Bearer' seguido de un espacio y luego el token JWT.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,  // El token será pasado en el encabezado de la solicitud
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" // Nombre del security scheme
                            }
                        },
                        new string[] { }
                    }
                });
            });

            return services;

        }
    }
}
