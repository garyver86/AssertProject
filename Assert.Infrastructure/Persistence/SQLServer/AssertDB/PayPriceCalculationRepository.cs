using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class PayPriceCalculationRepository(InfraAssertDbContext _context,
        IListingLogRepository _logRepository,
        IExceptionLoggerService _exceptionLoggerService, IServiceProvider serviceProvider) : IPayPriceCalculationRepository
    {
        private readonly DbContextOptions<InfraAssertDbContext> dbOptions = serviceProvider.GetRequiredService<DbContextOptions<InfraAssertDbContext>>();

        public async Task<PayPriceCalculation> ConsultingResponse(int userId, long priceCalculationId, bool isApproval, int? reasonRefused)
        {
            var existingCalc = await _context.PayPriceCalculations.Include(x => x.ListingRent).Include(c=>c.TmConversations).Where(x => x.PriceCalculationId == priceCalculationId).FirstOrDefaultAsync();
            if (existingCalc == null)
                throw new NotFoundException($"No se encontro la cotización con ID {priceCalculationId}.");

            if (existingCalc.ListingRent.OwnerUserId != userId)
                throw new UnauthorizedAccessException($"El usuario con ID {userId} no tiene permiso para autorizar/rechazar esta consulta.");

            List<int> cancellableStatuses = new List<int> { 4 };

            if (!cancellableStatuses.Contains(existingCalc.CalculationStatusId))
                throw new InvalidOperationException($"La consulta de la cotización con ID {priceCalculationId} no puede ser aprobada/rechazada en su estado actual.");

            if (isApproval)
            {
                existingCalc.CalculationStatusId = 1;
                await _context.SaveChangesAsync();               
            }
            else
            {
                existingCalc.CalculationStatusId = 5;
                existingCalc.ReasonRefusedId = reasonRefused;
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
            long PaymentTransactionId)
        {
            var priceCalculation = await _context.PayPriceCalculations.Where(x => x.CalculationCode == calculationCode).FirstOrDefaultAsync();
            if (priceCalculation != null)
            {
                priceCalculation.CalculationStatue = "PAYED";
                priceCalculation.CalculationStatusId = 2;
                priceCalculation.PaymentProviderId = paymentProviderId;
                priceCalculation.MethodOfPaymentId = methodOfPaymentId;
                priceCalculation.PaymentTransactionId = PaymentTransactionId;
                //_context.PayPriceCalculations.Update(priceCalculation);
                await _context.SaveChangesAsync();
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
}
