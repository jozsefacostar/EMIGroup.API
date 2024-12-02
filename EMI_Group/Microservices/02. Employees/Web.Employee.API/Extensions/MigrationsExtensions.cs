using Domain.Entities;
using Domain.ValueObjects;
using Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Infraestructure;
using k8s.KubeConfigModels;

namespace Web.API.Extensions
{
    public static class MigrationsExtensions
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scopre = app.Services.CreateScope();

            var dbContext = scopre.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
