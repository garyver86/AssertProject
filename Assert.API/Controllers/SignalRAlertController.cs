using Assert.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assert.API.Controllers
{
    public class SignalRAlertController : Controller
    {
        [ApiController]
        [Route("api/[controller]")]
        public class NotificationsController : ControllerBase
        {
            private readonly INotificationService _notificationService;
            private readonly ILogger<NotificationsController> _logger;

            public NotificationsController(
                INotificationService notificationService,
                ILogger<NotificationsController> logger)
            {
                _notificationService = notificationService;
                _logger = logger;
            }

            /// <summary>
            /// Envía una notificación de solicitud de reserva a un host
            /// </summary>
            [HttpPost("booking-request")]
            public async Task<IActionResult> SendBookingRequestNotification([FromBody] BookingRequestNotification request)
            {
                try
                {
                    await _notificationService.SendBookingRequestNotificationAsync(
                        request.HostId,
                        request.ListingRentId,
                        request.BookingId
                    );

                    _logger.LogInformation($"Notificación de reserva enviada al host {request.HostId}");
                    return Ok(new { Message = "Notificación enviada exitosamente" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error enviando notificación de reserva");
                    return StatusCode(500, new { Error = "Error interno del servidor" });
                }
            }

            /// <summary>
            /// Envía una notificación de mensaje entre usuarios
            /// </summary>
            [HttpPost("user-message")]
            public async Task<IActionResult> SendUserMessageNotification([FromBody] UserMessageRequest request)
            {
                try
                {
                    await _notificationService.SendNewMessageNotificationAsync(
                        request.SenderUserId,
                        request.ReceiverUserId,
                        request.BookingId
                    );

                    _logger.LogInformation($"Mensaje enviado de {request.SenderUserId} a {request.ReceiverUserId}");
                    return Ok(new { Message = "Mensaje enviado exitosamente" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error enviando mensaje entre usuarios");
                    return StatusCode(500, new { Error = "Error interno del servidor" });
                }
            }
        }

        // DTOs para las requests
        public class BookingRequestNotification
        {
            public int HostId { get; set; }
            public int ListingRentId { get; set; }
            public int BookingId { get; set; }
        }

        public class UserNotificationRequest
        {
            public int UserId { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public string NotificationType { get; set; }
            public int? RelatedEntityId { get; set; } // BookingId, ListingId, etc.
        }

        public class UserMessageRequest
        {
            public int SenderUserId { get; set; }
            public int ReceiverUserId { get; set; }
            public int? BookingId { get; set; }
        }
    }
}
