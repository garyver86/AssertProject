using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Common.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assert.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("Host/Booking")]
    public class HostBookingController : Controller
    {
        private readonly IAppBookService _appBookService;
        private readonly RequestMetadata _metadata;
        public HostBookingController(IAppBookService appBookService, RequestMetadata metadata)
        {
            _appBookService = appBookService;
            _metadata = metadata;
        }
        /// <summary>
        /// Servicio que devuelve la lista de reservas pendientes de aceptación
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <remarks>
        /// 
        /// </remarks>
        [HttpGet]
        [Authorize(Policy = "GuestOrHost")]
        [Route("PendingAcceptance")]
        public async Task<ReturnModelDTO<List<BookDTO>>> GetUnfinished()
        {
            var result = await _appBookService.GetPendingAcceptance(_metadata.UserId);
            return result;
        }
        /// <summary>
        /// Servicio que devuelve la lista de reservas que están aprobadas pero que no iniciaron (antes de la fecha de checkin)
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <remarks>
        /// 
        /// </remarks>
        [HttpGet]
        [Authorize(Policy = "GuestOrHost")]
        [Route("GetApprovedsWOInit")]
        public async Task<ReturnModelDTO<List<BookDTO>>> GetApprovedsWOInit()
        {
            var result = await _appBookService.GetApprovedsWOInit(_metadata.UserId);
            return result;
        }
        /// <summary>
        /// Servicio que devuelve la lista de reservas que pueden cancelarse, en base a la configuración de las politicas de cancelación
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <remarks>
        /// 
        /// </remarks>
        [HttpGet]
        [Authorize(Policy = "GuestOrHost")]
        [Route("GetCancelablesBookings")]
        public async Task<ReturnModelDTO<List<BookDTO>>> GetCancelablesBookings()
        {
            var result = await _appBookService.GetCancelablesBookings(_metadata.UserId);
            return result;
        }

        /// <summary>
        /// Servicio que devuelve autoriza una solicitud de reserva
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <remarks>
        /// 
        /// </remarks>
        [HttpPut]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{bookId}/Approve")]
        public async Task<ReturnModelDTO<BookDTO>> Approved(long bookId)
        {
            var result = await _appBookService.AuthorizationResponse(_metadata.UserId, bookId, true, null);
            return result;
        }

        /// <summary>
        /// Servicio que devuelve rechaza una solicitud de reserva
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <remarks>
        /// 
        /// </remarks>
        [HttpPut]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{bookId}/Refuse")]
        public async Task<ReturnModelDTO<BookDTO>> Refuse(long bookId, [FromBody] int reasonRefused)
        {
            var result = await _appBookService.AuthorizationResponse(_metadata.UserId, bookId, false,reasonRefused);
            return result;
        }
    }
}
