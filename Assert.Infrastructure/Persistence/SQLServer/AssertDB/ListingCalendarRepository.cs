using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Assert.Domain.ValueObjects;
using Azure.Core;
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

        public async Task<List<TlListingCalendar>> BulkBlockDaysAsync(long listingRentId, List<DateOnly> dates, int blockType, string? blockReason, long? bookId)
        {
            List<TlListingCalendar> response = new List<TlListingCalendar>();
            var failedDates = new List<DateOnly>();

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Buscar días existentes
                var existingDays = await _context.TlListingCalendars
                    .Where(c => c.ListingrentId == listingRentId &&
                               dates.Contains(c.Date))
                    .ToListAsync();

                // 2. Actualizar días existentes
                var existingDates = existingDays.Select(d => d.Date).ToList();

                foreach (var day in existingDays)
                {
                    response.Add(day);
                    if (day.BlockType != null && bookId == null)
                    { // Si ya está bloqueado, no lo actualizamos
                        continue;
                    }
                    else if (bookId != null && bookId != null)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception($"Error al bloquear días en el calendario, el día {day.ToString()} ya se encuentra bloqueado.");
                    }
                    else
                    {
                        day.BlockType = (byte)blockType;
                        day.BlockReason = blockReason;
                        day.BookId = bookId; // Asignar el BookId si se proporciona
                    }
                }

                // 3. Insertar nuevos días bloqueados
                var newDates = dates.Except(existingDates).ToList();

                foreach (var date in newDates)
                {
                    try
                    {
                        var newDay = new TlListingCalendar
                        {
                            ListingrentId = listingRentId,
                            Date = date,
                            BlockType = (byte)blockType,
                            BlockReason = blockReason,
                            BookId = bookId // Asignar el BookId si se proporciona
                        };
                        _context.TlListingCalendars.Add(newDay);

                        response.Add(newDay);
                    }
                    catch
                    {
                        failedDates.Add(date);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                if (failedDates.Any())
                {

                }
                else
                {

                }
                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error al bloquear días en el calendario del listing rent", ex);
            }
        }

        public async Task<List<TlListingCalendar>> BulkUnblockDaysAsync(long listingRentId, List<DateOnly> dates)
        {
            List<TlListingCalendar> response = new List<TlListingCalendar>();
            var failedDates = new List<DateOnly>();
            //var dateTimes = dates.Select(d => d.ToDateTime(TimeOnly.MinValue)).ToList();

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Buscar días existentes
                var existingDays = await _context.TlListingCalendars
                    .Where(c => c.ListingrentId == listingRentId &&
                               dates.Contains(c.Date))
                    .ToListAsync();

                // 2. Actualizar días existentes
                var existingDates = existingDays.Select(d => d.Date).ToList();

                foreach (var day in existingDays)
                {
                    response.Add(day);
                    if (day.BlockType == 2) //Si está bloqueado por reserva, no actualizamos nada
                    {
                        throw new Exception($"la fecha {day.ToString()} se encuentra bloqueada por una reserva confirmada.");
                    }
                    else
                    {
                        day.BlockType = null;
                        day.BlockReason = null;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error al bloquear días en el calendario del listing rent", ex);
            }
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
                .Include(x => x.BlockTypeNavigation)
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
