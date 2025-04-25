using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IPropertyAddressRepository
    {
        Task<TpPropertyAddress> Set(TpPropertyAddress addresInput, long propertyId);
    }
}
