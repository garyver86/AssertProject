using Assert.Domain.Entities;
using Assert.Domain.ValueObjects;

namespace Assert.Domain.Repositories
{
    public interface IListingViewHistoryRepository
    {
        Task<(List<TlListingRent>, PaginationMetadata)> GetViewsHistory(int userId, int pageNumber = 1, int pageSize = 10, int? countryId = null);
        Task ToggleFromHistory(long listingRentId, bool setAsFavorite, int userId);
    }
}
