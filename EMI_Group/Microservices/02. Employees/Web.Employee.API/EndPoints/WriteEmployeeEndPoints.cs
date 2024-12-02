using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Application.Modules.Employees.Commands;
using Application.Modules.Employee.Queries;
using Application.Responses;
using Infraestructure.Configurations;
using Web.Employee.API.Common;

namespace Web.Employee.API.EndPoints;
public class WriteEmployeeEndpoints : IEndpoints
{
    private const string BaseRoute = "WriteEmployees";
    public static void DefineEndpoints(IEndpointRouteBuilder app, AuthSettings authSettings)
    {
        // Endpoint POST /Employees
        app.MapPost($"{BaseRoute}/CreateEmployee", CreateEmployee)
            .WithName("CreateEmployee")
            .Produces<RequestResult<Unit>>(200) // Respuesta 200 OK
            .Produces<RequestResult<Unit>>(400) // Respuesta 400 Bad Request
            .Produces<RequestResult<Unit>>(401) // Respuesta 401 Unauthorized
            .Produces<RequestResult<Unit>>(403) // Respuesta 403 Forbidden
            .WithDescription("Crear un nuevo empleado")
            .WithOpenApi()
            .RequireAuthorization(authSettings.AdminRole);

        // Endpoint PUT /Employees
        app.MapPut($"{BaseRoute}/UpdateEmployee", UpdateEmployee)
           .WithName("UpdateEmployee")
            .Produces<RequestResult<Unit>>(200) // Respuesta 200 OK
            .Produces<RequestResult<Unit>>(400) // Respuesta 400 Bad Request
            .Produces<RequestResult<Unit>>(401) // Respuesta 401 Unauthorized
            .Produces<RequestResult<Unit>>(403) // Respuesta 403 Forbidden
            .WithDescription("Modificar un empleado")
            .WithOpenApi()
            .RequireAuthorization(authSettings.AdminRole);
    }
    internal static async Task<RequestResult<Unit>> CreateEmployee([FromBody] CreateEmployeeCommand command, ISender mediator) => await mediator.Send(command);
    internal static async Task<RequestResult<Unit>> UpdateEmployee([FromBody] UpdateEmployeeCommand command, ISender mediator) => await mediator.Send(command);
}
