using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingAmenitiesRepository : IListingAmenitiesRepository
    {
        private readonly InfraAssertDbContext _context;
        public ListingAmenitiesRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<List<TlListingAmenity>?> GetByListingRentId(long listingRentId)
        {
            var result = await _context.TlListingAmenities.Where(x => x.ListingRentId == listingRentId).ToListAsync();
            return result;
        }

        public async Task SetListingAmmenities(long listingRentId, List<int> amenities)
        {
            var actualAmenities = _context.TlListingAmenities.Where(x => x.ListingRentId == listingRentId).ToList();
            List<int> alreadyExist = new List<int>();
            foreach (var amenity in actualAmenities)
            {
                if (!amenities.Contains(amenity.AmenitiesTypeId))
                {
                    _context.TlListingAmenities.Remove(amenity);
                }
                else
                {
                    alreadyExist.Add(amenity.AmenitiesTypeId);
                }
            }
            foreach (var amenity in amenities)
            {
                if (!alreadyExist.Contains(amenity))
                {
                    TlListingAmenity newAmenity = new TlListingAmenity
                    {
                        AmenitiesTypeId = amenity,
                        ListingRentId = listingRentId
                    };
                    _context.TlListingAmenities.Add(newAmenity);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task SetListingAmmenities(long listingRentId, List<int> amenities, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            var actualAmenities = _context.TlListingAmenities.Where(x => x.ListingRentId == listingRentId).ToList();
            List<int> alreadyExist = new List<int>();
            foreach (var amenity in actualAmenities)
            {
                if (!amenities.Contains(amenity.AmenitiesTypeId))
                {
                    _context.TlListingAmenities.Remove(amenity);
                }
                else
                {
                    alreadyExist.Add(amenity.AmenitiesTypeId);
                }
            }
            foreach (var amenity in amenities)
            {
                if (!alreadyExist.Contains(amenity))
                {
                    TlListingAmenity newAmenity = new TlListingAmenity
                    {
                        AmenitiesTypeId = amenity,
                        ListingRentId = listingRentId
                    };
                    _context.TlListingAmenities.Add(newAmenity);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
