using Assert.API.Helpers;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Common.Metadata;
using Assert.Domain.Models;
using Assert.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assert.API.Controllers
{
    public class AdministrationController(
        IAppUserService _userService,
        IAppListingRentService _appListingRentService,
        IAppParametricService _parametricService,
        RequestMetadata _metadata) : Controller
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
        [HttpGet("ListingRent/RecentlyPublished")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO_Pagination> RecentlyPublished([FromQuery] SearchFiltersToListingRent filters,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        => await _appListingRentService.GetLatestPublished(filters, pageNumber, pageSize);

        /// <summary>
        /// Servicio que devuelve los ListingRent ordenados por mayor alquileres
        /// </summary>
        /// <returns>Detalle del listing rent con mayor cantidad de alquileres.</returns>
        /// <response code="200">Lista del Listing Rent</response>
        /// <remarks>
        /// Este servicio devuelve los ListingRent ordenados por mayor alquileres. 
        /// </remarks>
        [HttpGet("ListingRent/MostRented")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO_Pagination> GetSortedByMostRentalsAsync([FromQuery] SearchFiltersToListingRent filters,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        => await _appListingRentService.GetSortedByMostRentalsAsync(filters, pageNumber, pageSize);

        /// <summary>
        /// Servicio que bloquea un ListingRent específico.
        /// </summary>
        /// <param name="listingRentId">ID del ListingRent a bloquear.</param>
        /// <returns>Resultado de la operación de bloqueo.</returns>
        /// <response code="200">ListingRent bloqueado exitosamente.</response>
        /// <response code="404">No se encontró el ListingRent especificado.</response>
        /// <remarks>
        /// Este servicio permite a un administrador cambiar el estado de un ListingRent a "BLOCKED".
        /// Se registra la acción en el log con la información del usuario.
        /// </remarks>
        [HttpPut("ListingRent/Blocked")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> Blocked(int listingRentId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            return await _appListingRentService.ChangeStatusByAdmin(listingRentId, _metadata.UserId, "BLOCKED", requestInfo);
        }

        /// <summary>
        /// Servicio que desbloquea un ListingRent previamente bloqueado.
        /// </summary>
        /// <param name="listingRentId">ID del ListingRent a desbloquear.</param>
        /// <returns>Resultado de la operación de desbloqueo.</returns>
        /// <response code="200">ListingRent publicado exitosamente.</response>
        /// <response code="404">No se encontró el ListingRent especificado.</response>
        /// <remarks>
        /// Este servicio permite a un usuario Administrador cambiar el estado de un ListingRent a "PUBLISH", reactivando su visibilidad.
        /// Se registra la acción en el log con la información del usuario.
        /// </remarks>
        [HttpPut("ListingRent/Unblocked")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> Unblocked(int listingRentId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            return await _appListingRentService.ChangeStatusByAdmin(listingRentId, _metadata.UserId, "PUBLISH", requestInfo);
        }

        /// <summary>
        /// Obtiene una lista paginada de usuarios administradores
        /// </summary>
        /// <param name="filters">Filtros opcionales: busqueda por nombre o apellido</param>
        /// <param name="pageNumber">Numero de pagina para paginacion (por defecto: 1).</param>
        /// <param name="pageSize">Cantidad de elementos por pagina (por defecto: 20).</param>
        /// <returns>Modelo que contiene la lista de usuarios y metadatos de paginacion.</returns>
        /// <response code="200">Retorna la lista paginada de usuarios administradores.</response>
        /// <remarks>
        /// Este endpoint permite obtener usuarios que poseen un rol de Administrador, aplicando filtros opcionales por nombre/apellido y paginación.
        /// </remarks>
        [HttpGet("User/GetAdministrators")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO_Pagination> GetAdministrators([FromQuery] SearchFiltersToUser filters,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        => await _userService.GetUserByRoleCode(filters, "AD", pageNumber, pageSize);

        /// <summary>
        /// Obtiene una lista paginada de usuarios anfitriones
        /// </summary>
        /// <param name="filters">Filtros opcionales: busqueda por nombre o apellido</param>
        /// <param name="pageNumber">Numero de pagina para paginacion (por defecto: 1).</param>
        /// <param name="pageSize">Cantidad de elementos por pagina (por defecto: 20).</param>
        /// <returns>Modelo que contiene la lista de usuarios y metadatos de paginacion.</returns>
        /// <response code="200">Retorna la lista paginada de usuarios anfitriones.</response>
        /// <remarks>
        /// Este endpoint permite obtener usuarios que poseen un rol de Anfitrion, aplicando filtros opcionales por nombre/apellido y paginación.
        /// </remarks>
        [HttpGet("User/GetHosts")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO_Pagination> GetHosts([FromQuery] SearchFiltersToUser filters,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        => await _userService.GetUserByRoleCode(filters, "HO", pageNumber, pageSize);

        /// <summary>
        /// Obtiene una lista paginada de usuarios huespedes
        /// </summary>
        /// <param name="filters">Filtros opcionales: busqueda por nombre o apellido</param>
        /// <param name="pageNumber">Numero de pagina para paginacion (por defecto: 1).</param>
        /// <param name="pageSize">Cantidad de elementos por pagina (por defecto: 20).</param>
        /// <returns>Modelo que contiene la lista de usuarios y metadatos de paginacion.</returns>
        /// <response code="200">Retorna la lista paginada de usuarios huespedes.</response>
        /// <remarks>
        /// Este endpoint permite obtener usuarios que poseen un rol de Huesped, aplicando filtros opcionales por nombre/apellido y paginación.
        /// </remarks>
        [HttpGet("User/GetGuests")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO_Pagination> GetGuests([FromQuery] SearchFiltersToUser filters,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        => await _userService.GetUserByRoleCode(filters, "HO", pageNumber, pageSize);

        /// <summary>
        /// Bloquea usuario anfitrion: bloquea los ListingRent asociados.
        /// </summary>
        /// <param name="ownerId">ID del propietario cuyos ListingRent serán bloqueados.</param>
        /// <returns>Resultado de la operación de bloqueo masivo.</returns>
        /// <response code="200">Operación completada exitosamente. Se bloquearon los ListingRent del propietario.</response>
        /// <response code="404">No se encontraron ListingRent asociados al OwnerId especificado.</response>
        /// <remarks>
        /// Este endpoint permite a un administrador cambiar el estado de todos los ListingRent de un propietario a "BLOCKED".
        /// </remarks>
        [HttpPut("User/BlockedHost")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> BlockedHost(int ownerId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            return await _appListingRentService.ChangeListingRentStatusByOwnerId(
                ownerId, "BLOCKED", requestInfo);
        }

        /// <summary>
        /// Desbloquea usuario anfitrion: desbloquea ListingRent asociados.
        /// </summary>
        /// <param name="ownerId">ID del propietario cuyos ListingRent serán desbloqueados.</param>
        /// <returns>Resultado de la operación de desbloqueo masivo.</returns>
        /// <response code="200">Operación completada exitosamente. Se desbloquearon los ListingRent del propietario.</response>
        /// <response code="404">No se encontraron ListingRent asociados al OwnerId especificado.</response>
        /// <remarks>
        /// Este endpoint permite a un administrador cambiar el estado de todos los ListingRent de un propietario a "PUBLISH".
        /// </remarks>
        [HttpPut("User/UnblockedHost")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> UnblockedHost(int ownerId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            return await _appListingRentService.ChangeListingRentStatusByOwnerId(
                ownerId, "PUBLISH", requestInfo);
        }

        /// <summary>
        /// Bloquea de un usuario huesped: No le permite ser anfitrion si intenta hacerlo y no le permite hacer reeservas.
        /// </summary>
        /// <param name="userId">ID del usuario a bloquear.</param>
        /// <returns>Resultado de la operación de bloqueo.</returns>
        /// <response code="200">Usuario bloqueado exitosamente.</response>
        /// <response code="404">No se encontró el usuario especificado.</response>
        /// <remarks>
        /// Este servicio permite a un administrador bloquear a un usuario huesped,
        /// No le permite ser anfitrion si intenta hacerlo y no le permite hacer reeservas.
        /// </remarks>
        [HttpPut("User/BlockedGuest")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> BlockedGuest(int userId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            return await _userService.ChangeUserStatus(userId, "UN");
        }

        /// <summary>
        /// Desbloquea de un usuario huesped: Le permite ser anfitrion y le permite hacer reeservas nuevamente.
        /// </summary>
        /// <param name="userId">ID del usuario a desbloquear.</param>
        /// <returns>Resultado de la operación de desbloqueo.</returns>
        /// <response code="200">Usuario desbloqueado exitosamente.</response>
        /// <response code="404">No se encontró el usuario especificado.</response>
        /// <remarks>
        /// Este servicio permite a un administrador desbloquear a un usuario huesped,
        /// Le permite ser anfitrion y le permite hacer reeservas nuevamente.
        /// </remarks>
        [HttpPut("User/UnblockedGuest")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> UnblockedGuest(int userId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            return await _userService.ChangeUserStatus(userId, "AC");
        }

        /// <summary>
        /// Bloqueo de un usuario completo, no le permite realizar ninguna accion en la aplicacion
        /// </summary>
        /// <param name="userId">ID del usuario a bloquear.</param>
        /// <returns>Resultado de la operación de bloqueo.</returns>
        /// <response code="200">Usuario bloqueado exitosamente.</response>
        /// <response code="404">No se encontró el usuario especificado.</response>
        /// <remarks>
        /// Este servicio permite a un administrador bloquear por completo a un usuario,
        /// No le permite ingresar a la aplicacion.
        /// </remarks>
        [HttpPut("User/Inactive")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> Inactive(int userId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var blockedUser = await _userService.ChangeUserStatus(userId, "IN");
            var blockedListings = await _appListingRentService.ChangeListingRentStatusByOwnerId(
                userId, "BLOCKED", requestInfo);
            return blockedListings;
        }

        /// <summary>
        /// Bloqueo de un usuario completo, no le permite realizar ninguna accion en la aplicacion
        /// </summary>
        /// <param name="userId">ID del usuario a bloquear.</param>
        /// <returns>Resultado de la operación de bloqueo.</returns>
        /// <response code="200">Usuario bloqueado exitosamente.</response>
        /// <response code="404">No se encontró el usuario especificado.</response>
        /// <remarks>
        /// Este servicio permite a un administrador bloquear por completo a un usuario,
        /// No le permite ingresar a la aplicacion.
        /// </remarks>
        [HttpPut("User/Active")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> Active(int userId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var blockedUser = await _userService.ChangeUserStatus(userId, "AC");
            var blockedListings = await _appListingRentService.ChangeListingRentStatusByOwnerId(
                userId, "PUBLISH", requestInfo);
            return blockedListings;
        }

        /// <summary>
        /// Crea o actualiza (upsert) la tarifa de Assert para un país específico.
        /// Actualiza la fila existente a nivel país (CountryId == countryId y City/County/State == null)
        /// o inserta una nueva fila si no existe.
        /// </summary>
        /// <param name="countryId">ID del país al que aplica la tarifa.</param>
        /// <param name="feePercent">Porcentaje de la comisión (opcional).</param>
        /// <param name="feeBase">Monto base de la comisión (opcional).</param>
        /// <returns>ReturnModelDTO con el resultado de la operación y un mensaje o código.</returns>
        [HttpPut("AssertFee/UpsertAssertFeeByCountry")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<string>> UpsertAssertFeeByCountry
            (int countryId, decimal? feePercent, decimal? feeBase)
        => await _parametricService.UpsertAssertFeeByCountry(countryId, feePercent, feeBase);
        

    }
}
