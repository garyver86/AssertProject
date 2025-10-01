using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.CodeDom;

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

        public async Task SetPricing(long listingRentId, decimal? pricing, decimal? weekendPrice, int userId, int? currencyId)
        {
            if (currencyId == 0)
                currencyId = 2; //TODO : ir a base de datos para recuperar un default por codigo BOB

            var listing = _context.TlListingRents.AsNoTracking().FirstOrDefault(x => x.ListingRentId == listingRentId);
            if (listing == null)
            {
                throw new ArgumentException("El listing rent especificado no existe.", nameof(listingRentId));
            }
            if (listing.OwnerUserId != userId)
            {
                throw new UnauthorizedAccessException("El usuario no tiene permiso para modificar este listing rent.");
            }

            var price = await _context.TlListingPrices.Where(x => x.ListingRentId == listingRentId).FirstOrDefaultAsync();
            if (price != null)
            {
                if (pricing != null && pricing > 0) price.PriceNightly = pricing;
                if (weekendPrice != null && weekendPrice > 0) price.WeekendNightlyPrice = weekendPrice;
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

        public async Task SetWeekendPricing(long listingRentId, decimal? weekendPrice, int userId, int? currencyId = 2)
        {
            if (currencyId == 0)
                currencyId = 2; //TODO : ir a base de datos para recuperar un default por codigo BOB

            if (weekendPrice <= 0)
            {
                throw new ArgumentException("El precio debe ser mayor a 0", nameof(listingRentId));
            }

            var listing = _context.TlListingRents.AsNoTracking().FirstOrDefault(x => x.ListingRentId == listingRentId);
            if (listing == null)
            {
                throw new ArgumentException("El listing rent especificado no existe.", nameof(listingRentId));
            }
            if (listing.OwnerUserId != userId)
            {
                throw new UnauthorizedAccessException("El usuario no tiene permiso para modificar este listing rent.");
            }

            var price = await _context.TlListingPrices.FirstOrDefaultAsync(x => x.ListingRentId == listingRentId);
            if (price != null)
            {
                if (weekendPrice != null) price.WeekendNightlyPrice = weekendPrice;

                if (currencyId != null) price.CurrencyId = currencyId;

                await _context.SaveChangesAsync();
            }
        }
    }
}
