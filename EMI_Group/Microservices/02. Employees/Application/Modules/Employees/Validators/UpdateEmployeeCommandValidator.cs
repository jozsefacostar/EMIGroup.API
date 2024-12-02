using Application.Modules.Employees.Commands;
using FluentValidation;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(r => r.IdNum).NotEmpty().WithName("Identificación");
        RuleFor(r => r.Name).NotEmpty().MaximumLength(200).WithName("Nombre");
        RuleFor(r => r.CurrentPosition).NotEmpty().WithName("Posición actual");
        RuleFor(r => r.Salary).NotEmpty().GreaterThan(0).WithName("Salario");
    }
}