using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Domain.ValueObjects;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Assert.Domain.Implementation
{
    public class ListingFavoriteService : IListingFavoriteService
    {
        private readonly IListingFavoriteRepository _listingFavoriteRepository;
        private readonly IListingViewHistoryRepository _listingViewHistoryRepository;
        private readonly IImageService _imageService;
        private readonly IErrorHandler _errorHandler;

        public ListingFavoriteService(
            IListingFavoriteRepository listingFavoriteRepository,
            IImageService imageService,
            IErrorHandler errorHandler,
            IListingViewHistoryRepository listingViewHistoryRepository)
        {
            _listingFavoriteRepository = listingFavoriteRepository;
            _imageService = imageService;
            _errorHandler = errorHandler;
            _listingViewHistoryRepository = listingViewHistoryRepository;
        }

        public async Task<ReturnModel<TlListingFavoriteGroup>> CreateFavoriteGroup(string groupName, int userId, Dictionary<string, string> requestInfo)
        {
            ReturnModel<TlListingFavoriteGroup> result = new ReturnModel<TlListingFavoriteGroup>();
            try
            {
                var data = await _listingFavoriteRepository.CreateFavoriteGroup(groupName, userId);
                result = new ReturnModel<TlListingFavoriteGroup>
                {
                    HasError = false,
                    StatusCode = ResultStatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("ListingFavoriteService.CreateFavoriteGroup", ex, new { groupName, userId }, true);
            }
            return result;
        }

        public async Task<ReturnModel<TlListingFavoriteGroup>> GetFavoriteContent(long favoriteGroupId, int userId, Dictionary<string, string> requestInfo)
        {
            ReturnModel<TlListingFavoriteGroup> result = new ReturnModel<TlListingFavoriteGroup>();
            try
            {
                var data = await _listingFavoriteRepository.GetFavoriteGroupById(favoriteGroupId, userId);
                result = new ReturnModel<TlListingFavoriteGroup>
                {
                    HasError = false,
                    StatusCode = ResultStatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("ListingFavoriteService.GetFavoriteContent", ex, new { favoriteGroupId, userId }, true);
            }
            return result;
        }

        public async Task<ReturnModel<List<TlListingFavoriteGroup>>> GetFavoriteGroups(int userId, Dictionary<string, string> requestInfo)
        {
            ReturnModel<List<TlListingFavoriteGroup>> result = new ReturnModel<List<TlListingFavoriteGroup>>();
            try
            {
                var data = await _listingFavoriteRepository.GetFavoriteGroups(userId);

                var history = await _listingViewHistoryRepository.GetViewsHistory(userId, 1, 1);

                List<TlListingFavoriteGroup> resultList = new List<TlListingFavoriteGroup>();

                if (history.Item1 != null && history.Item1.Count > 0)
                {
                    // Create a group for views history
                    TlListingFavoriteGroup historyGroup = new TlListingFavoriteGroup
                    {
                        FavoriteGroupListingId = -1, // Special ID for history group
                        FavoriteGroupName = "Historial de Vistas",
                        TlListingFavorites = history.Item1.Select(x => new TlListingFavorite
                        {
                            ListingRentId = x.ListingRentId,
                            UserId = userId,
                            ListingRent = x
                        }).ToList(),
                        UserId = history.Item2.TotalItemCount
                    };
                    resultList.Add(historyGroup);
                }

                if (data.Count > 0)
                {
                    resultList.AddRange(data);
                }

                result = new ReturnModel<List<TlListingFavoriteGroup>>
                {
                    HasError = false,
                    StatusCode = ResultStatusCode.OK,
                    Data = resultList
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("ListingFavoriteService.GetFavoriteGroups", ex, new { userId }, true);
            }
            return result;
        }

        public async Task<ReturnModel<(List<TlListingRent> data, PaginationMetadata pagination)>> GetViewsHistory(int userId, int pageNumber, int pageSize, Dictionary<string, string> requestInfo)
        {
            ReturnModel<(List<TlListingRent> data, PaginationMetadata pagination)> result = new ReturnModel<(List<TlListingRent> data, PaginationMetadata pagination)>();
            try
            {
                (List<TlListingRent> data, PaginationMetadata pagination) = await _listingViewHistoryRepository.GetViewsHistory(userId, pageNumber, pageSize);
                result = new ReturnModel<(List<TlListingRent> data, PaginationMetadata pagination)>
                {
                    HasError = false,
                    StatusCode = ResultStatusCode.OK,
                    Data = (data, pagination)
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("ListingFavoriteService.GetViewsHistory", ex, new { userId }, true);
            }
            return result;
        }

        public async Task<ReturnModel> RemoveFavoriteGroup(long groupId, int userId, Dictionary<string, string> requestInfo)
        {
            ReturnModel result = new ReturnModel();
            try
            {
                await _listingFavoriteRepository.RemoveFavoriteGroup(groupId, userId);
                result = new ReturnModel<List<TlListingFavoriteGroup>>
                {
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("ListingFavoriteService.RemoveFavoriteGroup", ex, new { groupId, userId }, true);
            }
            return result;
        }

        public async Task<ReturnModel> ToggleFavorite(long listingRentId, long? groupId, bool setAsFavorite, int userId, Dictionary<string, string> requestInfo)
        {
            ReturnModel result = new ReturnModel();
            try
            {
                await _listingFavoriteRepository.ToggleFavorite(listingRentId, groupId, setAsFavorite, userId);
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
                result.ResultError = _errorHandler.GetErrorException("ListingFavoriteService.ToggleFavorite", ex, new { listingRentId }, true);
            }
            return result;
        }

        public async Task<ReturnModel> ToggleHistory(long listingRentId, bool remove, int userId, Dictionary<string, string> requestInfo)
        {
            ReturnModel result = new ReturnModel();
            try
            {
                await _listingViewHistoryRepository.ToggleFromHistory(listingRentId, !remove, userId);
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
                result.ResultError = _errorHandler.GetErrorException("ListingFavoriteService.ToggleHistory", ex, new { listingRentId }, true);
            }
            return result;
        }
    }
}
