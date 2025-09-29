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
            List<TlListingDiscountForRate> activeDiscounts = await _context.TlListingDiscountForRates.Include(x=>x.DiscountTypeForTypePrice).Where(x => x.ListingRentId == listingRentId).ToListAsync();
            return activeDiscounts;
        }

        public async Task SetDiscounts(long listingRentId, List<(int, decimal)> discountList, int userId)
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
                            DiscountCalculated = 0,
                            Porcentage = discountTypeId.Item2,
                            IsDiscount = true
                        };
                        _context.Add(newDiscount);
                        alreadyExist.Add(discountTypeId.Item1);
                    }
                    else
                    {
                        var existingDiscount = activeDiscounts.FirstOrDefault(x => x.DiscountTypeForTypePriceId == discountTypeId.Item1);
                        if (existingDiscount != null)
                        {
                            existingDiscount.Porcentage = discountTypeId.Item2;
                            _context.Update(existingDiscount);
                        }
                    }
                }
            }
            await _context.SaveChangesAsync();

        }
    }
}
