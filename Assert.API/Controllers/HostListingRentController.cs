using Assert.API.Helpers;
using Assert.Application.DTOs;
using Assert.Application.Interfaces;
using Assert.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assert.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("Host/ListingRent")]
    public class HostListingRentController : Controller
    {
        private readonly IAppListingRentService _appListingRentService;
        public HostListingRentController(IAppListingRentService appListingRentService)
        {
            _appListingRentService = appListingRentService;
        }


        /// <summary>
        /// Servicio que realiza el registro/edición de un listing rent.
        /// </summary>
        /// <param name="listinRentId">Id del linsting rent a recuperar.</param>
        /// <param name="request">Información ingresada en la vista correspondiente.</param>
        /// <returns>Datos necesarios para la siguiente vista, si es que corresponde.</returns>
        /// <response code="200">Si se procesó la información de la vista de forma correcta.</response>
        /// <remarks>
        /// El proceso de alta de un Listing Rent consta de 3 pasos, los cuales agrupan 16 vistas:<br />
        /// Paso 1: 6 vistas (Subtype, Accomodation Type, Address, Hosts-Bedrooms-Beds, Bathrooms-Stay, Precense)<br />
        /// Paso 2: 5 vistas (Amenities-Security Items, Photos, Title, FeaturedAspects, Description)<br />
        /// Paso 3: 5 vistas (Approval Policy, Pricing, Discounts, Security Confirmation, Review and confirmation)<br />
        /// </remarks>
        [HttpPost]
        [Authorize(Policy = "GuestOrHost")]
        [Route("ProcessData/{listinRentId}")]
        public async Task<ReturnModelDTO<ProcessDataResult>> ProcessData(long? listinRentId, [FromBody] ProcessDataRequest request)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            ReturnModelDTO<ProcessDataResult> result = await _appListingRentService.ProcessListingData(listinRentId ?? 0, request, requestInfo, request.UseTechnicalMessages);
            return result;
        }
    }
}
