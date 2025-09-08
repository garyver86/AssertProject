using Assert.API.Helpers;
using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Application.Services;
using Assert.Domain.Common.Metadata;
using Assert.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Assert.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class BookController : Controller
    {
        private readonly IAppBookService _bookService;
        private readonly IAppReviewService _reviewService;
        private readonly RequestMetadata _metadata;

        public BookController(IAppBookService bookService, RequestMetadata requestMetadata, IAppReviewService reviewService)
        {
            _bookService = bookService;
            _metadata = requestMetadata;
            _reviewService = reviewService;
        }

        /// <summary>
        /// Servicio q permite calcular el precio de una reserva para una propiedad en un periodo de tiempo.
        /// </summary>
        /// <param name="request">Objeto que contiene los datos necesarios para calcular el precio.</param>
        /// <param name="listinRentId">Id del listing rent para el cual se desea calcular el precio.</param>
        /// <returns>Respuesta paginada con los días del calendario</returns>
        /// <response code="200">Devuelve los días del calendario</response>
        /// <response code="400">Solicitud incorrecta</response>
        /// <response code="500">Error interno del servidor</response>
        /// <remarks>
        /// Si el propietario cambia el precio del alquiler durante esos 30 minutos, la cotización NO se actualiza automáticamente:
        /// •	El usuario podrá reservar al precio que se le cotizó, aunque el propietario haya cambiado el precio después.
        /// •	El monto, moneda y detalles de la cotización quedan “congelados” hasta la expiración(o hasta que el usuario reserve).
        /// ¿Por qué?
        /// Esto es una práctica común para evitar confusión y dar certeza al usuario que está reservando.
        /// Solo después de que la cotización expira, si el usuario solicita una nueva, se calculará con el nuevo precio.
        /// </remarks>
        [HttpPost("{listinRentId}/CalculatePrice")]
        public async Task<ReturnModelDTO<PayPriceCalculationDTO>> CalculatePrice(long listinRentId, [FromBody] PriceCalculationRequestDTO request)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);

            var result = await _bookService.CalculatePrice(listinRentId, request.startDate, request.endDate, userId,
           requestInfo, true);

            return new ReturnModelDTO<PayPriceCalculationDTO>
            {
                Data = result.Data.Item1,
                HasError = result.HasError,
                ResultError = result.ResultError,
                StatusCode = result.StatusCode,
            };
        }

        /// <summary>
        /// Servicio q permite calcular el precio de una reserva para una propiedad en un periodo de tiempo. Y devuelve un desglose de precios.
        /// </summary>
        /// <param name="request">Objeto que contiene los datos necesarios para calcular el precio.</param>
        /// <param name="listinRentId">Id del listing rent para el cual se desea calcular el precio.</param>
        /// <returns>Respuesta paginada con los días del calendario</returns>
        /// <response code="200">Devuelve los días del calendario</response>
        /// <response code="400">Solicitud incorrecta</response>
        /// <response code="500">Error interno del servidor</response>
        /// <remarks>
        /// Si el propietario cambia el precio del alquiler durante esos 30 minutos, la cotización NO se actualiza automáticamente:
        /// •	El usuario podrá reservar al precio que se le cotizó, aunque el propietario haya cambiado el precio después.
        /// •	El monto, moneda y detalles de la cotización quedan “congelados” hasta la expiración(o hasta que el usuario reserve).
        /// ¿Por qué?
        /// Esto es una práctica común para evitar confusión y dar certeza al usuario que está reservando.
        /// Solo después de que la cotización expira, si el usuario solicita una nueva, se calculará con el nuevo precio.
        /// </remarks>
        [HttpPost("{listinRentId}/CalculatePriceWithDetails")]
        public async Task<ReturnModelDTO<PayPriceCalculationCompleteDTO>> CalculatePriceWithDetails(long listinRentId, [FromBody] PriceCalculationRequestDTO request)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);

            var result = await _bookService.CalculatePrice(listinRentId, request.startDate, request.endDate, userId,
           requestInfo, true);

            return new ReturnModelDTO<PayPriceCalculationCompleteDTO>
            {
                Data = new PayPriceCalculationCompleteDTO
                {
                    PriceBreakdowns = result.Data.Item2,
                    PriceCalculation = result.Data.Item1
                },
                HasError = result.HasError,
                ResultError = result.ResultError,
                StatusCode = result.StatusCode
            };
        }

        /// <summary>
        /// Inserta/Actualiza una reserva
        /// </summary>
        /// <returns>Confirmación de la actualizacion: retorna el Id de la reserva</returns>
        /// <response code="200">Si se proceso correctamente.</response>
        /// <remarks>
        /// Si existe la reserva se envia el Id de esta caso contrario se envia 0 para indicar que es una nueva reserva.
        /// </remarks>
        [HttpPost("UpsertBookAsync")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> UpsertBookAsync([FromBody] BookDTO incomingBook)
        => await _bookService.UpsertBookAsync(incomingBook);

        /// <summary>
        /// Servicio que recupera una reserva esepecifica por Id.
        /// </summary>
        /// <returns>Confirmación de la actualizacion: retorna la informacion completa de la reserva</returns>
        /// <response code="200">Si se proceso correctamente.</response>
        /// <remarks>
        /// En caso de reserva vacia retorna error
        /// </remarks>
        [HttpPost("GetBookByIdAsync")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> GetBookByIdAsync([FromBody] long bookId)
        => await _bookService.GetBookByIdAsync(bookId);


        /// <summary>
        /// Servicio que recupera una reserva esepecifica por Id.
        /// </summary>
        /// <returns>Confirmación de la actualizacion: retorna la informacion completa de la reserva</returns>
        /// <response code="200">Si se proceso correctamente.</response>
        /// <remarks>
        /// En caso de reserva vacia retorna error
        /// </remarks>
        [HttpGet("{bookId}")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> GetBookById(long bookId)
        => await _bookService.GetBookByIdAsync(bookId);

        /// <summary>
        /// Servicio que recupera una lista de reservas del usuario que inicio sesion.
        /// </summary>
        /// <returns>Confirmación de la actualizacion: retorna la informacion completa de las reservas del usuario</returns>
        /// <response code="200">Si se proceso correctamente.</response>
        /// <remarks>
        /// En caso que no existan reservas para el usuario retorna error
        /// </remarks>
        [HttpPost("GetBookByUserIdAsync")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> GetBookByUserIdAsync()
        => await _bookService.GetBooksByUserIdAsync();

        /// <summary>
        /// Servicio que recupera una lista de reservas del usuario que inicio sesion.
        /// </summary>
        /// <returns>Confirmación de la actualizacion: retorna la informacion completa de las reservas del usuario</returns>
        /// <response code="200">Si se proceso correctamente.</response>
        /// <remarks>
        /// En caso que no existan reservas para el usuario retorna error
        /// </remarks>
        [HttpGet("GetBooks")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> GetBooks()
        => await _bookService.GetBooksByUserIdAsync();

        /// <summary>
        /// Servicio que recupera una lista de reservas del usuario que no cuentan con un review.
        /// </summary>
        /// <returns>Lista de reservas sin reviews.</returns>
        /// <response code="200">Si se proceso correctamente.</response>
        /// <remarks>
        /// En caso que no existan reservas para el usuario retorna error
        /// </remarks>
        [HttpGet("GetBooksWithoutReview")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> GetBooksWithoutReviewByUser()
        {
            var result = await _bookService.GetBooksWithoutReviewByUser(_metadata.UserId);
            return result;
        }

        /// <summary>
        /// Servicio que solicita la aprobación de una reserva.
        /// </summary>
        /// <param name="cotizationCode">Código de la cotización que se requiere aprobar para posteriormente pagar.</param>
        /// <returns>Información de la reserva.</returns>
        /// <response code="200">Si se proceso correctamente.</response>
        [HttpGet("RequestApproval/{cotizationCode}")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> BookingRequestApproval(string cotizationCode)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var result = await _bookService.BookingRequestApproval(new Guid(cotizationCode), _metadata.UserId,
            requestInfo, true);
            return result;
        }

        /// <summary>
        /// Servicio registra un review a una propiedad en base a una reserva.
        /// </summary>
        /// <param name="review">Objeto que contiene la información del review.</param>
        /// <param name="bookId">Id del re la reserva.</param>
        /// <returns>Lista de reservas sin reviews.</returns>
        /// <response code="200">Si se proceso correctamente.</response>
        [HttpPost("{bookId}/Review")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> SubmitBookReview(long bookId, [FromBody] ReviewDTO review)
        {
            review.BookId = bookId;
            var requestInfo = HttpContext.GetRequestInfo();
            var result = await _reviewService.SubmitBookReview(review, _metadata.UserId, requestInfo);
            return result;
        }


        /// <summary>
        /// Servicio que realiza la cancelación de una reserva.
        /// </summary>
        /// <param name="bookId">Id del la reserva.</param>
        /// <returns>información de la reserva.</returns>
        /// <response code="200">Si se proceso correctamente.</response>
        /// <remarks>
        /// Solo se podrá cancelar  una reserva si la misma se encuentra en estado PREBOOK (1) o APPROVED (2), en los otros casos devolverá error.
        /// </remarks>
        [HttpDelete("{bookId}/Cancel")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> Cancel(long bookId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var result = await _bookService.CancelBooking(_metadata.UserId, bookId);
            return result;
        }

        /// <summary>
        /// Servicio devuelve el detalle completo de un review.
        /// </summary>
        /// <param name="bookId">Id del re la reserva.</param>
        /// <returns>Lista de reservas sin reviews.</returns>
        /// <response code="200">Si se proceso correctamente.</response>
        [HttpGet("{bookId}/Review")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> GetBookReview(long bookId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var result = await _reviewService.GetBookReviewDetails(bookId, requestInfo);
            return result;
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
            var result = await _bookService.GetPendingAcceptanceForRenter(_metadata.UserId);
            return result;
        }
    }
}
