using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IListingStepViewRepository
    {
        Task<TlListingStepsView> Get(List<long> listingSteps, int viewTypeId);
        Task<TlListingStepsView> Get(int listngStepsViewId);
        Task<TlListingStepsView> Get(long listingRentId, string nextViewCode);
        Task SetEnded(int listngStepsViewId, bool isEnded);
    }
}
