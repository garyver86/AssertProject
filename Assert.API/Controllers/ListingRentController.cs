﻿using Assert.API.Helpers;
using Assert.Application.DTOs.Responses;
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
        [Authorize(Policy = "Guest")]
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
        /// <param name="pageNumber">Pagina o grupo de resultados que se quieren obtener. (Por defecto 1).</param>
        /// <param name="pageSize">Cantidad de propiedades destacadas que se quieren obtener por página (Por defecto 10).</param>
        /// <returns>Listado de propiedades que cumplen con los parametros ingresados y que se encuentran publicados y listos para rentar.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Actualmente no se tiene una restricción, pero se debe definir si existirán parametros obligatorios.
        /// --------------------------------------------------------------------------------------------------
        /// > Latitude y Longitude : Funcionan en conjunto con el Radius, el cual debe estar expresado en metros. En caso de ingresar estos parametros, las propiedades resultado devolverán la distancia aproximada hacia el punto de referencia ingresado.
        /// </remarks>
        [HttpGet("Search")]
        [Authorize(Policy = "Guest")]
        //public async Task<ReturnModelDTO<List<ListingRentDTO>>> Search([FromQuery] SearchFilters filters, [FromQuery] int? pageNumber = 1, [FromQuery] int? pageSize = 10)
        public async Task<ReturnModelDTO_Pagination> Search([FromQuery] SearchFilters filters, [FromQuery] int? pageNumber = 1, [FromQuery] int? pageSize = 10)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            var properties = await _searchService.SearchProperties(filters, pageNumber ?? 1, pageSize ?? 10, userId, requestInfo, true);

            //return properties;
            return new ReturnModelDTO_Pagination
            {
                Data = properties.Data.data,
                pagination = properties.Data.pagination,
                HasError = properties.HasError,
                ResultError = properties.ResultError,
                StatusCode = properties.StatusCode
            };
        }

        /// <summary>
        /// Servicio que devuelve un listado de las propiedades recien visitadas por el usuario
        /// </summary>
        /// <param name="pageNumber">Pagina o grupo de resultados que se quieren obtener. (Por defecto 1).</param>
        /// <param name="pageSize">Cantidad de propiedades destacadas que se quieren obtener por página (Por defecto 10).</param>
        /// <returns>Devolucion de la lista de propiedades visitadas y el detalle de la paginación.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        [HttpGet("ViewsHistory")]
        [Authorize(Policy = "Guest")]
        public async Task<ReturnModelDTO_Pagination> ViewsHistory([FromQuery] int? pageNumber = 1, [FromQuery] int? pageSize = 10)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            var properties = await _appListingFavoriteService.GetViewsHistory(userId, pageNumber ?? 1, pageSize ?? 10, requestInfo);

            return new ReturnModelDTO_Pagination
            {
                Data = properties.Data.data,
                pagination = properties.Data.pagination,
                HasError = properties.HasError,
                ResultError = properties.ResultError,
                StatusCode = properties.StatusCode
            };
        }


        /// <summary>
        /// Servicio que devuelve el detalle de grupos de favoritos del usuario
        /// </summary>
        /// <returns>Devolucion de la lista de grupos de favoritos.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        [HttpGet("Favorites")]
        [Authorize(Policy = "Guest")]
        public async Task<ReturnModelDTO<List<ListingFavoriteGroupDTO>>> Favorites()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            var properties = await _appListingFavoriteService.GetFavoriteGroups(userId, requestInfo);

            return new ReturnModelDTO<List<ListingFavoriteGroupDTO>>
            {
                Data = properties.Data,
                HasError = properties.HasError,
                ResultError = properties.ResultError,
                StatusCode = properties.StatusCode
            };
        }

        /// <summary>
        /// Servicio que devuelve el contenido de un grupo de favoritos del usuario
        /// </summary>
        /// <returns>Devolucion de la lista de grupos de favoritos.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        [HttpGet("Favorites/{groupId}")]
        [Authorize(Policy = "Guest")]
        public async Task<ReturnModelDTO<ListingFavoriteGroupDTO>> Favorites(long groupId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            var properties = await _appListingFavoriteService.GetFavoriteContent(groupId, userId, requestInfo);

            return new ReturnModelDTO<ListingFavoriteGroupDTO>
            {
                Data = properties.Data,
                HasError = properties.HasError,
                ResultError = properties.ResultError,
                StatusCode = properties.StatusCode
            };
        }

        /// <summary>
        /// Servicio que permite agregar una propiedad a los favoritos de un usuario.
        /// </summary>
        /// <param name="listingRentId">Id del linsting a marcar como favorito.</param>
        /// <param name="groupId">Id del grupo de favoritos.</param>
        /// <returns>Confirmación del marcado de la propiedad como favorito (StatusCode=200).</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// </remarks>
        [Authorize(Policy = "GuestOnly")]
        [HttpPost("{listingRentId}/favorite")]
        public async Task<ReturnModelDTO> AddToFavorites(long listingRentId, long groupId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            ReturnModelDTO result = await _appListingFavoriteService.ToggleFavorite(listingRentId, groupId, true, userId, requestInfo);
            return result;
        }


        /// <summary>
        /// Servicio que permite crear un grupo de favoritos.
        /// </summary>
        /// <param name="groupName">Nombre del grupo de favoritos.</param>
        /// <returns>Confirmación de la creacion del grupo(StatusCode=200).</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// </remarks>
        [Authorize(Policy = "GuestOnly")]
        [HttpPost("/favorite")]
        public async Task<ReturnModelDTO> CreateFavoriteGroup(string groupName)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            ReturnModelDTO result = await _appListingFavoriteService.CreateFavoriteGroup(groupName, userId, requestInfo);
            return result;
        }

        /// <summary>
        /// Servicio que permite eliminar un grupo de favoritos.
        /// </summary>
        /// <param name="groupId">Id del grupo de favoritos a eliminar.</param>
        /// <returns>Confirmación de la eliminación del grupo(StatusCode=200).</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// </remarks>
        [Authorize(Policy = "GuestOnly")]
        [HttpDelete("/favorite/{groupId}")]
        public async Task<ReturnModelDTO> CreateFavoriteGroup(int groupId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            ReturnModelDTO result = await _appListingFavoriteService.RemoveFavoriteGroup(groupId, userId, requestInfo);
            return result;
        }

        /// <summary>
        /// Servicio que permite eliminar una propiedad de los favorios de un usuario.
        /// </summary>
        /// <param name="listingRentId">Id del linsting a eliminar de favoritos.</param>
        /// <param name="groupId">Id del grupo de favoritos.</param>
        /// <returns>Confirmación del marcado de la eliminación de la propiedad como favorito (StatusCode=200).</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// </remarks>
        [Authorize(Policy = "GuestOnly")]
        [HttpDelete("{listingRentId}/Favorite")]
        public async Task<ReturnModelDTO> RemoveFromFavorites(long listingRentId, long groupId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            ReturnModelDTO result = await _appListingFavoriteService.ToggleFavorite(listingRentId, groupId, false, userId, requestInfo);
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
        [Authorize(Policy = "GuestOnly")]
        [HttpDelete("Favorite/{listingRentId}")]
        public async Task<ReturnModelDTO> RemoveFromFavorites(long listingRentId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            ReturnModelDTO result = await _appListingFavoriteService.ToggleFavorite(listingRentId, null, false, userId, requestInfo);
            return result;
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
        [HttpPost("favorite/{listingRentId}")]
        public async Task<ReturnModelDTO> AddToFavorites(long listingRentId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            ReturnModelDTO result = await _appListingFavoriteService.ToggleFavorite(listingRentId, null, true, userId, requestInfo);
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
        [Authorize(Policy = "Guest")]
        [HttpGet("{listingRentId}/Reviews")]
        public async Task<ReturnModelDTO<List<ReviewDTO>>> GetReviews(int listingRentId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            ReturnModelDTO<List<ReviewDTO>> result = await _appListingRentService.GetListingRentReviews(listingRentId, false, requestInfo);
            return result;
        }

        /// <summary>
        /// Servicio que recupera un resumen de reviews de un listing Rent.
        /// </summary>
        /// <param name="listingRentId">Id del linsting.</param>
        /// <param name="topCount">Cantidad de registros a recuperar por lista.</param>
        /// <returns>Listado de resumen de reviews asociados al listing rent.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// </remarks>
        [Authorize(Policy = "Guest")]
        [HttpGet("{listingRentId}/Reviews/Summary")]
        public async Task<ReturnModelDTO<ListingReviewSummaryDTO>> GetReviewsSummary(int listingRentId, int topCount = 3)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            ReturnModelDTO<ListingReviewSummaryDTO> result = await _appListingRentService.GetListingRentReviewsSummary(listingRentId, topCount, false, requestInfo);
            return result;
        }

        /// <summary>
        /// Servicio que devuelve la lista de propiedades destacadas.
        /// </summary>
        /// <param name="countryId">id de país de la que se quieren obtener las propiedades destacadas (no es requerido).</param>
        /// <param name="pageNumber">Pagina o grupo de resultados que se quieren obtener. (Por defecto 1).</param>
        /// <param name="pageSize">Cantidad de propiedades destacadas que se quieren obtener por página (Por defecto 10).</param>
        /// <returns>Lista de propiedades destacadas. (StatusCode=200).</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Si se envia el countryId, las propiedades destacadas devueltas corresponteran al pais, caso contrario, se devolveran
        /// las propiedades destacadas de la totalidad. El calculo de propiedades destacadas se la realiza en base a las valoraciones o reviews.
        /// </remarks>
        [HttpGet("Featureds")]
        [Authorize(Policy = "Guest")]
        public async Task<ReturnModelDTO_Pagination> FeaturedListings([FromQuery] int? countryId, [FromQuery] int? pageNumber = 1, [FromQuery] int? pageSize = 10)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            (ReturnModelDTO<List<ListingRentDTO>> result, PaginationMetadataDTO pagination) = await _appListingRentService.GetFeaturedListings(userId, countryId, pageNumber ?? 1, pageSize ?? 10, requestInfo);
            return new ReturnModelDTO_Pagination
            {
                Data = result.Data,
                pagination = pagination,
                HasError = result.HasError,
                ResultError = result.ResultError,
                StatusCode = result.StatusCode
            };
        }
    }
}
