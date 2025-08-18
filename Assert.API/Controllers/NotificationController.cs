using Assert.API.Helpers;
using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces.Notifications;
using Assert.Domain.Common.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assert.API.Controllers;

public class NotificationController(
    IEmailNotificationService _emailNotificationService,
    RequestMetadata _metadata) 
    : Controller
{
    /// <summary>
    /// Envía un correo electrónico de notificación.
    /// </summary>
    /// <param name="emailData">Información del correo a enviar, incluyendo destinatario, asunto y cuerpo.</param>
    /// <returns>Un objeto que indica el resultado del envío: SENDED</returns>
    /// <response code="200">Si el correo se envió correctamente.</response>
    /// <remarks>
    /// Este endpoint permite enviar notificaciones por correo electrónico usando el servicio configurado.
    /// Se recomienda que la información del destinatario y el contenido sean validados previamente.
    /// </remarks>
    [HttpPost("SendEmailAsync")]
    //[Authorize(Policy = "GuestOrHost")]
    public async Task<ReturnModelDTO> SendEmailAsync([FromBody] EmailNotificationRequestDTO emailData)
    => await _emailNotificationService.SendAsync(emailData);
}
