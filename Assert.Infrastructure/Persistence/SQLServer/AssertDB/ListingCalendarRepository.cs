using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Assert.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingCalendarRepository : IListingCalendarRepository
    {
        private readonly InfraAssertDbContext _context;
        public ListingCalendarRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }

        public async Task<(List<TlListingCalendar> CalendarDays, PaginationMetadata Pagination)> GetCalendarDaysAsync(int listingRentId, DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 30)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 30;

            DateOnly dateOnlyStart = DateOnly.FromDateTime(startDate);
            DateOnly dateOnlyEnd = DateOnly.FromDateTime(endDate);

            var baseQuery = _context.TlListingCalendars
                .Where(c => c.ListingrentId == listingRentId &&
                           c.Date >= dateOnlyStart &&
                           c.Date <= dateOnlyEnd)
                .Include(x=>x.BlockTypeNavigation)
                .OrderBy(c => c.Date)
                .AsNoTracking();

            var totalItemCount = await baseQuery.CountAsync();
            var items = await baseQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var paginationMetadata = new PaginationMetadata
            {
                TotalItemCount = totalItemCount,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize)
            };

            return (items, paginationMetadata);
        }

        public async Task<(List<TlListingCalendar> CalendarDays, PaginationMetadata Pagination)> GetCalendarDaysWithDetailsAsync(int listingRentId, DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 31)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 30;

            DateOnly dateOnlyStart = DateOnly.FromDateTime(startDate);
            DateOnly dateOnlyEnd = DateOnly.FromDateTime(endDate);

            var baseQuery = from calendar in _context.TlListingCalendars
                            where calendar.ListingrentId == listingRentId &&
                                  calendar.Date >= dateOnlyStart &&
                                  calendar.Date <= dateOnlyEnd
                            join discount in _context.TlCalendarDiscounts
                                on calendar.CalendarId equals discount.CalendarId into discounts
                            from discount in discounts.DefaultIfEmpty()
                            group new { calendar, discount } by new
                            {
                                calendar.Date,
                                calendar.Price,
                                calendar.BlockType,
                                calendar.BlockReason,
                                calendar.BookId,
                                calendar.MinimumStay,
                                calendar.MaximumStay,
                                calendar.BlockTypeNavigation.BlockTypeName
                            } into grouped
                            select new TlListingCalendar
                            {
                                Date = grouped.Key.Date,
                                Price = grouped.Key.Price,
                                BlockType = grouped.Key.BlockType,
                                BlockReason = grouped.Key.BlockReason,
                                BookId = grouped.Key.BookId,
                                MinimumStay = grouped.Key.MinimumStay,
                                MaximumStay = grouped.Key.MaximumStay,
                                TlCalendarDiscounts = new List<TlCalendarDiscount>(
                                    grouped.Select(x => x.discount).Where(d => d != null).Select(d => new TlCalendarDiscount
                                    {
                                        IsDiscount = d.IsDiscount,
                                        DiscountCalculated = d.DiscountCalculated,
                                        Porcentage = d.Porcentage
                                    })
                                ),
                                BlockTypeNavigation = new TCalendarBlockType
                                {
                                    BlockTypeName = grouped.Key.BlockTypeName
                                }
                            };

            var orderedQuery = baseQuery.OrderBy(x => x.Date);
            var totalItemCount = await orderedQuery.CountAsync();
            var items = await orderedQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var paginationMetadata = new PaginationMetadata
            {
                TotalItemCount = totalItemCount,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize)
            };

            return (items, paginationMetadata);
        }
    }
}
