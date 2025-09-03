using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Users.RegisterUser
{
    internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(c => c.Nombre).NotEmpty().WithMessage("Nombre no puede estar vacio");
            RuleFor(c => c.Apellidos).NotEmpty().WithMessage("Los apellidos no pueden estar vacios");
            RuleFor(c => c.Email).EmailAddress();
            RuleFor(c => c.Password).NotEmpty().MinimumLength(5);
        }
    }
}
