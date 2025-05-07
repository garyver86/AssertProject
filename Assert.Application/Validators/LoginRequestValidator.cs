using Assert.Application.DTOs.Requests;
using FluentValidation;

namespace Assert.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("El nombre de usuario es obligatorio. Con formato de email.");
            //.EmailAddress().WithMessage("Formato de email inválido.");

        //RuleFor(x => x.Password)
        //    .NotEmpty().WithMessage("La contraseña es obligatoria.")
        //    .MinimumLength(6).WithMessage("Mínimo 6 caracteres.");

        RuleFor(x => x.Platform)
            .NotEmpty().WithMessage("La plataforma es requerida.")
            .Must(BeAValidPlatform).WithMessage("Plataforma no soportada.");
    }

    private bool BeAValidPlatform(string platform)
    {
        var validPlatforms = new[] { "apple", "google", "meta", "local" };
        return validPlatforms.Contains(platform);
    }
}
