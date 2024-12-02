using Application.Modules.Employees.Commands;
using FluentValidation;

namespace Application.Modules.Employees.Validators;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(r => r.IdNum).NotEmpty().WithName("Identificación");
        RuleFor(r => r.Name).NotEmpty().MaximumLength(200).WithName("Nombre");
        RuleFor(r => r.CurrentPosition).NotEmpty().GreaterThan(0).WithName("Posición actual");
        RuleFor(r => r.Salary).NotEmpty().GreaterThan(0).WithName("Salario");
    }
}