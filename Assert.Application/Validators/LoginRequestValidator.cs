using Assert.Application.DTOs.Requests;
using FluentValidation;

namespace Assert.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("El nombre de usuario es obligatorio. Con formato de email.");

        RuleFor(x => x.UserName)
            .EmailAddress().WithMessage("Formato de email inválido.")
            .When(x => !string.Equals(x.UserName, "ASSERT_WEB", StringComparison.OrdinalIgnoreCase));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(8).WithMessage("Mínimo 8 caracteres.")
            .When(x => x.Platform?.ToLower() == "local");

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
