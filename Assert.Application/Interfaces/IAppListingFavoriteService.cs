using Assert.Application.DTOs.Responses;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.ValueObjects;

namespace Assert.Application.Interfaces
{
    public interface IAppListingFavoriteService
    {
        Task<ReturnModelDTO> ToggleFavorite(long listingRentId, long groupId, bool setAsFavorite, int userId, Dictionary<string, string> requestInfo);

        Task<ReturnModelDTO> ToggleHistory(long listingRentId, bool setAsFavorite, int userId, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO<(List<ListingRentDTO> data, PaginationMetadataDTO pagination)>> GetViewsHistory(int userId, int pageNumber, int pageSize, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO<ListingFavoriteGroupDTO>> GetFavoriteContent(long favoriteGroupId, int userId, Dictionary<string, string> requestInfo);

        Task<ReturnModelDTO<List<ListingFavoriteGroupDTO>>> GetFavoriteGroups(int userId, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO<ListingFavoriteGroupDTO>> CreateFavoriteGroup(string groupName, int userId, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO> RemoveFavoriteGroup(long groupId, int userId, Dictionary<string, string> requestInfo);
    }
}
