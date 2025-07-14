using Assert.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assert.Domain.Repositories
{
    public interface IListingFavoriteRepository
    {
        Task ToggleFavorite(long listingRentId, long groupId, bool setAsFavorite, int userId);

        Task<List<TlListingFavoriteGroup>> GetFavoriteGroups(int userId);
        Task<TlListingFavoriteGroup?> GetFavoriteGroupById(long groupId, int userId);
        Task<TlListingFavoriteGroup> CreateFavoriteGroup(string groupName, int userId);
        Task RemoveFavoriteGroup(long groupId, int userId);
        Task<List<long>> GetAllFavoritesList(long userId);
    }
}
