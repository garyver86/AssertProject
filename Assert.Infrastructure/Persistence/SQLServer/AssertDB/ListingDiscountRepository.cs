using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingDiscountRepository : IListingDiscountRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly IListingLogRepository _logRepository;
        private readonly IListingPriceRepository _priceRepository;
        public ListingDiscountRepository(InfraAssertDbContext infraAssertDbContext, IListingLogRepository logRepository,
            IListingStatusRepository listingStatusRepository, IListingPriceRepository listingPriceRepository)
        {
            _context = infraAssertDbContext;
            _logRepository = logRepository;
            _priceRepository = listingPriceRepository;
        }

        public async Task SetDiscounts(long listingRentId, IEnumerable<int>? enumerable)
        {
            var activeDiscounts = await _context.TlListingDiscountForRates.Where(x => x.ListingPrice.ListingRentId == listingRentId).ToListAsync();
            List<int> alreadyExist = new List<int>();
            foreach (var discount in activeDiscounts)
            {
                if (!(enumerable?.Contains(discount.DiscountTypeForTypePriceId) ?? false))
                {
                    _context.TlListingDiscountForRates.Remove(discount);
                }
                else
                {
                    alreadyExist.Add(discount.DiscountTypeForTypePriceId);
                }
            }
            if (enumerable.Count() > 0)
            {
                foreach (int discountTypeId in enumerable)
                {
                    if (!alreadyExist.Contains(discountTypeId))
                    {
                        TlListingDiscountForRate newDiscount = new TlListingDiscountForRate
                        {
                            DiscountTypeForTypePriceId = discountTypeId,
                            ListingPriceId = listingRentId
                        };
                        _context.Add(newDiscount);
                    }
                }
            }
            await _context.SaveChangesAsync();

        }
    }
}
