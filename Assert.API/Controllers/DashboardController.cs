using Assert.API.Helpers;
using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
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
    }
}
