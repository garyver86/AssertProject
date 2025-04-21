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
        private readonly IAppListingFavoriteService _appListingFavoriteService;
        private readonly IAppSearchService _searchService;
        public ListingRentController(IAppListingRentService appListingRentService, IAppSearchService searchService, IAppListingFavoriteService appListingFavoriteService)
        {
            _appListingRentService = appListingRentService;
            _searchService = searchService;
            _appListingFavoriteService = appListingFavoriteService;
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

        /// <summary>
        /// Servicio que permite agregar una propiedad a los favoritos de un usuario.
        /// </summary>
        /// <param name="listingRentId">Id del linsting a marcar como favorito.</param>
        /// <returns>Confirmación del marcado de la propiedad como favorito (StatusCode=200).</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// </remarks>
        [Authorize(Policy = "GuestOnly")]
        [HttpPost("{listingRentId}/favorite")]
        public async Task<ReturnModelDTO> AddToFavorites(int listingRentId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            ReturnModelDTO result = await _appListingFavoriteService.ToggleFavorite(listingRentId, true, userId, requestInfo);
            return result;
        }

        /// <summary>
        /// Servicio que permite eliminar una propiedad de los favorios de un usuario.
        /// </summary>
        /// <param name="listingRentId">Id del linsting a eliminar de favoritos.</param>
        /// <returns>Confirmación del marcado de la eliminación de la propiedad como favorito (StatusCode=200).</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// </remarks>
        [HttpDelete("{listingRentId}/Favorite")]
        public async Task<ReturnModelDTO> RemoveFromFavorites(int listingRentId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            ReturnModelDTO result = await _appListingFavoriteService.ToggleFavorite(listingRentId, false, userId, requestInfo);
            return result;
        }


        /// <summary>
        /// Servicio que recupera la lista de reviews de un listing Rent.
        /// </summary>
        /// <param name="listingRentId">Id del linsting.</param>
        /// <returns>Listado de reviews asociados al listing rent.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// </remarks>
        [HttpGet("{listingRentId}/Reviews")]
        public async Task<ReturnModelDTO<List<ReviewDTO>>> GetReviews(int listingRentId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            ReturnModelDTO<List<ReviewDTO>> result = await _appListingRentService.GetListingRentReviews(listingRentId, false, requestInfo);
            return result;
        }

        /// <summary>
        /// Servicio que devuelve la lista de propiedades destacadas.
        /// </summary>
        /// <param name="countryCode">Código de país de la que se quieren obtener las propiedades destacadas (no es requerido).</param>
        /// <param name="limit">Cantidad de propiedades destacadas que se quieren obtener (Por defecto 10).</param>
        /// <returns>Lista de propiedades destacadas. (StatusCode=200).</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Si se envia el countryId, las propiedades destacadas devueltas corresponteran al pais, caso contrario, se devolveran
        /// las propiedades destacadas de la totalidad. El calculo de propiedades destacadas se la realiza en base a los
        /// </remarks>
        [HttpGet("Featureds")]
        public async Task<ReturnModelDTO> FeaturedListings([FromQuery] int? countryId, [FromQuery] int? limit = 10)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            ReturnModelDTO result = await _appListingRentService.GetFeaturedListings(countryId, limit, requestInfo);
            return result;
        }
    }
}
