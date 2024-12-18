﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Application.Modules.Employees.Commands;
using Application.Modules.Employee.Queries;
using Application.Responses;
using Infraestructure.Configurations;
using Web.Employee.API.Common;

namespace Web.Employee.API.EndPoints;
public class DeleteEmployeeEndpoints : IEndpoints
{
    private const string BaseRoute = "DeleteEmployees";
    public static void DefineEndpoints(IEndpointRouteBuilder app, AuthSettings authSettings)
    {

        // Endpoint DELETE /Employees
        app.MapDelete($"{BaseRoute}/DeleteById/{{id}}", DeleteEmployee)
            .WithName("DeleteEmployee")
            .Produces<RequestResult<Unit>>(200) // Respuesta 200 OK
            .Produces<RequestResult<Unit>>(400) // Respuesta 400 Bad Request
            .Produces<RequestResult<Unit>>(401) // Respuesta 401 Unauthorized
            .Produces<RequestResult<Unit>>(403) // Respuesta 403 Forbidden
            .WithDescription("Elimina un empleado")
            .WithOpenApi()
            .RequireAuthorization(authSettings.AdminRole);
    }

    internal static async Task<RequestResult<Unit>> DeleteEmployee([FromQuery] string idNum, ISender mediator) => await mediator.Send(new DeleteEmployeeCommand(idNum));
}


