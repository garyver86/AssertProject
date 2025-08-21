using Assert.API.Helpers;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Application.Interfaces.Notifications;
using Assert.Domain.Common.Metadata;
using Assert.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assert.API.Controllers
{
    [ApiController]
    [Authorize]
    public class AlertController : Controller
    {
        private readonly IAppNotificationService _appNotificationService;
        private readonly RequestMetadata _metadata;
        public AlertController(IAppNotificationService notificationService, RequestMetadata metadata)
        {
            _appNotificationService = notificationService;
            _metadata = metadata;
        }

        /// <summary>
        /// Servicio que retorna listado de alertas no leidas.
        /// </summary>
        /// <param name="filter">Filtro para busqueda de Alertas</param>
        /// <response code="200">Detalle del alertas</response>
        /// <remarks>
        /// En el filtro solo funciona la página y el tamaño de la página.
        /// </remarks>
        [HttpPost()]
        [Authorize(Policy = "GuestOrHost")]
        [Route("Alert")]
        public async Task<ReturnModelDTO_Pagination> GetNotifications([FromBody] NotificationHistoryFilter filter)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var result = await _appNotificationService.GetUserNotificationsAsync(_metadata.UserId, filter.Page, filter.PageSize, requestInfo, true);
            return new ReturnModelDTO_Pagination
            {
                Data = result.Data.Item1,
                pagination = result.Data.Item2,
                HasError = result.HasError,
                ResultError = result.ResultError,
                StatusCode = result.StatusCode
            };
        }

        /// <summary>
        /// Servicio que retorna el histórico de Alertas.
        /// </summary>
        /// <param name="filter">Filtro para busqueda de Alertas</param>
        /// <response code="200">Detalle del alertas</response>
        /// <remarks>
        /// </remarks>
        [HttpPost()]
        [Authorize(Policy = "GuestOrHost")]
        [Route("Alert/Historical")]
        public async Task<ReturnModelDTO_Pagination> GetHistoricalNotifications([FromBody] NotificationHistoryFilter filter)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var result = await _appNotificationService.GetHistoricalNotificationsAsync(_metadata.UserId, filter, requestInfo, true);
            return new ReturnModelDTO_Pagination
            {
                Data = result.Data.Item1,
                pagination = result.Data.Item2,
                HasError = result.HasError,
                ResultError = result.ResultError,
                StatusCode = result.StatusCode
            };
        }

        /// <summary>
        /// Servicio que retorna el detalle de una alerta.
        /// </summary>
        /// <param name="notificationId">Id de la alerta</param>
        /// <response code="200">Detalle del alertas</response>
        /// <remarks>
        /// </remarks>
        [HttpGet()]
        [Authorize(Policy = "GuestOrHost")]
        [Route("Alert/{notificationId}")]
        public async Task<ReturnModelDTO> GetNotification(long notificationId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var result = await _appNotificationService.GetNotificationAsync(notificationId, _metadata.UserId, requestInfo, true);
            return result;
        }

        /// <summary>
        /// Servicio que retorna la cantidad de alertas no leidas.
        /// </summary>
        /// <response code="200">Contador de alertas no leidas</response>
        /// <remarks>
        /// </remarks>
        [HttpGet()]
        [Authorize(Policy = "GuestOrHost")]
        [Route("Alert/GetUnreadCount")]
        public async Task<ReturnModelDTO> GetUnreadNotification()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var result = await _appNotificationService.GetUnreadCountAsync(_metadata.UserId, requestInfo, true);
            return result;
        }


        /// <summary>
        /// Servicio que marca como leida una alerta.
        /// </summary>
        /// <param name="notificationId">Id de la alerta</param>
        /// <remarks>
        /// </remarks>
        [HttpPut()]
        [Authorize(Policy = "GuestOrHost")]
        [Route("Alert/{notificationId}/MarkAsRead")]
        public async Task MarkAsRead(long notificationId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            await _appNotificationService.MarkAsReadAsync(notificationId, _metadata.UserId, requestInfo, true);
            return;
        }

        /// <summary>
        /// Servicio que marca como leidas todas las alertas.
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPut()]
        [Authorize(Policy = "GuestOrHost")]
        [Route("Alert/MarkAllAsRead")]
        public async Task MarkAllAsRead()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            await _appNotificationService.MarkAllAsReadAsync(_metadata.UserId, requestInfo, true);
            return;
        }
    }
}
