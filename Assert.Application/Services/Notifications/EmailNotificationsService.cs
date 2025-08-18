using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces.Notifications;
using Assert.Domain.Interfaces.Notifications;
using Assert.Domain.Models;
using Assert.Domain.Notifications;
using AutoMapper;

namespace Assert.Application.Services.Notifications;

public class EmailNotificationsService(
    IMapper _mapper,
    IEmailNotification _emailNotification)
    : IEmailNotificationService
{
    public async Task<ReturnModelDTO<string>> SendAsync(EmailNotificationRequestDTO notification)
    {
        if (notification is null)
            throw new ApplicationException("Los datos para envio de correo no pueden ser nulos.");

        var emailData = _mapper.Map<EmailNotification>(notification);

        await _emailNotification.SendAsync(emailData);

        return new ReturnModelDTO<string>
        {
            StatusCode = ResultStatusCode.OK,
            HasError = false,
            Data = "SENDED"
        };
    }
}
