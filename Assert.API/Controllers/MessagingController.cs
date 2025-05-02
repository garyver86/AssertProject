using Assert.API.Helpers;
using Assert.API.Models;
using Assert.Application.DTOs;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Application.Services;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assert.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class MessagingController : Controller
    {
        private readonly IAppMessagingService _messagingService;

        public MessagingController(IAppMessagingService messagingService)
        {
            _messagingService = messagingService;
        }

        /// <summary>
        /// Servicio que crea una converzación entre un renter y un host
        /// </summary>
        /// <param name="request">Información de la converzación.</param>
        /// <returns>Converzación creada.</returns>
        /// <response code="200">Confirmación de la converrzación.</response>
        /// <remarks>
        /// Se valida que el usuario logueado sea el host o el renter.
        /// </remarks>
        [HttpPost("Conversations")]
        [Authorize(Policy = "GuestOrHost")]
        public async Task<ReturnModelDTO<ConversationDTO>> CreateConversation([FromBody] ConversationRequest request)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            ReturnModelDTO<ConversationDTO> result = await _messagingService.CreateConversation(request.RenterId, request.HostId, userId, requestInfo);
            return result;
        }

        /// <summary>
        /// Servicio que devuelve la lista de converzaciones del usuario logueado
        /// </summary>
        /// <returns>Lista de converzaciones.</returns>
        /// <remarks>
        /// </remarks>
        [HttpGet("Conversations")]
        [Authorize(Policy = "GuestOrHost")]
        public async Task<ReturnModelDTO<List<ConversationDTO>>> CreateConversation()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            ReturnModelDTO<List<ConversationDTO>> result = await _messagingService.GetConversations(userId, requestInfo);
            return result;
        }

        /// <summary>
        /// Servicio que crea una converzación entre un renter y un host
        /// </summary>
        /// <param name="conversationId">Información de la converzación.</param>
        /// <returns>Converzación creada.</returns>
        /// <response code="200">Confirmación de la converrzación.</response>
        /// <remarks>
        /// Se valida que el usuario logueado sea el host o el renter.
        /// </remarks>
        [HttpGet("Conversations/{conversationId}/Messages")]
        [Authorize(Policy = "GuestOrHost")]
        public async Task<ReturnModelDTO<List<MessageDTO>>> GetConversationMessages(int conversationId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string orderBy = "dateDesc")
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            ReturnModelDTO<List<MessageDTO>> result = await _messagingService.GetConversationMessages(conversationId, userId, page, pageSize, orderBy, requestInfo);
            return result;
        }

        /// <summary>
        /// Servicio que envía un mensaje a una converzación entre un renter y un host
        /// </summary>
        /// <param name="conversationId">Id de la converzación.</param>
        /// <param name="request">Mensaje a ser enviado.</param>
        /// <returns>Converzación creada.</returns>
        /// <response code="200">Confirmación de la converrzación.</response>
        /// <remarks>
        /// Se valida que el usuario logueado sea el host o el renter.
        /// </remarks>
        [HttpPost("Conversations/{conversationId}/Messages")]
        [Authorize(Policy = "GuestOrHost")]
        public async Task<ReturnModelDTO> SendMessages(int conversationId, [FromBody] SendMessageRequest request)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            ReturnModelDTO result = await _messagingService.SendMessage(conversationId, userId, request.Body, request.MessageTypeId, request.BookId, requestInfo);
            return result;
        }

        /// <summary>
        /// Servicio que marca varios mensajes como leidos
        /// </summary>
        /// <param name="conversationId">Id de la converzación.</param>
        /// <param name="messageIds">lista de mensajes a marcar como leidos.</param>
        /// <response code="200">Confirmación del marcado como leido.</response>
        /// <remarks>
        /// 
        /// </remarks>
        [HttpPut("Conversations/{conversationId}/Messages/Read")]
        [Authorize(Policy = "GuestOrHost")]
        public async Task<ReturnModelDTO> MarkAsRead(int conversationId, [FromBody] List<long> messageIds)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            ReturnModelDTO result = await _messagingService.SetAsRead(conversationId, userId, messageIds, requestInfo);
            return result;
        }

        /// <summary>
        /// Servicio que marca varios mensajes como no leidos
        /// </summary>
        /// <param name="conversationId">Id de la converzación.</param>
        /// <param name="messageIds">lista de mensajes a marcar como leidos.</param>
        /// <response code="200">Confirmación del marcado como leido.</response>
        /// <remarks>
        /// 
        /// </remarks>
        [HttpPut("Conversations/{conversationId}/Messages/Unread")]
        [Authorize(Policy = "GuestOrHost")]
        public async Task<ReturnModelDTO> MarkAsUnread(int conversationId, [FromBody] List<long> messageIds)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            ReturnModelDTO result = await _messagingService.SetAsUnread(conversationId, userId, messageIds, requestInfo);
            return result;
        }

        /// <summary>
        /// Servicio que devuelve la cantidad de mensaje no leidos del usuario
        /// </summary>
        /// <response code="200">Cantidad de mensajes no leidos.</response>
        /// <remarks>
        /// 
        /// </remarks>
        [HttpGet("Conversations/{conversationId}/Messages/Unread/Count")]
        [Authorize(Policy = "GuestOrHost")]
        public async Task<ReturnModelDTO> GetUnreadCount()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            ReturnModelDTO result = await _messagingService.GetUnreadCount(userId, requestInfo);
            return result;
        }
    }
}
