using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Interfaces.Notifications;
using Assert.Domain.Notifications;
using Assert.Domain.Notifications.Settings;
using Assert.Infrastructure.Exceptions;
using Assert.Shared.Extensions;
using Azure.Core;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Runtime;
using System.Threading;

namespace Assert.Infrastructure.External.Notifications;

public class GmailNotification(
    IOptions<EmailSettings> emailOptions,
    SmtpClient _smtpClient,
    IExceptionLoggerService _exceptionLoggerService) 
    : IEmailNotification
{
    private readonly EmailSettings _emailSettings = emailOptions.Value;

    public async Task SendAsync(EmailNotification message)
    {
        try
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.From),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true
            };
            foreach (var recipient in message.To)
                mailMessage.To.Add(recipient);

            await _smtpClient.SendMailAsync(mailMessage);
        }
        catch(Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, new { message });
            throw new InfrastructureException(ex.Message);
        }
    }
}
