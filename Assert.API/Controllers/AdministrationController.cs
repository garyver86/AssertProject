using Assert.API.Helpers;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Models;
using Assert.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assert.API.Controllers
{
    public class AdministrationController(
        IAppUserService _userService,
        IAppListingRentService _appListingRentService) : Controller
    {
        /// <summary>
        /// Servicio que permite la busqueda de usuarios hosts.
        /// </summary>
        /// <param name="filters">Diferentes valores que reducen el rango de busqueda de hosts.</param>
        /// <param name="pageNumber">Pagina o grupo de resultados que se quieren obtener. (Por defecto 1).</param>
        /// <param name="pageSize">Cantidad de propiedades destacadas que se quieren obtener por página (Por defecto 10).</param>
        /// <returns>Listado de hosts que cumplen con los parametros ingresados.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        [HttpGet("Hosts")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO_Pagination> SearchHosts([FromQuery] SearchFilters filters, [FromQuery] int? pageNumber = 1, [FromQuery] int? pageSize = 10)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            var properties = await _userService.SearchHosts(filters, pageNumber ?? 1, pageSize ?? 10, userId, requestInfo, true);

            //return properties;
            return new ReturnModelDTO_Pagination
            {
                Data = properties.Data.Item1,
                pagination = properties.Data.Item2,
                HasError = properties.HasError,
                ResultError = properties.ResultError,
                StatusCode = properties.StatusCode
            };
        }

        /// <summary>
        /// Servicio que devuelve los ultimos ListingRent publicados
        /// </summary>
        /// <returns>Detalle del últimos listing rent publicados.</returns>
        /// <response code="200">Detalle del Listing Rent</response>
        /// <remarks>
        /// Este servicio devuelve los últimos listing rent que se encuentran publicados. 
        /// </remarks>
        [HttpGet("ListingRent/Published")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> GetLatestPublished()
        => await _appListingRentService.GetLatestPublished();

        /// <summary>
        /// Servicio que devuelve los ListingRent ordenados por mayor alquileres
        /// </summary>
        /// <returns>Detalle del listing rent con mayor cantidad de alquileres.</returns>
        /// <response code="200">Lista del Listing Rent</response>
        /// <remarks>
        /// Este servicio devuelve los ListingRent ordenados por mayor alquileres. 
        /// </remarks>
        [HttpGet("ListingRent/MostRentals")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> GetSortedByMostRentalsAsync(
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        => await _appListingRentService.GetSortedByMostRentalsAsync(pageNumber, pageSize);
    }
}
