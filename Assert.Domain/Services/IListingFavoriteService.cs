using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.ValueObjects;

namespace Assert.Domain.Services
{
    public interface IListingFavoriteService
    {
        Task<ReturnModel> ToggleFavorite(long listingRentId, long groupId, bool setAsFavorite, int userId, Dictionary<string, string> requestInfo);
        Task<ReturnModel> ToggleHistory(long listingRentId, bool setAsFavorite, int userId, Dictionary<string, string> requestInfo);
        Task<ReturnModel<(List<TlListingRent> data, PaginationMetadata pagination)>> GetViewsHistory(int userId, int pageNumber, int pageSize, Dictionary<string, string> requestInfo);
        Task<ReturnModel<TlListingFavoriteGroup>> GetFavoriteContent(long favoriteGroupId, int userId, Dictionary<string, string> requestInfo);

        Task<ReturnModel<List<TlListingFavoriteGroup>>> GetFavoriteGroups(int userId, Dictionary<string, string> requestInfo);
        Task<ReturnModel<TlListingFavoriteGroup>> CreateFavoriteGroup(string groupName, int userId, Dictionary<string, string> requestInfo);
        Task<ReturnModel> RemoveFavoriteGroup(long groupId, int userId, Dictionary<string, string> requestInfo);
    }
}
