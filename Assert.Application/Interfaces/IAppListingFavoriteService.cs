using Assert.Application.DTOs;

namespace Assert.Application.Interfaces
{
    public interface IAppListingFavoriteService
    {
        Task<ReturnModelDTO> ToggleFavorite(int listingRentId, bool setAsFavorite, int userId, Dictionary<string, string> requestInfo);
    }
}
