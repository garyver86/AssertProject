using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Common.Metadata;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Assert.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar operaciones relacionadas con el calendario de listados
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly IAppListingCalendarService _listingCalendarService;
        private readonly RequestMetadata _metadata;

        public CalendarController(IAppListingCalendarService listingCalendarService, RequestMetadata metadata)
        {
            _listingCalendarService = listingCalendarService;
            _metadata = metadata;
        }

        /// <summary>
        /// Obtiene los días del calendario para un listado específico con paginación
        /// </summary>
        /// <param name="listingRentId">ID del listado de renta</param>
        /// <param name="startDate">Fecha de inicio del rango (opcional)</param>
        /// <param name="endDate">Fecha de fin del rango (opcional)</param>
        /// <param name="pageNumber">Número de página (por defecto 1)</param>
        /// <param name="pageSize">Tamaño de página (por defecto 30, máximo 100)</param>
        /// <returns>Respuesta paginada con los días del calendario</returns>
        /// <response code="200">Devuelve los días del calendario</response>
        /// <response code="400">Solicitud incorrecta</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{listingRentId}")]
        public async Task<ReturnModelDTO<CalendarResultPaginatedDTO>> GetCalendarDays(
            [Range(1, int.MaxValue, ErrorMessage = "El ID del listado debe ser mayor que 0")]
            int listingRentId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery, Range(1, int.MaxValue, ErrorMessage = "El número de página debe ser mayor que 0")]
            int pageNumber = 1,
            [FromQuery, Range(1, 100, ErrorMessage = "El tamaño de página debe estar entre 1 y 100")]
            int pageSize = 30)
        {
            if (startDate > endDate)
            {
                return new ReturnModelDTO<CalendarResultPaginatedDTO>
                {
                    StatusCode = ResultStatusCode.BadRequest,
                    HasError = true,
                    ResultError = new ErrorCommonDTO
                    {
                        Message = "La fecha de inicio no puede ser mayor que la fecha de fin"
                    }
                };
            }


            ReturnModelDTO<CalendarResultPaginatedDTO> result = await _listingCalendarService.GetCalendarDays(
                listingRentId,
                startDate ?? DateTime.Today.AddMonths(-1),
                endDate ?? DateTime.Today.AddMonths(1),
                pageNumber,
                pageSize);

            return result;
        }

        /// <summary>
        /// Obtiene los días del calendario para un listado específico con paginación
        /// </summary>
        /// <param name="listingRentId">ID del listado de renta</param>
        /// <param name="startDate">Fecha de inicio del rango (opcional)</param>
        /// <param name="endDate">Fecha de fin del rango (opcional)</param>
        /// <param name="pageNumber">Número de página (por defecto 1)</param>
        /// <param name="pageSize">Tamaño de página (por defecto 30, máximo 100)</param>
        /// <returns>Respuesta paginada con los días del calendario</returns>
        /// <response code="200">Devuelve los días del calendario</response>
        /// <response code="400">Solicitud incorrecta</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{listingRentId}/WithDetails")]
        public async Task<ReturnModelDTO<CalendarResultPaginatedDTO>> GetCalendarDaysWithDetails(
            [Range(1, int.MaxValue, ErrorMessage = "El ID del listado debe ser mayor que 0")]
            int listingRentId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery, Range(1, int.MaxValue, ErrorMessage = "El número de página debe ser mayor que 0")]
            int pageNumber = 1,
            [FromQuery, Range(1, 100, ErrorMessage = "El tamaño de página debe estar entre 1 y 100")]
            int pageSize = 30)
        {
            if (startDate > endDate)
            {
                return new ReturnModelDTO<CalendarResultPaginatedDTO>
                {
                    StatusCode = ResultStatusCode.BadRequest,
                    HasError = true,
                    ResultError = new ErrorCommonDTO
                    {
                        Message = "La fecha de inicio no puede ser mayor que la fecha de fin"
                    }
                };
            }


            ReturnModelDTO<CalendarResultPaginatedDTO> result = await _listingCalendarService.GetCalendarDaysWithDetails(
                listingRentId,
                startDate ?? DateTime.Today.AddMonths(-1),
                endDate ?? DateTime.Today.AddMonths(1),
                pageNumber,
                pageSize);

            return result;
        }

        /// <summary>
        /// Obtiene los días del calendario para un listado específico con paginación
        /// </summary>
        /// <param name="listingRentId">ID del listado de renta</param>
        /// <param name="startDate">Fecha de inicio del rango (opcional)</param>
        /// <param name="endDate">Fecha de fin del rango (opcional)</param>
        /// <param name="pageNumber">Número de página (por defecto 1)</param>
        /// <param name="pageSize">Tamaño de página (por defecto 30, máximo 100)</param>
        /// <returns>Respuesta paginada con los días del calendario</returns>
        /// <response code="200">Devuelve los días del calendario</response>
        /// <response code="400">Solicitud incorrecta</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost("unblock")]
        public async Task<ReturnModelDTO<List<CalendarDayDto>>> BulkUnblockDays([FromBody] BulkBlockCalendarDaysRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new ReturnModelDTO<List<CalendarDayDto>>
                {
                    StatusCode = ResultStatusCode.BadRequest,
                    HasError = true,
                    ResultError = new ErrorCommonDTO
                    {
                        Message = "Datos de solicitud inválidos"
                    }
                };
            }

            // Validación adicional de fechas
            if (request.Dates.Any(d => d < DateOnly.FromDateTime(DateTime.Today)))
            {
                ModelState.AddModelError(nameof(request.Dates), "No se pueden desbloquear fechas pasadas");
                return new ReturnModelDTO<List<CalendarDayDto>>
                {
                    StatusCode = ResultStatusCode.BadRequest,
                    HasError = true,
                    ResultError = new ErrorCommonDTO
                    {
                        Message = "No se pueden desbloquear fechas pasadas"
                    }
                };
            }

            var result = await _listingCalendarService.UnblockDays(request, _metadata.UserId);

            return result;
        }

        /// <summary>
        /// Modifica las tarifas de noches específicas en el calendario de un listing
        /// </summary>
        /// <param name="listingRentId">ID del listing al que pretenecen las fechs</param>
        /// <param name="Dates">Listado de fechas a modificar</param>
        /// <param name="NightPrice">Precio a registar por los días ingresados</param>
        [HttpPost("SetNightPrice")]
        public async Task<ReturnModelDTO<List<CalendarDayDto>>> SetNightPriceDays([FromBody] BulkSetPriceDaysRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new ReturnModelDTO<List<CalendarDayDto>>
                {
                    StatusCode = ResultStatusCode.BadRequest,
                    HasError = true,
                    ResultError = new ErrorCommonDTO
                    {
                        Message = "Datos de solicitud inválidos"
                    }
                };
            }

            // Validación adicional de fechas
            if (request.Dates.Any(d => d < DateOnly.FromDateTime(DateTime.Today)))
            {
                ModelState.AddModelError(nameof(request.Dates), "No se pueden modificar tarifas de fechas pasadas");
                return new ReturnModelDTO<List<CalendarDayDto>>
                {
                    StatusCode = ResultStatusCode.BadRequest,
                    HasError = true,
                    ResultError = new ErrorCommonDTO
                    {
                        Message = "No se pueden modificar tarifas de fechas pasadas"
                    }
                };
            }

            var result = await _listingCalendarService.SetNightPriceDays(request.ListingRentId, request.Dates, request.NightPrice, _metadata.UserId);

            return result;
        }

        /// <summary>
        /// Obtiene los días del calendario para un listado específico con paginación
        /// </summary>
        /// <param name="listingRentId">ID del listado de renta</param>
        /// <param name="startDate">Fecha de inicio del rango (opcional)</param>
        /// <param name="endDate">Fecha de fin del rango (opcional)</param>
        /// <param name="pageNumber">Número de página (por defecto 1)</param>
        /// <param name="pageSize">Tamaño de página (por defecto 30, máximo 100)</param>
        /// <returns>Respuesta paginada con los días del calendario</returns>
        /// <response code="200">Devuelve los días del calendario</response>
        /// <response code="400">Solicitud incorrecta</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost("block")]
        public async Task<ReturnModelDTO<List<CalendarDayDto>>> BulkBlockDays([FromBody] BulkBlockCalendarDaysRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new ReturnModelDTO<List<CalendarDayDto>>
                {
                    StatusCode = ResultStatusCode.BadRequest,
                    HasError = true,
                    ResultError = new ErrorCommonDTO
                    {
                        Message = "Datos de solicitud inválidos"
                    }
                };
            }

            // Validación adicional de fechas
            if (request.Dates.Any(d => d < DateOnly.FromDateTime(DateTime.Today)))
            {
                ModelState.AddModelError(nameof(request.Dates), "No se pueden bloquear fechas pasadas");
                return new ReturnModelDTO<List<CalendarDayDto>>
                {
                    StatusCode = ResultStatusCode.BadRequest,
                    HasError = true,
                    ResultError = new ErrorCommonDTO
                    {
                        Message = "No se pueden bloquear fechas pasadas"
                    }
                };
            }

            var result = await _listingCalendarService.BlockDays(request, _metadata.UserId);

            return result;
        }
    }
}
