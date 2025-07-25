using Assert.API.Helpers;
using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
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

        public BookController(IAppBookService bookService)
        {
            _bookService = bookService;
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

            return result;
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
    }
}
