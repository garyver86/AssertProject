using Assert.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assert.API.Controllers
{
    [ApiController]
    [Route("TestAlert")]
    public class SignalRAlertController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<SignalRAlertController> _logger;

        public SignalRAlertController(
            INotificationService notificationService,
            ILogger<SignalRAlertController> logger)
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
                    request.ReceiverUserId,
                    request.Message
                );
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
        public int ReceiverUserId { get; set; }
        public string? Message { get; set; }
    }
}
