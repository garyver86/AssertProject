using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IListingAmenitiesRepository
    {
        Task<List<TlListingAmenity>?> GetByListingRentId(long listingRentId);
        Task SetListingAmmenities(long listingRentId, List<int> amenities, Dictionary<string, string> clientData, bool useTechnicalMessages);
    }
}
