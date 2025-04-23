using Assert.API.Helpers;
using Assert.Application.DTOs.Responses;
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
        public ParametricsController(IAppSearchService searchService)
        {
            _searchService = searchService;
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
    }
}
