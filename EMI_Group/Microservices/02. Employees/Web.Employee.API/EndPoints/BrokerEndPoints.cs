﻿
using Application.Modules.BrokerMessages.Commands;
using Infraestructure.Configurations;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Web.Employee.API.Common;

namespace Web.Employee.API.EndPoints
{
    public class BrokerEndPoints : IEndpoints
    {
        private const string BaseRoute = "brokers";
        public static void DefineEndpoints(IEndpointRouteBuilder app, AuthSettings authSettings)
        {
            //Endpoint POST / brokers
            app.MapPost($"{BaseRoute}/listen", ListenMessagesBroker)
                .WithName("ListenMessagesBroker")
                .Produces<RequestResult<bool>>(200)
                .WithDescription("Lee los mensajes de una cola")
                .WithOpenApi()
                .RequireAuthorization(authSettings.AdminRole);

            // Endpoint POST /brokers
            app.MapPost($"{BaseRoute}/publish", PublishMessageBroker)
                .WithName("PublishMessageBroker")
                .Produces<RequestResult<bool>>(200)
                .WithDescription("Publica un mensaje a una cola")
                .WithOpenApi()
                .RequireAuthorization(authSettings.AdminRole);
        }
        internal static async Task<RequestResult<bool>> ListenMessagesBroker([FromBody] ListenMessagesBrokerCommand command, ISender mediator) => await mediator.Send(command);
        internal static async Task<RequestResult<bool>> PublishMessageBroker([FromBody] PublishMessageBrokerCommand command, ISender mediator) => await mediator.Send(command);
    }
}