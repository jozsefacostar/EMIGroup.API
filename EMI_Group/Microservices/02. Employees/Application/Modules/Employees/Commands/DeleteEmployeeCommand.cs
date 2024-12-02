using Domain.Interfaces.IEmployee;
using Domain.Primitives;
using MediatR;
using Shared;

namespace Application.Modules.Employees.Commands;
public record DeleteEmployeeCommand(string Id) : IRequest<RequestResult<Unit>>;

public sealed class DeleteEmployeeCommandHandle : IRequestHandler<DeleteEmployeeCommand, RequestResult<Unit>>
{
    private readonly IDeleteEmployeeRepository _EmployeeRepository;

    private readonly IUnitOfWork _unitofWork;

    public DeleteEmployeeCommandHandle(IDeleteEmployeeRepository EmployeeRepository, IUnitOfWork unitOfWork)
    {
        _EmployeeRepository = EmployeeRepository ?? throw new ArgumentNullException(nameof(EmployeeRepository));
        _unitofWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<RequestResult<Unit>> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        var (success, result) = await _EmployeeRepository.Delete(command.Id);
        if (!success)
            return RequestResult<Unit>.ErrorResult(result);
        await _unitofWork.SaveChangesAsync();
        return RequestResult<Unit>.SuccessDelete();
    }
}

