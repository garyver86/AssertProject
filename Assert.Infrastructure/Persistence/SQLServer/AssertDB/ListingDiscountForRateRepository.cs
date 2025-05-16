using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Assert.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingDiscountForRateRepository(
        InfraAssertDbContext _context,
        IExceptionLoggerService _exceptionLoggerService) 
        : IListingDiscountForRateRepository
    {
        
        public async Task<List<TlListingDiscountForRate>?> GetByListingRentId(long listingRentId)
        {
            var result = await _context.TlListingDiscountForRates.Where(x => x.ListingRentId == listingRentId).ToListAsync();
            return result;
        }

        public async Task<string> SetDiscounts(long listingRentId, List<(int, decimal)> discountList)
        {
            try 
            {
                var listingDiscounts = await _context.TlListingDiscountForRates
                    .Where(d => d.ListingRentId == listingRentId).ToListAsync();
                _context.TlListingDiscountForRates.RemoveRange(listingDiscounts);

                foreach (var discount in discountList)
                {
                    _context.TlListingDiscountForRates.Add(new TlListingDiscountForRate { 
                        ListingRentId = listingRentId,
                        DiscountCalculated = discount.Item2,
                        Porcentage = 0,
                        DiscountTypeForTypePriceId = discount.Item1,
                        IsDiscount = true
                    });
                }
                await _context.SaveChangesAsync();
                return "UPDATED";
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { listingRentId, discountList });

                throw new InfrastructureException(ex.Message);
            }
        }
    }
}
