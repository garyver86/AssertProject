using Assert.Application.DTOs.Requests;
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

        /// <summary>
        /// Registra o actualiza la cancelación de una reserva por parte del host.
        /// </summary>
        /// <param name="bookId">Identificador único de la reserva afectada.</param>
        /// <param name="request">Objeto <see cref="UpsertHostBookCancellationRequestDTO"/> que contiene los detalles de la cancelación.</param>
        /// <returns>
        /// Resultado de la operación como texto, encapsulado en un ReturnModelDTO.Data string: SAVED.
        /// </returns>
        /// <response code="200">OK: La cancelación fue registrada o actualizada correctamente.</response>
        /// <response code="400">BAD REQUEST: Datos inválidos o inconsistentes en la solicitud.</response>
        /// <response code="401">UNAUTHORIZED: El usuario no tiene permisos para realizar esta acción.</response>
        /// <response code="500">INTERNAL SERVER ERROR: Error inesperado durante el procesamiento.</response>
        /// <remarks>
        /// Características del servicio:
        /// - Permite registrar o modificar una cancelación iniciada por el host.
        /// - Requiere el motivo de cancelación y mensajes opcionales para el huésped.
        /// - El campo <c>BookCancellationId</c> determina si se trata de una inserción o actualización.
        /// - El campo <c>MessageTo</c> puede usarse para direccionar el mensaje al huésped.
        /// </remarks>
        [HttpPut]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{bookId}/UpsertHostBookCancellation")]
        public async Task<ReturnModelDTO<string>> UpsertHostBookCancellation(long bookId,
            [FromBody] UpsertHostBookCancellationRequestDTO request)
        => await _bookService.UpsertHostBookCancellation(request);

        /// <summary>
        /// Recupera la lista de cancelaciones realizadas por el host para una reserva específica.
        /// </summary>
        /// <param name="bookId">Identificador único de la reserva.</param>
        /// <returns>
        /// Lista de objetos <see cref="BookCancellationDTO"/> que representan las cancelaciones registradas por el host,
        /// encapsulada en un <see cref="ReturnModelDTO&lt;List&lt;BookCancellationDTO&gt;&gt;"/>.
        /// </returns>
        /// <response code="200">OK: La lista de cancelaciones fue obtenida correctamente.</response>
        /// <response code="400">BAD REQUEST: El identificador de reserva es inválido o no existe.</response>
        /// <response code="401">UNAUTHORIZED: El usuario no tiene permisos para acceder a esta información.</response>
        /// <response code="500">INTERNAL SERVER ERROR: Se produjo un error inesperado durante la operación.</response>
        /// <remarks>
        /// Características del servicio:
        /// - Devuelve todas las cancelaciones registradas por el host para la reserva indicada.
        /// - Cada elemento incluye motivo, mensajes y metadatos asociados.
        /// - Útil para auditar el historial de cancelaciones o validar el estado actual.
        /// </remarks>
        [HttpGet]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{bookId}/GetHostBookCancellation")]
        public async Task<ReturnModelDTO<List<BookCancellationDTO>>> GetHostBookCancellation(long bookId)
        => await _bookService.GetHostBookCancellation(bookId);
    }
}
