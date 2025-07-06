using Assert.Application.DTOs.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.Validators;

internal class ChangePasswordRequestValidator
    : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("La nueva contraseña no puede ser vacia.")
            .MinimumLength(8).WithMessage("La contraseña tiene que ser minimo de 8 caracteres.")
            .MaximumLength(30).WithMessage("La contraseña no debe superar los 30 caracteres.")
            .Matches(@"^(?=.*[A-Z])(?=.*\d).+$").WithMessage("La contraseña debe contener al menos una letra mayúscula y un número."); ;

    }
}
