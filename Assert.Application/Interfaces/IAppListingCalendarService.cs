using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.ValueObjects;

namespace Assert.Application.Interfaces
{
    public interface IAppListingCalendarService
    {
        /// <summary>
        /// Obtiene los días del calendario para un listing rent en un rango de fechas específico con paginación
        /// </summary>
        /// <param name="listingRentId">ID del listing rent</param>
        /// <param name="startDate">Fecha de inicio del rango</param>
        /// <param name="endDate">Fecha de fin del rango</param>
        /// <param name="pageNumber">Número de página (base 1)</param>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <returns>Tupla con lista de días del calendario e información de paginación</returns>
        Task<ReturnModelDTO<CalendarResultPaginatedDTO>> GetCalendarDays(int listingRentId, DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 30);

        /// <summary>
        /// Obtiene los días del calendario para un listing rent en un rango de fechas específico con información adicional y paginación
        /// </summary>
        /// <param name="listingRentId">ID del listing rent</param>
        /// <param name="startDate">Fecha de inicio del rango</param>
        /// <param name="endDate">Fecha de fin del rango</param>
        /// <param name="pageNumber">Número de página (base 1)</param>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <returns>Tupla con lista de días del calendario con información de descuentos e información de paginación</returns>
        Task<ReturnModelDTO<CalendarResultPaginatedDTO>> GetCalendarDaysWithDetails(int listingRentId, DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 31);

        Task<ReturnModelDTO<List<CalendarDayDto>>> BlockDays(BulkBlockCalendarDaysRequest request);
        Task<ReturnModelDTO<List<CalendarDayDto>>> UnblockDays(BulkBlockCalendarDaysRequest request);

    }
}
