using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingPricingRepository : IListingPricingRepository
    {
        private readonly InfraAssertDbContext _context;

        public ListingPricingRepository(InfraAssertDbContext context)
        {
            _context = context;
        }

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
                price.PriceNightly = pricing;
                price.WeekendNightlyPrice = weekendPrice;
                price.CurrencyId = currencyId;
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
