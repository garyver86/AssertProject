using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;

namespace Assert.Domain.Implementation
{
    public class ListingFavoriteService : IListingFavoriteService
    {
        private readonly IListingFavoriteRepository _listingFavoriteRepository;
        private readonly IImageService _imageService;
        private readonly IErrorHandler _errorHandler;

        public ListingFavoriteService(
            IListingFavoriteRepository listingFavoriteRepository,
            IImageService imageService,
            IErrorHandler errorHandler)
        {
            _listingFavoriteRepository = listingFavoriteRepository;
            _imageService = imageService;
            _errorHandler = errorHandler;
        }

        public async Task<ReturnModel> ToggleFavorite(int listingRentId, bool setAsFavorite, int userId, Dictionary<string, string> requestInfo)
        {
            ReturnModel result = new ReturnModel();
            try
            {
                await _listingFavoriteRepository.ToggleFavorite(listingRentId, setAsFavorite, userId);
                result = new ReturnModel
                {
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("L_ListingRentView.ToggleFavorite", ex, new { listingRentId }, true);
            }
            return result;
        }
    }
}
