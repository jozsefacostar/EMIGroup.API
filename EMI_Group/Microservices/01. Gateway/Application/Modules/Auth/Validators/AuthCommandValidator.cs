using Application.Modules.Auth.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Auth.Validators
{
    public class AuthCommandValidator : AbstractValidator<AuthCommand>
    {
        public AuthCommandValidator()
        {
            RuleFor(r => r.userName).NotEmpty().WithName("Usuario");
            RuleFor(r => r.password).NotEmpty().WithName("Contraseña");
        }
    }
}

