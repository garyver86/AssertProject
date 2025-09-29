using Assert.Domain.Entities;
using Assert.Domain.ValueObjects;

namespace Assert.Domain.Repositories
{
    public interface IListingCalendarRepository
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
        Task<(List<TlListingCalendar> CalendarDays, PaginationMetadata Pagination)>
            GetCalendarDaysAsync(int listingRentId, DateTime startDate, DateTime endDate,
                               int pageNumber = 1, int pageSize = 30);

        /// <summary>
        /// Obtiene los días del calendario para un listing rent en un rango de fechas específico con información adicional y paginación
        /// </summary>
        /// <param name="listingRentId">ID del listing rent</param>
        /// <param name="startDate">Fecha de inicio del rango</param>
        /// <param name="endDate">Fecha de fin del rango</param>
        /// <param name="pageNumber">Número de página (base 1)</param>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <returns>Tupla con lista de días del calendario con información de descuentos e información de paginación</returns>
        Task<(List<TlListingCalendar> CalendarDays, PaginationMetadata Pagination)>
            GetCalendarDaysWithDetailsAsync(int listingRentId, DateTime startDate, DateTime endDate,
                                          int pageNumber = 1, int pageSize = 31);

        /// <summary>
        /// Bloquea los días de un calendar asociado a un listing
        /// </summary>
        /// <param name="listingRentId">ID del listing rent</param>
        /// <param name="dates">Fechas que desean ser bloqueadas</param>
        /// <param name="blockType">tipo de bloqueo (2: Reserva, 1: Bloqueo manual)</param>
        /// <param name="blockReason">Motivo del bloqueo.</param>
        /// <returns>Tupla con lista de días bloqueados</returns>
        Task<List<TlListingCalendar>> BulkBlockDaysAsync(long listingRentId, List<DateOnly> dates, int blockType, string? blockReason, long? bookId, int? userId);

        /// <summary>
        /// Desbloquea los días de un calendar asociado a un listing
        /// </summary>
        /// <param name="listingRentId">ID del listing rent</param>
        /// <param name="dates">Fechas que desean ser bloqueadas</param>
        /// <returns>Tupla con lista de días desbloqueados</returns>
        Task<List<TlListingCalendar>> BulkUnblockDaysAsync(long listingRentId, List<DateOnly> dates, int userId);

        Task<List<TlListingCalendar>> BulkSetNightPriceDaysAsync(long listingRentId, List<DateOnly> dates, decimal priceNigthly, int userId);
    }
}
