using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingSecurityItemsRepository : IListingSecurityItemsRepository
    {
        private readonly InfraAssertDbContext _context;
        public ListingSecurityItemsRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<List<TlListingSecurityItem>?> GetByListingRentId(long listingRentId)
        {
            var result = await _context.TlListingSecurityItems.Where(x => x.ListingRentId == listingRentId).ToListAsync();
            return result;
        }

        public async Task SetListingSecurityItems(long listingRentId, List<int> securityItems)
        {
            var actualAmenities = _context.TlListingSecurityItems.Where(x => x.ListingRentId == listingRentId).ToList();
            List<int> alreadyExist = new List<int>();
            foreach (var amenity in actualAmenities)
            {
                if (!securityItems.Contains(amenity.SecurityItemTypeId??0))
                {
                    _context.TlListingSecurityItems.Remove(amenity);
                }
                else
                {
                    alreadyExist.Add(amenity.SecurityItemTypeId ?? 0);
                }
            }
            foreach (var amenity in securityItems)
            {
                if (!alreadyExist.Contains(amenity))
                {
                    TlListingSecurityItem newAmenity = new TlListingSecurityItem
                    {
                        SecurityItemTypeId = amenity,
                        ListingRentId = listingRentId
                    };
                    _context.TlListingSecurityItems.Add(newAmenity);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
