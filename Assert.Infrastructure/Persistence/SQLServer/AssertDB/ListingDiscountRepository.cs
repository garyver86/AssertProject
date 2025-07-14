using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingDiscountRepository : IListingDiscountRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly IListingLogRepository _logRepository;
        private readonly IListingPricingRepository _priceRepository;
        public ListingDiscountRepository(InfraAssertDbContext infraAssertDbContext, IListingLogRepository logRepository,
            IListingStatusRepository listingStatusRepository, IListingPricingRepository listingPriceRepository)
        {
            _context = infraAssertDbContext;
            _logRepository = logRepository;
            _priceRepository = listingPriceRepository;
        }

        public async Task<List<TlListingDiscountForRate>?> Get(long listingRentId)
        {
            List<TlListingDiscountForRate> activeDiscounts = await _context.TlListingDiscountForRates.Where(x => x.ListingRentId == listingRentId).ToListAsync();
            return activeDiscounts;
        }

        public async Task SetDiscounts(long listingRentId, List<(int, decimal)> discountList)
        {
            List<TlListingDiscountForRate> activeDiscounts = await _context.TlListingDiscountForRates.Where(x => x.ListingRentId == listingRentId).ToListAsync();
            List<int> alreadyExist = new List<int>();
            foreach (var discount in activeDiscounts)
            {
                if (!(discountList?.Select(x => x.Item1).Contains(discount.DiscountTypeForTypePriceId) ?? false))
                {
                    _context.TlListingDiscountForRates.Remove(discount);
                }
                else
                {
                    alreadyExist.Add(discount.DiscountTypeForTypePriceId);
                }
            }
            if (discountList.Count() > 0)
            {
                foreach (var discountTypeId in discountList)
                {
                    if (!alreadyExist.Contains(discountTypeId.Item1))
                    {
                        TlListingDiscountForRate newDiscount = new TlListingDiscountForRate
                        {
                            DiscountTypeForTypePriceId = discountTypeId.Item1,
                            ListingRentId = listingRentId,
                            DiscountCalculated = discountTypeId.Item2,
                            Porcentage = 0,
                            IsDiscount = true
                        };
                        _context.Add(newDiscount);
                        alreadyExist.Add(discountTypeId.Item1);
                    }
                }
            }
            await _context.SaveChangesAsync();

        }
    }
}
