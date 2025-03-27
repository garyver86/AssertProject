using Assert.Application.DTOs;
using Assert.Application.Interfaces;
using Assert.Domain.Models;
using Assert.Domain.Services;
using AutoMapper;

namespace Assert.Application.Services
{
    public class AppListingFavoriteService : IAppListingFavoriteService
    {
        private readonly IListingFavoriteService _listingFavoriteService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IErrorHandler _errorHandler;

        public AppListingFavoriteService(
            IListingFavoriteService listingFavoriteService,
            IImageService imageService,
            IMapper mapper,
            IErrorHandler errorHandler)
        {
            _listingFavoriteService = listingFavoriteService;
            _imageService = imageService;
            _mapper = mapper;
            _errorHandler = errorHandler;
        }

        public async Task<ReturnModelDTO> ToggleFavorite(int listingRentId, bool setAsFavorite, int userId, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO result = new ReturnModelDTO();
            try
            {
                ReturnModel listings = await _listingFavoriteService.ToggleFavorite(listingRentId, setAsFavorite, userId, requestInfo);
                result = _mapper.Map<ReturnModelDTO>(listings);
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("L_ListingRentView.ToggleFavorite", ex, new { listingRentId }, true));
            }
            return result;
        }
    }
}
