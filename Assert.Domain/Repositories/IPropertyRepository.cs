using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IPropertyRepository
    {
        Task<TpProperty> GetFromListingId(long listingRentId);
        Task<TpProperty> Register(long listingRentId);
        Task SetLocation(long propertyId, double? latitude, double? longitude);
        Task<TpProperty> SetPropertySubType(long propertyId, int? subtypeId);
        Task Update(long propertyId, TpProperty tp_property);
    }
}
