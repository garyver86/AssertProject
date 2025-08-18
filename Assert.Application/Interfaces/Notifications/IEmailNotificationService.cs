using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;

namespace Assert.Application.Interfaces.Notifications;

public interface IEmailNotificationService
{
    Task<ReturnModelDTO<string>> SendAsync(EmailNotificationRequestDTO notification);
}
