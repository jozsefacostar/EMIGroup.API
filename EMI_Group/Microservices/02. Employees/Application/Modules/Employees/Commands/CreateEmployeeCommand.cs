using Domain.Interfaces.IEmployee;
using Domain.Primitives;
using MediatR;
using Shared;

namespace Application.Modules.Employees.Commands;
public record CreateEmployeeCommand(string IdNum, string Name, int CurrentPosition, int deparment, decimal Salary, DateTime startDate, DateTime endDate) : IRequest<RequestResult<Unit>>;

public sealed class CreateEmployeeCommandHandle : IRequestHandler<CreateEmployeeCommand, RequestResult<Unit>>
{
    private readonly IWriteEmployeeRepository _EmployeeRepository;

    private readonly IUnitOfWork _unitofWork;

    public CreateEmployeeCommandHandle(IWriteEmployeeRepository EmployeeRepository, IUnitOfWork unitOfWork)
    {
        _EmployeeRepository = EmployeeRepository ?? throw new ArgumentNullException(nameof(EmployeeRepository));
        _unitofWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<RequestResult<Unit>> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var newEmployee = new Domain.Entities.Employee(command.IdNum, command.Name, command.CurrentPosition, command.deparment, command.Salary);
        var (success, result) = await _EmployeeRepository.Create(newEmployee, command.startDate, command.endDate);
        if (!success)
            return RequestResult<Unit>.ErrorRecord(result);
        await _unitofWork.SaveChangesAsync();
        return RequestResult<Unit>.SuccessRecord();
    }
}

