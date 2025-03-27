using Assert.Domain.Models;

namespace Assert.Domain.Services
{
    public interface IListingFavoriteService
    {
        Task<ReturnModel> ToggleFavorite(int listingRentId, bool setAsFavorite, int userId, Dictionary<string, string> requestInfo);
    }
}
