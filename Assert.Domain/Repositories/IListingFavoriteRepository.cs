using Assert.Domain.Models;

namespace Assert.Domain.Repositories
{
    public interface IListingFavoriteRepository
    {
        Task ToggleFavorite(int listingRentId, bool setAsFavorite, int userId);
    }
}
