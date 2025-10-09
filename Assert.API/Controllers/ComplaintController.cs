using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assert.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ComplaintController : Controller
    {
        private readonly IAppCompliantService _complaintService;

        public ComplaintController(IAppCompliantService complaintService)
        {
            _complaintService = complaintService;
        }

        /// <summary>
        /// Servicio que permite obtener una denuncia por su código único.
        /// </summary>
        /// <param name="complaintCode">Código único de la denuncia (Ej: COMP-2024-001).</param>
        /// <returns>Detalle completo de la denuncia.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <response code="404">Si no se encuentra la denuncia.</response>
        /// <remarks>
        /// El código de denuncia sigue el formato: COMP-YYYY-NNN
        /// </remarks>
        [HttpGet("ByCode/{complaintCode}")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<ComplaintDTO>> GetByComplaintCode(string complaintCode)
        {
            var result = await _complaintService.GetByComplaintCodeAsync(complaintCode);
            return result;
        }

        /// <summary>
        /// Servicio que permite obtener una denuncia asociada a una reserva específica.
        /// </summary>
        /// <param name="bookingId">Identificador de la reserva.</param>
        /// <returns>Detalle de la denuncia asociada a la reserva.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <response code="404">Si no existe denuncia para la reserva.</response>
        /// <remarks>
        /// Solo puede existir una denuncia por reserva.
        /// </remarks>
        [HttpGet("ByBooking/{bookingId}")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<ComplaintDTO>> GetByBookingId(int bookingId)
        {
            var result = await _complaintService.GetByBookingIdAsync(bookingId);
            return result;
        }

        /// <summary>
        /// Servicio que permite obtener las denuncias realizadas por un usuario.
        /// </summary>
        /// <param name="userId">Identificador del usuario denunciante.</param>
        /// <returns>Listado de denuncias realizadas por el usuario.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Solo se retornan las denuncias a las que el usuario tiene acceso.
        /// </remarks>
        [HttpGet("ByComplainant/{userId}")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<ComplaintDTO>>> GetByComplainantUserId(int userId)
        {
            var result = await _complaintService.GetByComplainantUserIdAsync(userId);
            return result;
        }

        /// <summary>
        /// Servicio que permite obtener las denuncias recibidas por un anfitrión.
        /// </summary>
        /// <param name="hostId">Identificador del anfitrión denunciado.</param>
        /// <returns>Listado de denuncias recibidas por el anfitrión.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Solo se retornan las denuncias a las que el usuario tiene acceso.
        /// </remarks>
        [HttpGet("ByReportedHost/{hostId}")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<ComplaintDTO>>> GetByReportedHostId(int hostId)
        {
            var result = await _complaintService.GetByReportedHostIdAsync(hostId);
            return result;
        }

        /// <summary>
        /// Servicio que permite filtrar denuncias con base en múltiples criterios.
        /// </summary>
        /// <param name="filter">Objeto con los criterios de filtrado.</param>
        /// <returns>Listado de denuncias que coinciden con los filtros.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Los filtros disponibles incluyen: estado, prioridad, fechas, etc.
        /// </remarks>
        [HttpPost("Filter")]
        [Authorize(Policy = "AdminOnly")] // Solo administradores pueden filtrar todas las denuncias
        public async Task<ReturnModelDTO<List<ComplaintDTO>>> GetFiltered([FromBody] ComplaintFilterDto filter)
        {
            var result = await _complaintService.GetFilteredAsync(filter);
            return result;
        }

        /// <summary>
        /// Servicio que permite obtener denuncias paginadas con filtros.
        /// </summary>
        /// <param name="filter">Objeto con los criterios de filtrado y paginación.</param>
        /// <returns>Listado paginado de denuncias y metadatos de paginación.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Incluye información de paginación (total de registros, página actual, etc.).
        /// </remarks>
        [HttpPost("Paginated")]
        [Authorize(Policy = "AdminOnly")] // Solo administradores pueden acceder a la paginación
        public async Task<ReturnModelDTO_Pagination> GetPaginated([FromBody] ComplaintFilterDto filter)
        {
            var result = await _complaintService.GetPaginatedAsync(filter);
            return new ReturnModelDTO_Pagination
            {
                Data = result.Data.Item1,
                HasError = result.HasError,
                pagination = result.Data.Item2,
                ResultError = result.ResultError,
                StatusCode = result.StatusCode
            };
        }

        /// <summary>
        /// Servicio que verifica si existe una denuncia para una reserva específica.
        /// </summary>
        /// <param name="bookingId">Identificador de la reserva.</param>
        /// <returns>True si existe denuncia, False en caso contrario.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Útil para validar antes de crear una nueva denuncia.
        /// </remarks>
        [HttpGet("ExistsForBooking/{bookingId}")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<bool> ExistsForBooking(int bookingId)
        {
            var exists = await _complaintService.ExistsForBookingAsync(bookingId);
            return exists;
        }

        /// <summary>
        /// Servicio que permite crear una nueva denuncia.
        /// </summary>
        /// <param name="complaintRequest">Datos de la denuncia a crear.</param>
        /// <returns>Denuncia creada con su información completa.</returns>
        /// <response code="200">Si se creó correctamente.</response>
        /// <response code="400">Si los datos de la denuncia no son válidos.</response>
        /// <response code="409">Si ya existe una denuncia para la reserva.</response>
        /// <remarks>
        /// Requiere que el motivo de denuncia esté activo y cumpla con las validaciones.
        /// </remarks>
        [HttpPost("Create")]
        [Authorize(Policy = "GuestOrHost")]
        public async Task<ReturnModelDTO<ComplaintDTO>> Create([FromBody] ComplaintRequestDTO complaintRequest)
        {
            var result = await _complaintService.AddAsync(complaintRequest);
            return result;
        }

        /// <summary>
        /// Servicio que permite actualizar una denuncia existente.
        /// </summary>
        /// <param name="complaintRequest">Datos actualizados de la denuncia.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="200">Si se actualizó correctamente.</response>
        /// <response code="404">Si no se encuentra la denuncia.</response>
        /// <remarks>
        /// Solo permite actualizar ciertos campos según el estado de la denuncia.
        /// </remarks>
        [HttpPut("Update")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<bool>> Update([FromBody] ComplaintRequestDTO complaintRequest)
        {
            await _complaintService.UpdateAsync(complaintRequest);
            return new ReturnModelDTO<bool>
            {
                HasError = false,
                StatusCode = "200",
                Data = true
            };
        }

        /// <summary>
        /// Servicio que permite actualizar el estado de una denuncia.
        /// </summary>
        /// <param name="complaintId">Identificador de la denuncia.</param>
        /// <param name="status">Nuevo estado de la denuncia.</param>
        /// <param name="adminUserId">Identificador del administrador que realiza el cambio (opcional).</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="200">Si se actualizó correctamente.</response>
        /// <response code="404">Si no se encuentra la denuncia.</response>
        /// <remarks>
        /// Solo usuarios administradores pueden cambiar el estado de las denuncias.
        /// Estados válidos: PENDING, UNDER_REVIEW, RESOLVED, DISMISSED, ESCALATED.
        /// </remarks>
        [HttpPatch("UpdateStatus/{complaintId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ReturnModelDTO<bool>> UpdateStatus(int complaintId, [FromQuery] string status, [FromQuery] int? adminUserId = null)
        {
            await _complaintService.UpdateStatusAsync(complaintId, status, adminUserId);
            return new ReturnModelDTO<bool>
            {
                HasError = false,
                StatusCode = "200",
                Data = true
            };
        }

        /// <summary>
        /// Servicio que permite asignar una denuncia a un administrador para su revisión.
        /// </summary>
        /// <param name="complaintId">Identificador de la denuncia.</param>
        /// <param name="adminUserId">Identificador del administrador asignado.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="200">Si se asignó correctamente.</response>
        /// <response code="404">Si no se encuentra la denuncia o el administrador.</response>
        /// <remarks>
        /// Al asignar una denuncia, su estado cambia automáticamente a UNDER_REVIEW.
        /// </remarks>
        [HttpPost("Assign/{complaintId}/{adminUserId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ReturnModelDTO<bool>> AssignComplaint(int complaintId, int adminUserId)
        {
            await _complaintService.AssignComplaintAsync(complaintId, adminUserId);
            return new ReturnModelDTO<bool>
            {
                HasError = false,
                StatusCode = "200",
                Data = true
            };
        }

        /// <summary>
        /// Servicio que permite resolver una denuncia.
        /// </summary>
        /// <param name="complaintId">Identificador de la denuncia.</param>
        /// <param name="resolutionInfo">Información de la resolución.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="200">Si se resolvió correctamente.</response>
        /// <response code="404">Si no se encuentra la denuncia.</response>
        /// <response code="400">Si los datos de resolución no son válidos.</response>
        /// <remarks>
        /// Al resolver una denuncia, su estado cambia automáticamente a RESOLVED.
        /// Se requiere especificar el tipo de resolución y notas explicativas.
        /// </remarks>
        [HttpPost("Resolve/{complaintId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ReturnModelDTO<bool>> ResolveComplaint(int complaintId, [FromBody] CompliantResolutionRequest resolutionInfo)
        {
            await _complaintService.ResolveComplaintAsync(complaintId, resolutionInfo);
            return new ReturnModelDTO<bool>
            {
                HasError = false,
                StatusCode = "200",
                Data = true
            };
        }

    }
}
