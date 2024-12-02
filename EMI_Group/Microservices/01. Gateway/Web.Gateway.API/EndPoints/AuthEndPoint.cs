namespace Web.Auth.API.EndPoints
{
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Shared;
    using Application.Modules.Auth.Commands;
    using global::Web.Auth.API.Common;
    using Application.Responses;
    using Infraestructure.Configurations;
    public class AuthEndpoints : IEndpoints
    {
        private const string BaseRoute = "Auth";

        // Método estático que define los endpoints
        public static void DefineEndpoints(IEndpointRouteBuilder app, AuthorizationSettings authSettings)
        {
            // Endpoint POST /Auth/Login
            app.MapPost($"{BaseRoute}/Login", Login)
                .WithName("Login")
                .Produces<RequestResult<Unit>>(200)
                .WithDescription("Permite loguear a un usuario")
                .WithOpenApi();

            // Endpoint POST /Auth/Create
            app.MapPost($"{BaseRoute}/Register", Register)
                .WithName("Create")
                .Produces<RequestResult<Unit>>(200) // Respuesta 200 OK
                .Produces<RequestResult<Unit>>(400) // Respuesta 400 Bad Request
                .Produces<RequestResult<Unit>>(401) // Respuesta 401 Unauthorized
                .Produces<RequestResult<Unit>>(403) // Respuesta 403 Forbidden
                .WithDescription("Permite crear un nuevo usuario")
                .WithOpenApi()
                .RequireAuthorization(authSettings.AdminRole);
        }

        internal static async Task<RequestResult<AuthResponseDTO>> Login([FromBody] AuthCommand command, ISender mediator) =>
            await mediator.Send(command);

        internal static async Task<RequestResult<AuthResponseDTO>> Register([FromBody] RegisterCommand command, ISender mediator) =>
            await mediator.Send(command);
    }
}
