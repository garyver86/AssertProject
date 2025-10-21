using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Models;
using Assert.Domain.Models.Dashboard;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class PayPriceCalculationRepository(InfraAssertDbContext _context,
        IListingLogRepository _logRepository,
        IExceptionLoggerService _exceptionLoggerService, IServiceProvider serviceProvider,
        IBookRepository _bookRepository) : IPayPriceCalculationRepository
    {
        private readonly DbContextOptions<InfraAssertDbContext> dbOptions = serviceProvider.GetRequiredService<DbContextOptions<InfraAssertDbContext>>();

        public async Task<PayPriceCalculation> ConsultingResponse(int userId, long priceCalculationId, bool isApproval, int? reasonRefused)
        {
            var existingCalc = await _context.PayPriceCalculations.Include(x => x.ListingRent).Include(c => c.TmConversations).Where(x => x.PriceCalculationId == priceCalculationId).FirstOrDefaultAsync();
            if (existingCalc == null)
                throw new NotFoundException($"No se encontro la cotización con ID {priceCalculationId}.");

            if (existingCalc.ListingRent.OwnerUserId != userId)
                throw new UnauthorizedAccessException($"El usuario con ID {userId} no tiene permiso para autorizar/rechazar esta consulta.");

            List<int> cancellableStatuses = new List<int> { 4 };

            if (!cancellableStatuses.Contains(existingCalc.CalculationStatusId))
                throw new InvalidOperationException($"La consulta de la cotización con ID {priceCalculationId} no puede ser aprobada/rechazada en su estado actual.");

            if (isApproval)
            {
                var status = await _context.PayPriceCalculationStatuses.FirstOrDefaultAsync(x => x.PriceCalculationStatusCode == "PENDING");

                existingCalc.CalculationStatusId = status.PayPriceCalculationStatus1;
                existingCalc.ConsultAccepted = true;
                existingCalc.ConsultResponse = DateTime.UtcNow;
                existingCalc.CalculationStatue = status.PriceCalculationStatusCode;
                await _context.SaveChangesAsync();
            }
            else
            {
                var status = await _context.PayPriceCalculationStatuses.FirstOrDefaultAsync(x => x.PriceCalculationStatusCode == "REFUSED");

                existingCalc.CalculationStatusId = 5;
                existingCalc.ReasonRefusedId = reasonRefused;
                existingCalc.CalculationStatue = status.PriceCalculationStatusCode;
                existingCalc.ConsultAccepted = false;
                existingCalc.ConsultResponse = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return existingCalc;
        }

        public async Task<ReturnModel<PayPriceCalculation>> Create(PayPriceCalculation payPriceCalculation)
        {
            PayPriceCalculation priceCalculation = null;
            if (payPriceCalculation.BookId > 0)
            {
                priceCalculation = await _context.PayPriceCalculations.Where(x => x.BookId == payPriceCalculation.BookId).FirstOrDefaultAsync();
            }

            if (priceCalculation == null || priceCalculation.CalculationStatue != "PAYED")
            {
                if (priceCalculation != null)
                {
                    if (priceCalculation.ExpirationDate <= DateTime.UtcNow && priceCalculation.CalculationStatue != "EXPIRED")
                    {
                        priceCalculation.CalculationStatue = "EXPIRED";
                    }
                    else if (priceCalculation.CalculationStatue != "PAYED" && priceCalculation.CalculationStatue != "EXPIRED")
                    {
                        priceCalculation.CalculationStatue = "CANCELED";
                    }
                }
                _context.PayPriceCalculations.Add(payPriceCalculation);
                await _context.SaveChangesAsync();

                return new ReturnModel<PayPriceCalculation>
                {
                    HasError = false,
                    StatusCode = ResultStatusCode.OK,
                    Data = payPriceCalculation
                };
            }
            else
            {
                return new ReturnModel<PayPriceCalculation>
                {
                    HasError = false,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = new ErrorCommon
                    {
                        Code = "PAY_PRICE_CALCULATION_EXISTS",
                        Message = "La reserva ya cuenta con una cotización pagada."
                    }
                };
            }
        }


        public async Task<PayPriceCalculation> GetByCode(Guid calculationCode)
        {
            using (var context = new InfraAssertDbContext(dbOptions))
            {
                var priceCalculation = await context.PayPriceCalculations.Where(x => x.CalculationCode == calculationCode)
                    .Include(x => x.ListingRent).Include(x => x.ListingRent.TlCheckInOutPolicies).FirstOrDefaultAsync();
                if (priceCalculation != null)
                {
                    return priceCalculation;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<PayPriceCalculation> GetById(long id)
        {
            using (var context = new InfraAssertDbContext(dbOptions))
            {
                var priceCalculation = await context.PayPriceCalculations.Where(x => x.PriceCalculationId == id)
                    .Include(x => x.ListingRent).FirstOrDefaultAsync();
                if (priceCalculation != null)
                {
                    return priceCalculation;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<ReturnModel> SetAsPayed(Guid calculationCode, int paymentProviderId, int methodOfPaymentId,
            long PaymentTransactionId, DateTime datetimePayment)
        {
            using (var context = new InfraAssertDbContext(dbOptions))
            {
                var priceCalculation = await context.PayPriceCalculations.Where(x => x.CalculationCode == calculationCode).FirstOrDefaultAsync();
                if (priceCalculation != null)
                {
                    var status = await context.PayPriceCalculationStatuses.FirstOrDefaultAsync(x => x.PriceCalculationStatusCode == "PAYED");

                    priceCalculation.CalculationStatue = status.PriceCalculationStatusCode;
                    priceCalculation.CalculationStatusId = status.PayPriceCalculationStatus1;
                    priceCalculation.PaymentProviderId = paymentProviderId;
                    priceCalculation.MethodOfPaymentId = methodOfPaymentId;
                    priceCalculation.PaymentTransactionId = PaymentTransactionId;
                    priceCalculation.DatetimePayment = datetimePayment;
                    //_context.PayPriceCalculations.Update(priceCalculation);
                    await context.SaveChangesAsync();
                    return new ReturnModel
                    {
                        HasError = false,
                        StatusCode = ResultStatusCode.OK
                    };
                }
                else
                {
                    return new ReturnModel<PayPriceCalculation>
                    {
                        HasError = false,
                        StatusCode = ResultStatusCode.BadRequest,
                        ResultError = new ErrorCommon
                        {
                            Code = "PAY_PRICE_CALCULATION_EXISTS",
                            Message = "la cotización no puede ser encontrada."
                        }
                    };
                }
            }
        }

        public async Task SetBookId(long priceCalculationId, long bookId)
        {
            using (var context = new InfraAssertDbContext(dbOptions))
            {
                var priceCalculation = await context.PayPriceCalculations.FirstOrDefaultAsync(x => x.PriceCalculationId == priceCalculationId);
                if (priceCalculation != null)
                {
                    priceCalculation.BookId = bookId;
                    await context.SaveChangesAsync();
                }
            }
        }
        public async Task<RevenueSummary> GetRevenueReportAsync(RevenueReportRequest request)
        {
            var query = _context.PayPriceCalculations.AsQueryable();

            // Aplicar filtros
            query = ApplyFilters(query, request);

            // Filtrar solo cálculos completados/pagados (ajusta según tus status)
            query = query.Where(x => x.DatetimePayment != null); // Asumiendo que 3 = Completado

            var monthlyData = await query
                .GroupBy(x => new { ((DateTime)x.DatetimePayment).Year, ((DateTime)x.DatetimePayment).Month })
                .Select(g => new RevenueReport
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalRevenue = g.Sum(x => x.Amount),
                    TotalPlatformFees = g.Sum(x => x.PlatformFee ?? 0),
                    NetRevenue = g.Sum(x => x.Amount) - g.Sum(x => x.PlatformFee ?? 0),
                    TransactionCount = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            return CalculateSummary(monthlyData);
        }

        public async Task<BusinessReport> GetBusinessReportAsync(RevenueReportRequest request)
        {
            var report = new BusinessReport
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            // Consulta base filtrada
            var baseQuery = _context.PayPriceCalculations.AsQueryable();


            // Aplicar filtros
            baseQuery = ApplyFilters(baseQuery, request);


            // 1. INGRESOS CONFIRMADOS (Ya pagados)
            var confirmedRevenue = await baseQuery.Where(x => x.DatetimePayment != null)
                .SumAsync(x => x.Amount);

            report.ConfirmedRevenue = confirmedRevenue;

            // 2. FUTUROS INGRESOS (Reservas confirmadas)
            var futureRevenue = await baseQuery.Where(x => x.DatetimePayment != null && x.PayPayoutTransactions == null)
                .SumAsync(x => x.Amount);

            report.FutureRevenue = futureRevenue;

            // 3. INGRESO ASSERT (Comisiones de plataforma)
            var assertIncome = await baseQuery.Where(x => x.DatetimePayment != null)
                .SumAsync(x => x.PlatformFee);

            report.AssertFee = assertIncome ?? 0;

            // 4. RESERVAS CONFIRMADAS Y CANCELADAS
            var bookingsData = await baseQuery.Where(x => x.DatetimePayment != null)
                .GroupBy(x => x.PriceCalculationId)
                .Select(g => new
                {
                    StatusId = g.Key,
                    Count = g.Count(),
                    Book = g.Select(x => x.Book).FirstOrDefault()
                })
                .ToListAsync();

            // Asumiendo estos status IDs (ajusta según tu sistema):
            // 2 = Confirmado, 3 = Pagado, 4 = Cancelado, 5 = Rechazado
            report.ConfirmedBookings = bookingsData
                .Where(x => x.Book.BookStatusId == 2 || x.Book.BookStatusId == 3 || x.Book.BookStatusId == 6)
                .Sum(x => x.Count);

            report.CancelledBookings = bookingsData
                .Where(x => x.Book.BookStatusId == 4 || x.Book.BookStatusId == 5 || x.Book.BookStatusId == 7)
                .Sum(x => x.Count);

            // 5. FACTOR DE OCUPACIÓN
            var occupancyData = await CalculateOccupancyAsync(request);
            report.OccupiedNights = occupancyData.OccupiedNights;
            report.OccupancyRate = occupancyData.OccupancyRate;

            return report;
        }

        private async Task<(int OccupiedNights, decimal OccupancyRate)> CalculateOccupancyAsync(RevenueReportRequest request)
        {
            // Consulta base filtrada
            var baseQuery = _context.PayPriceCalculations.AsQueryable();


            // Aplicar filtros
            baseQuery = ApplyFilters(baseQuery, request);

            // Obtener reservas confirmadas/pagadas que se superponen con el periodo
            var occupiedBookings = await baseQuery
                .Where(x => (x.Book != null && (x.Book.BookStatusId == 2 || x.Book.BookStatusId == 3 || x.Book.BookStatusId == 6))) // Superposición con el periodo
                .Select(x => new
                {
                    ActualStart = x.InitBook < request.StartDate ? request.StartDate : x.InitBook,
                    ActualEnd = x.EndBook > request.EndDate ? request.EndDate : x.EndBook
                })
                .ToListAsync();

            // Calcular total de noches ocupadas
            var occupiedNights = occupiedBookings
                .Sum(booking => (int)((TimeSpan)(booking.ActualEnd - booking.ActualStart)).TotalDays + 1);

            // Calcular factor de ocupación: (noches ocupadas * 100 / 30 días)
            var occupancyRate = (decimal)occupiedNights * 100 / 30;

            return (occupiedNights, Math.Round(occupancyRate, 2));
        }

        private IQueryable<PayPriceCalculation> ApplyFilters(
       IQueryable<PayPriceCalculation> query,
       RevenueReportRequest request)
        {
            if (request.Year.HasValue)
            {
                query = query.Where(x => x.DatetimePayment != null && ((DateTime)x.DatetimePayment).Year == request.Year.Value);
            }

            if (request.StartDate.HasValue)
            {
                query = query.Where(x => x.DatetimePayment != null && x.DatetimePayment >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(x => x.DatetimePayment != null && x.DatetimePayment <= request.EndDate.Value);
            }

            if (request.UserId > 0)
            {
                query = query.Where(x => x.ListingRent.OwnerUserId == request.UserId);
            }

            return query;
        }

        private RevenueSummary CalculateSummary(List<RevenueReport> monthlyData)
        {
            return new RevenueSummary
            {
                MonthlyData = monthlyData,
                GrandTotalRevenue = monthlyData.Sum(x => x.TotalRevenue),
                GrandTotalPlatformFees = monthlyData.Sum(x => x.TotalPlatformFees),
                GrandTotalNetRevenue = monthlyData.Sum(x => x.NetRevenue),
                GrandTotalTransactions = monthlyData.Sum(x => x.TransactionCount)
            };
        }
    }
}
