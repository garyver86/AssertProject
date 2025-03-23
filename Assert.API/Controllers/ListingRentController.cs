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
    [Route("[controller]")]
    public class ListingRentController : Controller
    {
        private readonly IAppListingRentService _appListingRentService;
        private readonly IAppSearchService _searchService;
        public ListingRentController(IAppListingRentService appListingRentService, IAppSearchService searchService)
        {
            _appListingRentService = appListingRentService;
            _searchService = searchService;
        }


        /// <summary>
        /// Servicio que devuelve el detalle de un Listing Rent
        /// </summary>
        /// <param name="listinRentId">Id del linsting rent a recuperar.</param>
        /// <returns>Detalle de un listing rent.</returns>
        /// <response code="200">Detalle del Listing Rent</response>
        /// <remarks>
        /// Este servicio devuelve el detalle de un listing rent en base a su id, al tratarse de un servicio para Guest, solo se recuperará el listing si este se encuentra publicado.
        /// </remarks>
        [HttpGet("{listinRentId}")]
        [Authorize(Policy = "GuestOnly")]
        public async Task<ReturnModelDTO> Get(long listinRentId)
        {
            var requestInfo = HttpContext.GetRequestInfo();

            var result = await _appListingRentService.Get(listinRentId, true, requestInfo, true);
            return result;
        }

        /// <summary>
        /// Servicio que permite la busqueda de propiedades en base a distintos parámetros.
        /// </summary>
        /// <param name="filters">Diferentes valores que reducen el rango de busqueda de propiedades.</param>
        /// <returns>Listado de propiedades que cumplen con los parametros ingresados y que se encuentran publicados y listos para rentar.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Actualmente no se tiene una restricción, pero se debe definir si existirán parametros obligatorios.
        /// --------------------------------------------------------------------------------------------------
        /// > Latitude y Longitude : Funcionan en conjunto con el Radius, el cual debe estar expresado en metros. En caso de ingresar estos parametros, las propiedades resultado devolverán la distancia aproximada hacia el punto de referencia ingresado.
        /// </remarks>
        [HttpGet("Search")]
        [Authorize(Policy = "GuestOnly")]
        public async Task<ReturnModelDTO<List<ListingRentDTO>>> Search([FromQuery] SearchFilters filters)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var properties = await _searchService.SearchProperties(filters, requestInfo, true);

            return properties;
        }
    }
}
