using Assert.Domain.Entities;
using Assert.Domain.Models;

namespace Assert.Domain.Repositories
{
    public interface IListingStepViewRepository
    {
        Task<TlListingStepsView> Get(List<long> listingSteps, int viewTypeId);
        Task<TlListingStepsView> Get(int listngStepsViewId);
        Task<TlListingStepsView> Get(long listingRentId, string nextViewCode);
        Task<TlListingStepsView> GetLastView(long listinRentId, int ownerId);
        Task<ReturnModel> IsAllViewsEndeds(long listingRentId);
        Task SetEnded(int listngStepsViewId, bool isEnded);
        Task SetEnded(long listingRentId, int viewTypeId, bool isEnded);
    }
}
