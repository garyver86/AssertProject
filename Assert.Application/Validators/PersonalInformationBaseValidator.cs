using Assert.Application.DTOs.Bases;
using FluentValidation;

namespace Assert.Application.Validators;

internal class PersonalInformationBaseValidator<T>
    : AbstractValidator<T>
    where T : PersonalInformationBase
{
    public PersonalInformationBaseValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("El ID de usuario es obligatorio.");

        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage("El nombre no debe superar los 50 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.LastName)
            .MaximumLength(50).WithMessage("El apellido no debe superar los 50 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.LastName));

        RuleFor(x => x.FavoriteName)
            .MaximumLength(50).WithMessage("El nombre favorito no debe superar los 50 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.FavoriteName));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Phone)
            .Matches(@"^\+?[0-9]{1,5}-[0-9]{6,15}$")
            .WithMessage("El número de teléfono no tiene un formato válido. Use el formato +Código-Número.")
            .When(x => !string.IsNullOrEmpty(x.Phone));

    }
}
