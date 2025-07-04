using Assert.Application.DTOs.Requests;
using FluentValidation;

namespace Assert.Application.Validators;

internal class LocalUserRequestValidator 
    : AbstractValidator<LocalUserRequest>
{
    public LocalUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre no debe superar los 50 caracteres.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido es obligatorio.")
            .MaximumLength(50).WithMessage("El apellido no debe superar los 50 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
            .Matches("[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula.")
            .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número.");
            //.Matches("[^a-zA-Z0-9]").WithMessage("La contraseña debe contener al menos un carácter especial.");

        RuleFor(x => x.CountryId)
            .GreaterThan(0).WithMessage("El país es obligatorio.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("El número de teléfono es obligatorio.")
            .Matches(@"^\+?[0-9]{7,15}$").WithMessage("El número de teléfono no tiene un formato válido.");
    }
}
