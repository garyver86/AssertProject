using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Common.Metadata;
using Assert.Domain.Services;
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
        private readonly IAppBookService _bookService;
        public HostBookingController(IAppBookService appBookService, RequestMetadata metadata, IAppBookService bookService)
        {
            _appBookService = appBookService;
            _metadata = metadata;
            _bookService = bookService;
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
            var result = await _appBookService.AuthorizationResponse(_metadata.UserId, bookId, false, reasonRefused);
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
        [Route("Consulting/{priceCalculationId}/Approve")]
        public async Task<ReturnModelDTO<PayPriceCalculationDTO>> ApproveConsulting(long priceCalculationId)
        {
            var result = await _appBookService.ConsultingResponse(_metadata.UserId, priceCalculationId, true, null);
            return result;
        }

        /// <summary>
        /// Servicio que recupera una lista de reservas del usuario que inicio sesion como owner(Pasando como filtro el estado de las reservas).
        /// </summary>
        /// <returns>Confirmación de la actualizacion: retorna la informacion completa de las reservas del usuario</returns>
        /// <response code="200">Si se proceso correctamente.</response>
        /// <remarks>
        /// En caso que no existan reservas para el usuario retorna error. Usar ALL para recuperar todas las reservas.
        /// </remarks>
        [HttpGet("GetBooks/{statusCode}")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> GetBooks(string statusCode)
        => await _bookService.GetBooksByOwnerIdAsync(statusCode);

        /// <summary>
        /// Servicio que recupera una lista de reservas pagadas del un usuario owner.
        /// </summary>
        /// <returns>Confirmación de la actualizacion: retorna la informacion completa de las reservas del usuario</returns>
        /// <response code="200">Si se proceso correctamente.</response>
        /// <remarks>
        /// En caso que no existan reservas para el usuario retorna error. 
        /// </remarks>
        [HttpGet("GetBooksPayeds/{userId}")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> GetBooksPayeds(int userId)
        => await _bookService.GetPayedsByOwnerId(userId);


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
        [Route("Consulting/{priceCalculationId}/Refuse")]
        public async Task<ReturnModelDTO<PayPriceCalculationDTO>> RefuseConsulting(long priceCalculationId, [FromBody] int reasonRefused)
        {
            var result = await _appBookService.ConsultingResponse(_metadata.UserId, priceCalculationId, false, reasonRefused);
            return result;
        }
    }
}
