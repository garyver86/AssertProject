using Assert.API.Helpers;
using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Implementation;
using Assert.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assert.API.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IAppDashboardService _dashboardService;
        public DashboardController(IAppDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Servicio que devuelve el reporte de ingesos para el usuario logueado.
        /// </summary>
        /// <param name="request">Filtros de fechas/año para recuperar ingresos agrupados por mes.</param>
        /// <returns>reporte de ingresos.</returns>
        /// <response code="200">Si se proceso correctamente.</response>
        [HttpPost("GetRevenueReport")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> GetRevenueReport([FromBody] RevenueReportRequestDTO request)
        {
            var result = await _dashboardService.GetRevenueReportAsync(request);
            return result;
        }


        /// <summary>
        /// Servicio que devuelve el reporte de ingesos para el usuario logueado.
        /// </summary>
        /// <param name="year">Año del cual se desean recuperar los ingresos.</param>
        /// <returns>reporte de ingresos.</returns>
        /// <response code="200">Si se proceso correctamente.</response>
        [HttpGet("GetRevenueReportYear/{year}")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> GetRevenueReport(int year)
        {
            var result = await _dashboardService.GetRevenueReportByYearAsync(year);
            return result;
        }

        /// <summary>
        /// Servicio que devuelve el reporte de ingesos para un usuario en específico.
        /// </summary>
        /// <param name="request">Filtros de fechas/año para recuperar ingresos agrupados por mes.</param>
        /// <returns>reporte de ingresos.</returns>
        /// <response code="200">Si se proceso correctamente.</response>
        [HttpPost("GetRevenueReport/{userId}")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> GetRevenueReportById([FromBody] RevenueReportRequestDTO request, int userId)
        {
            var result = await _dashboardService.GetRevenueReportByUserAsync(request,userId);
            return result;
        }


        /// <summary>
        /// Servicio que devuelve el reporte de ingesos para un usuario en específico.
        /// </summary>
        /// <param name="year">Año del cual se desean recuperar los ingresos.</param>
        /// <returns>reporte de ingresos.</returns>
        /// <response code="200">Si se proceso correctamente.</response>
        [HttpGet("GetRevenueReport/{year}/{userId}")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> GetRevenueReport(int year, int userId)
        {
            var result = await _dashboardService.GetRevenueReportByYearAndUserAsync(year, userId);
            return result;
        }

        /// <summary>
        /// Recupera la información agregada del dashboard para un año específico, con opción de filtrar por mes.
        /// </summary>
        /// <param name="year">Año de referencia para el reporte.</param>
        /// <param name="month">
        /// Mes opcional para filtrar los datos. Si se proporciona, el reporte será mensual; si es nulo, se generará un resumen anual.
        /// </param>
        /// <returns>
        /// Objeto <see cref="DashboardInfoDTO"/> que contiene métricas financieras y de reservas, encapsulado en un <see cref="ReturnModelDTO{DashboardInfoDTO}"/>.
        /// </returns>
        /// <response code="200">OK: La información del dashboard fue obtenida correctamente.</response>
        /// <response code="400">BAD REQUEST: Los parámetros de entrada son inválidos o inconsistentes.</response>
        /// <response code="401">UNAUTHORIZED: El usuario no tiene permisos para acceder a esta información.</response>
        /// <response code="500">INTERNAL SERVER ERROR: Se produjo un error inesperado durante la operación.</response>
        /// <remarks>
        /// Características del servicio:
        /// - Devuelve métricas agregadas como total pagado, pagos próximos, reservas confirmadas, noches reservadas y promedios.
        /// - El desglose por período se adapta dinámicamente al filtro: por mes si se especifica, por año si no.
        /// - Útil para alimentar dashboards visuales y analizar el rendimiento de reservas.
        /// </remarks>
        [HttpGet]
        [Authorize(Policy = "GuestOrHost")]
        [Route("Dashboard/GetDashboardInfo")]
        public async Task<ReturnModelDTO<DashboardInfoDTO>> GetDashboardInfo(int year, int? month)
        => await _dashboardService.GetDashboardInfo(year, month);
    }
}
