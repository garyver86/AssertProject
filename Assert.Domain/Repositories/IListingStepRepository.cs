using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IListingStepRepository
    {
        Task<List<TlListingStep>> GetAll(long listingRentId);
        Task<bool> HasViewNotEnded(long listingStepsId);
    }
}
