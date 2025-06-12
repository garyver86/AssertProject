using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
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

        public CalendarController(IAppListingCalendarService listingCalendarService)
        {
            _listingCalendarService = listingCalendarService;
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
    }
}
