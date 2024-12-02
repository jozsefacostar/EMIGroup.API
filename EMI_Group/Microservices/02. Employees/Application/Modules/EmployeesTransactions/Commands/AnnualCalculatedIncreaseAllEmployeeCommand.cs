using Domain.Interfaces.IEmployeeTransactions;
using Domain.Primitives;
using MediatR;
using Shared;

namespace Application.Modules.EmployeesTransactions.Commands;
public record AnnualCalculatedIncreaseAllEmployeeCommand : IRequest<RequestResult<bool>>;

public sealed class AnnualCalculatedIncreaseAllEmployeeCommandHandler : IRequestHandler<AnnualCalculatedIncreaseAllEmployeeCommand, RequestResult<bool>>
{
    private readonly IEmployeeTransactionRepository _EmployeeRepository;
    private readonly IUnitOfWork _unitofWork;

    public AnnualCalculatedIncreaseAllEmployeeCommandHandler(IEmployeeTransactionRepository EmployeeRepository, IUnitOfWork unitOfWork)
    {
        _EmployeeRepository = EmployeeRepository ?? throw new ArgumentNullException(nameof(EmployeeRepository));
        _unitofWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<RequestResult<bool>> Handle(AnnualCalculatedIncreaseAllEmployeeCommand command, CancellationToken cancellationToken)
    {
        await _EmployeeRepository.AnnualCalculatedIncreaseAllEmployee();
        await _unitofWork.SaveChangesAsync();
        return RequestResult<bool>.SuccessOperation();
    }
}

