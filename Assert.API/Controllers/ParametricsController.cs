using Assert.API.Helpers;
using Assert.Application.DTOs;
using Assert.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assert.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ParametricsController : Controller
    {
        private readonly IAppSearchService _searchService;
        private readonly IAppParametricService _parametricService;
        public ParametricsController(IAppSearchService searchService, IAppParametricService parametricService)
        {
            _searchService = searchService;
            _parametricService = parametricService;
        }

        /// <summary>
        /// Servicio que permite la busqueda de ciudades.
        /// </summary>
        /// <param name="filter">Texto que permite filtrar las ciudades (Mínimo 3 caracteres).</param>
        /// <returns>Listado de ciudades que coinciden con la búsqueda.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Solo se consideraran las ciudades que no se encuentren deshabilitadas.
        /// </remarks>
        [HttpGet("City/{filter}")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<CountryDTO>>> Search(string filter)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var cities = await _searchService.SearchCities(filter, requestInfo, true);

            return cities;
        }
        
        /// <summary>
         /// Servicio que devuelve la lista de tipos de alojamientos activos.
         /// </summary>
         /// <returns>Listado de tipos de alojamientos.</returns>
         /// <response code="200">Si se procesó correctamente.</response>
         /// <remarks>
         /// Solo se consideraran los tipos de alojamiento que no se encuentren deshabilitadas.
         /// </remarks>
        [HttpGet("AccomodationType")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<AccomodationTypeDTO>>> AccomodationType()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var cities = await _parametricService.GetAccomodationTypes(requestInfo, true);

            return cities;
        }
    }
}
