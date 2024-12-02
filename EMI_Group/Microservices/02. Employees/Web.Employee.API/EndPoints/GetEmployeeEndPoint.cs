using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Application.Modules.Employee.Queries;
using Application.Responses;
using Infraestructure.Configurations;
using Web.Employee.API.Common;

namespace Web.Employee.API.EndPoints;
public class GetEmployeeEndpoints : IEndpoints
{
    private const string BaseRoute = "GetEmployees";
    public static void DefineEndpoints(IEndpointRouteBuilder app, AuthSettings authSettings)
    {
        // Endpoint GET /Employees
        app.MapGet($"{BaseRoute}", GetAllEmployees)
            .WithName("GetAllEmployees")
            .Produces<RequestResult<List<EmployeeResponseDTO>>>(200)
            .WithDescription("Obtener la lista de todos los empleados")
            .WithOpenApi()
            .RequireAuthorization();

        // Endpoint GET /Employees
        app.MapGet($"{BaseRoute}/GetByIdEmployee/{{id}}", GetByIdEmployee)
            .WithName("GetByIdEmployee")
            .Produces<RequestResult<List<EmployeeResponseDTO>>>(200)
            .WithDescription("Obtener un empleado")
            .WithOpenApi()
            .RequireAuthorization(authSettings.AdminRole);

        // Endpoint GET /Employees
        app.MapGet($"{BaseRoute}/GetEmployeesByDepartmentAndProjects/{{codeDeparment}}", GetEmployeesByDepartmentAndProject)
            .WithName("GetEmployeesByDepartmentAndProjects")
            .Produces<RequestResult<List<EmployeeResponseDTO>>>(200)
            .WithDescription("Busca todos los empleados que forman parte de un departamento específico y están trabajando en al menos un proyecto")
            .WithOpenApi()
            .RequireAuthorization(authSettings.AdminRole);
    }
    internal static async Task<RequestResult<List<EmployeeResponseDTO>>> GetAllEmployees(ISender mediator) => await mediator.Send(new GetAllEmployeesQuery());
    internal static async Task<RequestResult<EmployeeResponseDTO>> GetByIdEmployee([FromQuery] string idNum, ISender mediator) => await mediator.Send(new GetEmployeeByIdQuery(idNum));

    internal static async Task<RequestResult<EmployeeResponseDTO>> GetEmployeesByDepartmentAndProject([FromQuery] string idNum, ISender mediator) => await mediator.Send(new GetEmployeesByDepartmentAndProjectQuery(idNum));
}
