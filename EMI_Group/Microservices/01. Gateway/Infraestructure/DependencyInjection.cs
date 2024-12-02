using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Infraestructure.Repositories;
using Application;
using Infraestructure.Persistence;
using Microsoft.Extensions.Configuration;
using Application.Data;
using Domain.Primitives;
using Infraestructure.Configurations;

namespace Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraestructureGateway(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuramos de JWT desde appsettings
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            services.Configure<AuthorizationSettings>(configuration.GetSection("Authorization"));

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));
            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IUnitOfWorkGateway>(sp => sp.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IAuthRepository, AuthRepository>();
            return services;
        }
    }
}
