using Assert.API.Helpers;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Models;
using Assert.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assert.API.Controllers
{
    public class AdministrationController : Controller
    {

        private readonly IAppUserService _userService;
        public AdministrationController(IAppUserService appUserService)
        {
            _userService = appUserService;
        }
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
    }
}
