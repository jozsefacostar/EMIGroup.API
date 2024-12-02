using Domain.Entities;
using Domain.Interfaces.IEmployee;
using Domain.Interfaces.IEmployeeTransactions;
using Domain.Primitives;
using FluentValidation;
using MediatR;
using Shared;

namespace Application.Modules.EmployeesTransactions.Commands;
public record RegisterEmployeeProjectCommand(int idEmployee, int idproject) : IRequest<RequestResult<Unit>>;

public sealed class RegisterEmployeeProjectCommandHandle : IRequestHandler<RegisterEmployeeProjectCommand, RequestResult<Unit>>
{
    private readonly IEmployeeTransactionRepository _IEmployeeTransactionRepository;

    private readonly IUnitOfWork _unitofWork;

    public RegisterEmployeeProjectCommandHandle(IEmployeeTransactionRepository IEmployeeTransactionRepository, IUnitOfWork unitOfWork)
    {
        this._IEmployeeTransactionRepository = IEmployeeTransactionRepository ?? throw new ArgumentNullException(nameof(IEmployeeTransactionRepository));
        _unitofWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<RequestResult<Unit>> Handle(RegisterEmployeeProjectCommand command, CancellationToken cancellationToken)
    {
        var newEmployee = new EmployeeProject(command.idEmployee, command.idproject);
        var (success, result) = await _IEmployeeTransactionRepository.RegisterEmployeeProject(newEmployee);
        if (!success)
            return RequestResult<Unit>.ErrorRecord(result);
        await _unitofWork.SaveChangesAsync();
        return RequestResult<Unit>.SuccessRecord();
    }
}

