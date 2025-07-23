using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Domain.ValueObjects;
using Assert.Infrastructure.Persistence.SQLServer.AssertDB;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Assert.Application.Services
{
    public class AppListingFavoriteService : IAppListingFavoriteService
    {
        private readonly IListingFavoriteService _listingFavoriteService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IErrorHandler _errorHandler;
        private readonly ISystemConfigurationRepository _systemConfigurationRepository;
        private readonly IHttpContextAccessor requestContext;

        public AppListingFavoriteService(
            IListingFavoriteService listingFavoriteService,
            IImageService imageService,
            IMapper mapper,
            IErrorHandler errorHandler, ISystemConfigurationRepository systemConfigurationRepository,
            IHttpContextAccessor contextAccessor)
        {
            _listingFavoriteService = listingFavoriteService;
            _imageService = imageService;
            _mapper = mapper;
            _errorHandler = errorHandler;
            _systemConfigurationRepository = systemConfigurationRepository;
            requestContext = contextAccessor;
        }

        public async Task<ReturnModelDTO<ListingFavoriteGroupDTO>> CreateFavoriteGroup(string groupName, int userId, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO<ListingFavoriteGroupDTO> result = new ReturnModelDTO<ListingFavoriteGroupDTO>();
            try
            {
                ReturnModel<TlListingFavoriteGroup> listings = await _listingFavoriteService.CreateFavoriteGroup(groupName, userId, requestInfo);

                result = new ReturnModelDTO<ListingFavoriteGroupDTO>
                {
                    HasError = listings.HasError,
                    StatusCode = listings.StatusCode,
                    ResultError = _mapper.Map<ErrorCommonDTO>(listings.ResultError),
                    Data = _mapper.Map<ListingFavoriteGroupDTO>(listings.Data)
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("L_ListingRentView.CreateFavoriteGroup", ex, new { groupName, userId }, true));
            }
            return result;
        }

        public async Task<ReturnModelDTO<ListingFavoriteGroupDTO>> GetFavoriteContent(long favoriteGroupId, int userId, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO<ListingFavoriteGroupDTO> result = new ReturnModelDTO<ListingFavoriteGroupDTO>();
            try
            {
                ReturnModel<TlListingFavoriteGroup> listings = await _listingFavoriteService.GetFavoriteContent(favoriteGroupId, userId, requestInfo);

                if (listings.Data.TlListingFavorites?.Count > 0)
                {
                    string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                    _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");
                    foreach (var list in listings.Data.TlListingFavorites)
                    {
                        if (list.ListingRent?.TlListingPhotos?.Count > 0)
                        {
                            foreach (var item in list.ListingRent.TlListingPhotos)
                            {
                                item.PhotoLink = $"{requestContext.HttpContext?.Request.Scheme}://{requestContext.HttpContext?.Request.Host}/{_basePath}/{item.Name}";
                            }
                        }
                    }
                }

                result = new ReturnModelDTO<ListingFavoriteGroupDTO>
                {
                    HasError = listings.HasError,
                    StatusCode = listings.StatusCode,
                    ResultError = _mapper.Map<ErrorCommonDTO>(listings.ResultError),
                    Data = _mapper.Map<ListingFavoriteGroupDTO>(listings.Data)
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("L_ListingRentView.GetFavoriteContent", ex, new { favoriteGroupId, userId }, true));
            }
            return result;
        }

        public async Task<ReturnModelDTO<List<ListingFavoriteGroupDTO>>> GetFavoriteGroups(int userId, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO<List<ListingFavoriteGroupDTO>> result = new ReturnModelDTO<List<ListingFavoriteGroupDTO>>();
            try
            {
                ReturnModel<List<TlListingFavoriteGroup>> listings = await _listingFavoriteService.GetFavoriteGroups(userId, requestInfo);

                result = new ReturnModelDTO<List<ListingFavoriteGroupDTO>>
                {
                    HasError = listings.HasError,
                    StatusCode = listings.StatusCode,
                    ResultError = _mapper.Map<ErrorCommonDTO>(listings.ResultError),
                    Data = _mapper.Map<List<ListingFavoriteGroupDTO>>(listings.Data)
                };

                if (result.Data?.Count > 0)
                {
                    foreach (var group in result.Data)
                    {
                        group.totalRegisters = group.UserId;
                        group.UserId = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("L_ListingRentView.GetFavoriteGroups", ex, new { userId }, true));
            }
            return result;
        }

        public async Task<ReturnModelDTO<(List<ListingRentDTO> data, PaginationMetadataDTO pagination)>> GetViewsHistory(int userId, int pageNumber, int pageSize, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO<(List<ListingRentDTO> data, PaginationMetadataDTO pagination)> result = new ReturnModelDTO<(List<ListingRentDTO> data, PaginationMetadataDTO pagination)>();
            try
            {
                ReturnModel<(List<TlListingRent> data, PaginationMetadata pagination)> listings = await _listingFavoriteService.GetViewsHistory(userId, pageNumber, pageSize, requestInfo);

                if (listings.Data.data?.Count > 0)
                {
                    string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                    _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");
                    foreach (var list in listings.Data.data)
                    {
                        if (list?.TlListingPhotos?.Count > 0)
                        {
                            foreach (var item in list.TlListingPhotos)
                            {
                                item.PhotoLink = $"{requestContext.HttpContext?.Request.Scheme}://{requestContext.HttpContext?.Request.Host}/{_basePath}/{item.Name}";
                            }
                        }
                    }
                }

                result = new ReturnModelDTO<(List<ListingRentDTO> data, PaginationMetadataDTO pagination)>
                {
                    HasError = listings.HasError,
                    StatusCode = listings.StatusCode,
                    ResultError = _mapper.Map<ErrorCommonDTO>(listings.ResultError),
                    Data = (_mapper.Map<List<ListingRentDTO>>(listings.Data.data), _mapper.Map<PaginationMetadataDTO>(listings.Data.pagination))
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("L_ListingRentView.GetViewsHistory", ex, new { userId, pageNumber, pageSize }, true));
            }
            return result;
        }

        public async Task<ReturnModelDTO> RemoveFavoriteGroup(long groupId, int userId, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO result = new ReturnModelDTO();
            try
            {
                ReturnModel listings = await _listingFavoriteService.RemoveFavoriteGroup(groupId, userId, requestInfo);
                result = _mapper.Map<ReturnModelDTO>(listings);
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("L_ListingRentView.RemoveFavoriteGroup", ex, new { groupId, userId }, true));
            }
            return result;
        }

        public async Task<ReturnModelDTO> ToggleFavorite(long listingRentId, long? groupId, bool setAsFavorite, int userId, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO result = new ReturnModelDTO();
            try
            {
                ReturnModel listings = await _listingFavoriteService.ToggleFavorite(listingRentId, groupId, setAsFavorite, userId, requestInfo);
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

        public async Task<ReturnModelDTO> ToggleHistory(long listingRentId, bool setAsFavorite, int userId, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO result = new ReturnModelDTO();
            try
            {
                ReturnModel listings = await _listingFavoriteService.ToggleHistory(listingRentId, setAsFavorite, userId, requestInfo);
                result = _mapper.Map<ReturnModelDTO>(listings);
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("L_ListingRentView.ToggleHistory", ex, new { listingRentId }, true));
            }
            return result;
        }
    }
}
