﻿using Domain.Interfaces.IEmployee;
using Domain.Primitives;
using MediatR;
using Shared;

namespace Application.Modules.Employees.Commands;
public record UpdateEmployeeCommand(string IdNum, string Name, int CurrentPosition, decimal Salary, int department, DateTime startDate, DateTime? endDate) : IRequest<RequestResult<Unit>>;

public sealed class UpdateEmployeeCommandHandle : IRequestHandler<UpdateEmployeeCommand, RequestResult<Unit>>
{
    private readonly IWriteEmployeeRepository _EmployeeRepository;

    private readonly IUnitOfWork _unitofWork;

    public UpdateEmployeeCommandHandle(IWriteEmployeeRepository EmployeeRepository, IUnitOfWork unitOfWork)
    {
        _EmployeeRepository = EmployeeRepository ?? throw new ArgumentNullException(nameof(EmployeeRepository));
        _unitofWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<RequestResult<Unit>> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var (success, result) = await _EmployeeRepository.Update(new Domain.Entities.Employee(command.IdNum, command.Name, command.CurrentPosition, command.department, command.Salary), command.startDate, command.endDate);
        if (!success)
            return RequestResult<Unit>.ErrorRecord(result);
        await _unitofWork.SaveChangesAsync();
        return RequestResult<Unit>.SuccessUpdate();
    }
}
