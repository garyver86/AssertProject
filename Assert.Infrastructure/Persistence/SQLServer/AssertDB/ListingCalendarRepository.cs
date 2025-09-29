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

        public async Task<List<TlListingCalendar>> BulkBlockDaysAsync(long listingRentId, List<DateOnly> dates, int blockType, string? blockReason, long? bookId, int? userId)
        {
            var listing = _context.TlListingRents.AsNoTracking().FirstOrDefault(x => x.ListingRentId == listingRentId);
            if (listing == null)
            {
                throw new ArgumentException("El listing rent especificado no existe.", nameof(listingRentId));
            }
            if (listing.OwnerUserId != userId && blockType == 1)
            {
                throw new UnauthorizedAccessException("El usuario no tiene permiso para modificar este listing rent.");
            }
            var executionStrategy = _context.Database.CreateExecutionStrategy();

            return await executionStrategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                List<TlListingCalendar> response = new();
                var failedDates = new List<DateOnly>();

                try
                {
                    // 1. Buscar días existentes
                    var existingDays = await _context.TlListingCalendars
                        .Where(c => c.ListingrentId == listingRentId && dates.Contains(c.Date))
                        .ToListAsync();

                    // 2. Verificar conflictos primero
                    var conflictingDays = existingDays
                        .Where(day => bookId != null && day.BookId != null && day.BookId != bookId)
                        .ToList();

                    if (conflictingDays.Any())
                    {
                        await transaction.RollbackAsync();
                        throw new Exception($"Error al bloquear días en el calendario, los siguientes días ya están bloqueados para otras reservas: {string.Join(", ", conflictingDays.Select(d => d.Date))}");
                    }

                    // 3. Actualizar días existentes
                    var existingDates = existingDays.Select(d => d.Date).ToList();

                    foreach (var day in existingDays)
                    {
                        if (day.BlockType == null || bookId != null)
                        {
                            day.BlockType = (byte)blockType;
                            day.BlockReason = blockReason;
                            day.BookId = bookId;
                        }
                        response.Add(day);
                    }

                    // 4. Insertar nuevos días bloqueados
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
                                BookId = bookId
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
                        // Registrar días fallidos si es necesario
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    try
                    {
                        await transaction.RollbackAsync();
                    }
                    catch
                    {
                        // Ignorar errores en rollback si la transacción ya terminó
                    }
                    throw new Exception("Error al bloquear días en el calendario del listing rent", ex);
                }
            });
        }

        public async Task<List<TlListingCalendar>> BulkSetNightPriceDaysAsync(long listingRentId, List<DateOnly> dates, decimal priceNigthly, int userId)
        {
            var listing = _context.TlListingRents.AsNoTracking().FirstOrDefault(x => x.ListingRentId == listingRentId);
            if (listing == null)
            {
                throw new ArgumentException("El listing rent especificado no existe.", nameof(listingRentId));
            }
            if (listing.OwnerUserId != userId)
            {
                throw new UnauthorizedAccessException("El usuario no tiene permiso para modificar este listing rent.");
            }
            if (priceNigthly <= 0)
            {
                throw new ArgumentException("El precio por noche debe ser mayor a cero.", nameof(priceNigthly));
            }
            var executionStrategy = _context.Database.CreateExecutionStrategy();

            return await executionStrategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                List<TlListingCalendar> response = new();
                var failedDates = new List<DateOnly>();

                try
                {
                    // 1. Buscar días existentes
                    var existingDays = await _context.TlListingCalendars
                        .Where(c => c.ListingrentId == listingRentId && dates.Contains(c.Date))
                        .ToListAsync();

                    // 2. Verificar conflictos primero
                    var conflictingDays = existingDays
                        .Where(day => day.BookId != null)
                        .ToList();

                    if (conflictingDays.Any())
                    {
                        await transaction.RollbackAsync();
                        throw new Exception($"Error al editar el precio por noche de los días seleccionados, los siguientes días están bloqueados por reservas: {string.Join(", ", conflictingDays.Select(d => d.Date))}");
                    }

                    // 3. Actualizar días existentes
                    var existingDates = existingDays.Select(d => d.Date).ToList();

                    foreach (var day in existingDays)
                    {
                        day.Price = priceNigthly;
                        response.Add(day);
                    }

                    // 4. Insertar nuevos días bloqueados
                    var newDates = dates.Except(existingDates).ToList();

                    foreach (var date in newDates)
                    {
                        try
                        {
                            var newDay = new TlListingCalendar
                            {
                                ListingrentId = listingRentId,
                                Date = date,
                                Price = priceNigthly
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
                        // Registrar días fallidos si es necesario
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    try
                    {
                        await transaction.RollbackAsync();
                    }
                    catch
                    {
                        // Ignorar errores en rollback si la transacción ya terminó
                    }
                    throw new Exception("Error al bloquear días en el calendario del listing rent", ex);
                }
            });
        }

        public async Task<List<TlListingCalendar>> BulkUnblockDaysAsync(long listingRentId, List<DateOnly> dates, int userId)
        {
            var listing = _context.TlListingRents.AsNoTracking().FirstOrDefault(x => x.ListingRentId == listingRentId);
            if (listing == null)
            {
                throw new ArgumentException("El listing rent especificado no existe.", nameof(listingRentId));
            }
            if (listing.OwnerUserId != userId)
            {
                throw new UnauthorizedAccessException("El usuario no tiene permiso para modificar este listing rent.");
            }
            var executionStrategy = _context.Database.CreateExecutionStrategy();

            return await executionStrategy.ExecuteAsync(async () =>
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
                    foreach (var day in existingDays)
                    {
                        response.Add(day);
                        if (day.BlockType == 2) // Si está bloqueado por reserva, no actualizamos
                        {
                            throw new Exception($"La fecha {day.Date} se encuentra bloqueada por una reserva confirmada.");
                        }
                        else
                        {
                            day.BlockType = null;
                            day.BlockReason = null;
                            day.BookId = null; // También limpiamos el BookId si existe
                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return response;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Error al desbloquear días en el calendario del listing rent", ex);
                }
            });
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
