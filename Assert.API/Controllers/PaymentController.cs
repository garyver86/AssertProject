using Assert.API.Helpers;
using Assert.Application.DTOs;
using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Assert.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("Payment/")]
    public class PaymentController : Controller
    {
        private readonly IAppBookService _appBookService;
        public PaymentController(IAppBookService appBookService)
        {
            _appBookService = appBookService;
        }
        /// <summary>
        /// Servicio que actualiza informacion de precios de renta y descuentos
        /// </summary>
        /// <param name="request">Información relacionada al pago</param>
        /// <returns></returns>
        /// <response code="200">El valor de Data seria UPDATED</response>
        /// <remarks>
        /// CalculationCode: Codigo de la cotización
        /// Monto y Modena debe coincidir con la info de la cotización
        /// MethodOfPaymentId: 1 (Tarjetas de Débito/Crédito), 2 (QR)
        /// </remarks>
        [HttpPost]
        [Authorize(Policy = "GuestOrHost")]
        [Route("Simulate")]
        public async Task<ReturnModelDTO<BookDTO>> Simulate([FromBody] PaymentModel request)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);

            request.OrderCode = $"ord_{DateTime.UtcNow.ToString("yyyyMMddHHmm")}";
            request.Stan = new Guid().ToString();
            request.PaymentProviderId = 1;
            request.CountryId = 2;
            request.PaymentData = JsonConvert.SerializeObject(new { Info = "Payment Information" });
            request.TransactionData = JsonConvert.SerializeObject(new { Info = "Payment Information" });

            ReturnModelDTO<BookDTO> result = await _appBookService.SimulatePayment(request, userId, requestInfo, true);
            return result;
        }
    }
}
