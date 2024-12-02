using Application;
using HealthChecks.UI.Client;
using Infraestructure;
using Infraestructure.Configurations;
using Infraestructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;
using Web.API.Extensions;
using Web.Employee.API;
using Web.Employee.API.EndPoints;
using Web.Employee.API.Middlewares.Authetication;
using Web.Employee.API.Middlewares.GlobalExceptions;
using Web.Employee.API.Middlewares.HealthCheck;


var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddPresentation(builder.Configuration).AddInfraestructure(builder.Configuration).AddAplication();
    builder.Services.ConfigureHealthChecks(builder.Configuration);
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();
    if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    {
        app.UseCors("AllowOrigin");
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee API V1");
            options.RoutePrefix = "swagger";
        });
        app.ApplyMigrations();
    }

    // Configure the HTTP request pipeline.

    // Mapeamos los endpoints
    var authSettings = app.Services.GetRequiredService<IOptions<AuthSettings>>().Value;
    GetEmployeeEndpoints.DefineEndpoints(app, authSettings);
    EmployeesTransactionEndPoints.DefineEndpoints(app, authSettings);
    WriteEmployeeEndpoints.DefineEndpoints(app, authSettings);
    DeleteEmployeeEndpoints.DefineEndpoints(app, authSettings);
    BrokerEndPoints.DefineEndpoints(app, authSettings);
    //HealthCheck Middleware
    app.MapHealthChecks("/api/health", new HealthCheckOptions() { Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
    app.UseHealthChecksUI(delegate (HealthChecks.UI.Configuration.Options options) { options.UIPath = "/healthcheck-ui"; });
    // Configure the HTTP request pipeline.
    app.UseHttpsRedirection();
    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();   
    app.UseMiddleware<AuthenticationMiddlewareAPI>();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    // Middleware para ejecutar SeedData antes de procesar cualquier solicitud
    app.Use(async (context, next) =>
    {
        var dbContext = context.RequestServices.GetRequiredService<ApplicationDbContext>();
        builder.Services.SeedData(dbContext);
        await next();
    });

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, $"The program was stopped because there was an error: {ex.Message}");
}
finally
{
    NLog.LogManager.Shutdown();
}
