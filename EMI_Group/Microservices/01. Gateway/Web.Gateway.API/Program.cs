using Application;
using Infraestructure;
using Infraestructure.Configurations;
using Microsoft.Extensions.Options;
using Web.Auth.API.EndPoints;
using Web.Gateway.API;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddPresentationGateway(builder.Configuration).AddInfraestructureGateway(builder.Configuration).AddAplicationGateway();
builder.Logging.ClearProviders();
var app = builder.Build();
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseCors("AllowOrigin");
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API V1");
        options.RoutePrefix = "swagger";
    });
}
// Configure the HTTP request pipeline.
// Mapeamos los endpoints
var authSettings = app.Services.GetRequiredService<IOptions<AuthorizationSettings>>().Value;
AuthEndpoints.DefineEndpoints(app, authSettings);

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();




