using Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
