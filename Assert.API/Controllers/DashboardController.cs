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
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        [Route("Dashboard/GetDashboardInfo")]
        public async Task<ReturnModelDTO<DashboardInfoDTO>> GetDashboardInfo(int year, int? month)
        => await _dashboardService.GetDashboardInfo(year, month);

        /// <summary>
        /// Recupera el ranking de propiedades de un anfitrión en función de su rendimiento dentro de un rango de fechas.
        /// </summary>
        /// <param name="hostId">Identificador único del anfitrión.</param>
        /// <param name="startDate">Fecha de inicio del período a evaluar.</param>
        /// <param name="endDate">Fecha de fin del período a evaluar.</param>
        /// <returns>
        /// Lista de objetos <see cref="ListingRentRankingDTO"/> que representan cada propiedad del anfitrión,
        /// incluyendo ingresos, cantidad de reservas, ocupación porcentual y días ocupados,
        /// encapsulada en un <see cref="ReturnModelDTO{ListingRentRankingDTO}"/>.
        /// </returns>
        /// <response code="200">OK: El ranking de propiedades fue obtenido correctamente.</response>
        /// <response code="400">BAD REQUEST: Los parámetros de entrada son inválidos o inconsistentes.</response>
        /// <response code="401">UNAUTHORIZED: El usuario no tiene permisos para acceder a esta información.</response>
        /// <response code="500">INTERNAL SERVER ERROR: Se produjo un error inesperado durante la operación.</response>
        /// <remarks>
        /// Características del servicio:
        /// - Evalúa todas las reservas pagadas dentro del rango de fechas.
        /// - Calcula ingresos reales considerando descuentos, comisiones y tarifas adicionales.
        /// - Determina el factor de ocupación en base a días reservados versus días del período.
        /// - Ordena las propiedades por ingresos en orden descendente.
        /// - Útil para visualizar el rendimiento comparativo de propiedades en dashboards.
        /// </remarks>
        [HttpGet]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        [Route("Dashboard/GetPropertyRanking")]
        public async Task<ReturnModelDTO<ListingRentRankingDTO>> GetPropertyRanking(long hostId, DateTime startDate, DateTime endDate)
        => await _dashboardService.GetPropertyRanking(hostId, startDate, endDate);
    }
}
