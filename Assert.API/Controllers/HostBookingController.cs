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
    }
}
