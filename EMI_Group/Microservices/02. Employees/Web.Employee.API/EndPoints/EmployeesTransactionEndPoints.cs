using MediatR;
using Shared;
using Application.Responses;
using Application.Modules.EmployeesTransactions.Commands;
using Infraestructure.Configurations;
using Web.Employee.API.Common;
using Domain.Entities;

namespace Web.Employee.API.EndPoints;

public class EmployeesTransactionEndPoints : IEndpoints
{
    private const string BaseRoute = "EmployeesTransaction";
    public static void DefineEndpoints(IEndpointRouteBuilder app, AuthSettings authSettings)
    {
        // Endpoint POST /EmployeesTransaction
        app.MapPost($"{BaseRoute}/AnnualCalculatedIncreaseAllEmployee", AnnualCalculatedIncreaseAllEmployee)
        .WithName("AnnualCalculatedIncreaseAllEmployee")
        .Produces<RequestResult<bool>>(200) // Respuesta 200 OK
        .Produces<RequestResult<bool>>(400) // Respuesta 400 Bad Request
        .Produces<RequestResult<bool>>(401) // Respuesta 401 Unauthorized
        .Produces<RequestResult<bool>>(403) // Respuesta 403 Forbidden


        .WithDescription("Función que calcula el aumento anual para todos los empleados")
        .WithOpenApi()
        .RequireAuthorization(authSettings.AdminRole);

        // Endpoint POST /EmployeesTransaction
        app.MapPost($"{BaseRoute}/RegisterEmployeeProject", RegisterEmployeeProject)
        .WithName("RegisterEmployeeProject")
        .Produces<RequestResult<Unit>>(200) // Respuesta 200 OK
        .Produces<RequestResult<Unit>>(400) // Respuesta 400 Bad Request
        .Produces<RequestResult<Unit>>(401) // Respuesta 401 Unauthorized
        .Produces<RequestResult<Unit>>(403) // Respuesta 403 Forbidden
        .WithDescription("Función que relaciona un empleado a un proyecto")
        .WithOpenApi()
        .RequireAuthorization(authSettings.AdminRole);
    }
    internal static async Task<RequestResult<bool>> AnnualCalculatedIncreaseAllEmployee(ISender mediator) => await mediator.Send(new AnnualCalculatedIncreaseAllEmployeeCommand());
    internal static async Task<RequestResult<Unit>> RegisterEmployeeProject(ISender mediator, int employee, int project) => await mediator.Send(new RegisterEmployeeProjectCommand(employee, project));
}





