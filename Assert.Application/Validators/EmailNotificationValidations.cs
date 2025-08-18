using Assert.Domain.Notifications;
using FluentValidation;

namespace Assert.Application.Validators;

public class EmailNotificationValidations : AbstractValidator<EmailNotification>
{
    public EmailNotificationValidations()
    {
        RuleFor(x => x.To)
            .NotEmpty().WithMessage("El o los destinatarios de correo no puede ser un campo vacio.");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("El asunto de correo no puede ser un campo vacio.");
       
        RuleFor(x => x.Body)
             .NotEmpty().WithMessage("El cuerpo de correo no puede ser un campo vacio.");

    }
}
