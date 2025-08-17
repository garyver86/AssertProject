using Assert.Domain.Notifications;

namespace Assert.Domain.Interfaces.Notifications;

public interface IEmailNotification
{
    Task SendAsync(EmailNotification message);
}
