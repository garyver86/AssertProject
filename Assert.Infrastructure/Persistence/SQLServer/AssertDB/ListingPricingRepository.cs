using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingPricingRepository(InfraAssertDbContext _context,
        IExceptionLoggerService _exceptionLoggerService) 
        : IListingPricingRepository
    {
        public async Task<TlListingPrice> GetByListingRent(long listingRentId)
        {
            var price = await _context.TlListingPrices.Where(x => x.ListingRentId == listingRentId).FirstOrDefaultAsync();
            return price;
        }

        public Task SetPricing(long listingRentId, decimal? pricing, int? currencyId)
        {
            throw new NotImplementedException();
        }

        public async Task SetPricing(long listingRentId, decimal? pricing, decimal? weekendPrice, int? currencyId)
        {
            var price = await _context.TlListingPrices.Where(x => x.ListingRentId == listingRentId).FirstOrDefaultAsync();
            if (price != null)
            {
                if(pricing != null) price.PriceNightly = pricing;
                if (weekendPrice != null) price.WeekendNightlyPrice = weekendPrice;
                if (currencyId != null) price.CurrencyId = currencyId;

                await _context.SaveChangesAsync();
            }
            else
            {
                price = new TlListingPrice
                {
                    PriceNightly = pricing,
                    WeekendNightlyPrice = weekendPrice,
                    CurrencyId = currencyId,
                    ListingRentId = listingRentId
                };

                await _context.TlListingPrices.AddAsync(price);
                await _context.SaveChangesAsync();
            }
        }
    }
}
