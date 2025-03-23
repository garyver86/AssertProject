using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IAccommodationTypeRepository
    {
        Task<TlAccommodationType> GetActive(int? accomodationId);
    }
}
