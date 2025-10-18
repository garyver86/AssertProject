using Assert.Application.DTOs;
using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Models;
using Assert.Domain.Models.Host;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Domain.Utils;
using Assert.Domain.ValueObjects;
using Assert.Infrastructure.Persistence.SQLServer.AssertDB;
using Assert.Shared.Extensions;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.CodeDom;

namespace Assert.Application.Services
{
    internal class AppListingRentService(IListingRentService _listingRentService,
        IListingRentRepository _listingRentRepository,
        IListingPhotoRepository _listingPhotoRepository,
        IListingRentReviewRepository _listingReviewRepository,
        IListingPricingRepository _listingPriceRepository,
        IListingDiscountForRateRepository _listingDiscountForRateRepository,
        IImageService _imageService,
        IMapper _mapper,
        IErrorHandler _errorHandler,
        IExceptionLoggerService _exceptionLoggerService,
        ISystemConfigurationRepository _systemConfigurationRepository,
        IPropertyRepository _propertyRepository,
        IPropertyAddressRepository _propertyAddressRepository,
        IListingAmenitiesRepository _listingAmenitiesRepository,
        IListingFeaturedAspectRepository _listingFeaturedAspectRepository,
        IListingSecurityItemsRepository _listingSecurityItemsRepository,
        IListingDiscountRepository _listingDiscountRepository,
        IListingRentRulesRepository _listingRentRulesRepository,
        IListingStatusRepository _listingStatusRepository,
        ILocationService _locationService,
        IHttpContextAccessor requestContext)
        : IAppListingRentService
    {
        private bool UseTechnicalMessages { get; set; } = false;

        public async Task<ReturnModelDTO> ChangeStatus(long listingRentId, int ownerUserId, string newStatusCode, Dictionary<string, string> clientData,
            bool useTechnicalMessages = true)
        {
            ReturnModelDTO result = new ReturnModelDTO();
            try
            {
                var changeResult = await _listingRentService.ChangeStatus(listingRentId, ownerUserId, newStatusCode, clientData, useTechnicalMessages);

                if (changeResult.StatusCode == ResultStatusCode.OK)
                {
                    result = _mapper.Map<ReturnModelDTO>(changeResult); // Mapeo de la entidad del dominio al DTO
                }
                else
                {
                    result = _mapper.Map<ReturnModelDTO>(changeResult); // Mapeo de errores
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.ChangeStatus", ex, new { listingRentId, ownerUserId, newStatusCode, clientData }, UseTechnicalMessages));
            }
            return result;
        }

        public async Task<ReturnModelDTO> ChangeStatusByAdmin(long listingRentId, int ownerUserId,
            string newStatusCode, Dictionary<string, string> clientData)
        {
            ReturnModelDTO result = new ReturnModelDTO();
            try
            {
                var status = await _listingStatusRepository.Get(newStatusCode);

                var changeResult = await _listingRentRepository.ChangeStatus(listingRentId, ownerUserId,
                    status.ListingStatusId, clientData);

                result.Data = "UPDATED";
                result.StatusCode = ResultStatusCode.OK;
                result.HasError = false;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            return result;
        }

        public async Task<ReturnModelDTO> ChangeListingRentStatusByOwnerId(
            int ownerId, string statusCode, Dictionary<string, string> userInfo)
        {
            ReturnModelDTO result = new ReturnModelDTO();
            try
            {
                var response = await _listingRentRepository.ChangeStatusByOwnerIdAsync(
                    ownerId, statusCode, userInfo);

                result.Data = response;
                result.StatusCode = ResultStatusCode.OK;
                result.HasError = false;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            return result;
        }

        public async Task<ReturnModelDTO_Pagination> GetLatestPublished(
            SearchFiltersToListingRent filters, int pageNumber, int pageSize)
        {
            try
            {
                var listingRentResult = await _listingRentRepository
                    .GetPublished(filters, pageNumber, pageSize);

                var result = new ReturnModelDTO<(List<ListingRentDTO>, PaginationMetadataDTO)>
                {
                    Data = (_mapper.Map<List<ListingRentDTO>>(listingRentResult.Item1),
                            _mapper.Map<PaginationMetadataDTO>(listingRentResult.Item2)),
                    StatusCode = ResultStatusCode.OK,
                    HasError = false
                };

                return new ReturnModelDTO_Pagination
                {
                    Data = result.Data.Item1,
                    pagination = result.Data.Item2,
                    HasError = result.HasError,
                    ResultError = result.ResultError,
                    StatusCode = result.StatusCode
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public async Task<ReturnModelDTO_Pagination> GetSortedByMostRentalsAsync(
            SearchFiltersToListingRent filters, int pageNumber, int pageSize)
        {
            try
            {
                var listingRentResult = await _listingRentRepository
                    .GetSortedByMostRentalsAsync(filters, pageNumber, pageSize);

                var result = new ReturnModelDTO<(List<ListingRentDTO>, PaginationMetadataDTO)>
                {
                    Data = (_mapper.Map<List<ListingRentDTO>>(listingRentResult.Item1),
                            _mapper.Map<PaginationMetadataDTO>(listingRentResult.Item2)),
                    StatusCode = ResultStatusCode.OK,
                    HasError = false
                };

                return new ReturnModelDTO_Pagination
                {
                    Data = result.Data.Item1,
                    pagination = result.Data.Item2,
                    HasError = result.HasError,
                    ResultError = result.ResultError,
                    StatusCode = result.StatusCode
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public async Task<ReturnModelDTO<List<ListingRentDTO>>> GetAllListingsRentsData(int ownerUserId, Dictionary<string, string> clientData, bool useTechnicalMessages = true)
        {
            ReturnModelDTO<List<ListingRentDTO>> result = new ReturnModelDTO<List<ListingRentDTO>>();
            try
            {
                List<TlListingRent> listings = await _listingRentRepository.GetAll(ownerUserId);
                if (listings?.Count > 0)
                {
                    string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                    _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");
                    foreach (var list in listings)
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
                listings = listings.OrderByDescending(x => x.ListingRentId).ToList();
                result = new ReturnModelDTO<List<ListingRentDTO>>
                {
                    Data = _mapper.Map<List<ListingRentDTO>>(listings),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.GetAllListingsRentsData", ex, new { ownerUserId }, UseTechnicalMessages));
            }
            return result;
        }

        public async Task<ReturnModelDTO<ListingRentDTO>> Get(long listingRentId,
            bool onlyActive, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            ReturnModelDTO<ListingRentDTO> result = new ReturnModelDTO<ListingRentDTO>();
            try
            {
                string userId = clientData["UserId"] ?? "-50";
                int _userID = -1;
                int.TryParse(userId, out _userID);
                TlListingRent listings = await _listingRentRepository.Get(listingRentId, _userID, onlyActive);
                if (listings == null)
                {
                    return new ReturnModelDTO<ListingRentDTO>
                    {
                        StatusCode = ResultStatusCode.NotFound,
                        ResultError = new ErrorCommonDTO
                        {
                            Code = ResultStatusCode.NotFound,
                            Message = "El listing no ha podido ser encontrado."
                        }
                    };
                }
                else if (!onlyActive || (onlyActive && listings.ListingStatusId == 3))
                {
                    if (listings?.TlListingPhotos?.Count > 0)
                    {
                        string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                        _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");
                        foreach (var item in listings.TlListingPhotos)
                        {
                            item.PhotoLink = $"{requestContext.HttpContext?.Request.Scheme}://{requestContext.HttpContext?.Request.Host}/{_basePath}/{item.Name}";
                        }
                    }
                    ListingRentDTO listingRentReult = _mapper.Map<ListingRentDTO>(listings);
                    if (listings.OwnerUser?.RegisterDate != null)
                    {
                        int[] registerDetails = AppUtils.GetTimeElapsed((DateTime)listings.OwnerUser.RegisterDate);
                        listingRentReult.Owner.RegisterDateDays = registerDetails[0];
                        listingRentReult.Owner.RegisterDateMonths = registerDetails[1];
                        listingRentReult.Owner.RegisterDateYears = registerDetails[2];
                    }
                    result = new ReturnModelDTO<ListingRentDTO>
                    {
                        Data = listingRentReult,
                        HasError = false,
                        StatusCode = ResultStatusCode.OK
                    };
                }
                else
                {
                    return new ReturnModelDTO<ListingRentDTO>
                    {
                        StatusCode = ResultStatusCode.NotFound,
                        ResultError = new ErrorCommonDTO
                        {
                            Code = ResultStatusCode.NotFound,
                            Message = "El listing no está activo o no ha podido ser encontrado."
                        }
                    };
                }

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.GetAllListingsRentsData", ex, new { listingRentId }, useTechnicalMessages));
            }
            return result;
        }

        public async Task<ReturnModelDTO<ProcessDataResult>> ProcessListingData(long listinRentId,
            ProcessDataRequest request, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            ReturnModelDTO<ProcessDataResult> result = new ReturnModelDTO<ProcessDataResult>();
            try
            {
                string userId = clientData["UserId"] ?? "-50";
                int _userID = -1;
                int.TryParse(userId, out _userID);

                ListingProcessDataModel request_ = _mapper.Map<ListingProcessDataModel>(request);
                ReturnModel<ListingProcessDataResultModel> changeResult = await _listingRentService.ProcessData(listinRentId, request.ViewCode, request_, _userID, clientData, useTechnicalMessages);

                if (changeResult.StatusCode == ResultStatusCode.OK)
                {
                    if (changeResult.Data?.ListingData?.ListingPhotos?.Count > 0)
                    {
                        string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                        _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");
                        foreach (var item in changeResult.Data.ListingData.ListingPhotos)
                        {
                            item.PhotoLink = $"{requestContext.HttpContext?.Request.Scheme}://{requestContext.HttpContext?.Request.Host}/{_basePath}/{item.Name}";
                        }

                    }

                    result = _mapper.Map<ReturnModelDTO<ProcessDataResult>>(changeResult);
                }
                else
                {
                    result = _mapper.Map<ReturnModelDTO<ProcessDataResult>>(changeResult);
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.ProcessListingData", ex, new { listinRentId, request, clientData }, useTechnicalMessages));
            }
            return result;
        }

        public async Task<List<ReturnModelDTO>> UploadImages(IEnumerable<IFormFile> imageFiles, Dictionary<string, string> clientData)
        {
            List<ReturnModelDTO> result = [];
            try
            {
                var savingResult = await _imageService.SaveListingRentImages(imageFiles, true);
                result = _mapper.Map<List<ReturnModelDTO>>(savingResult);
                return result;
            }
            catch (Exception ex)
            {
                return new List<ReturnModelDTO>
                {
                    new ReturnModelDTO
                    {
                        StatusCode = ResultStatusCode.InternalError,
                        HasError = true,
                        ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.ProcessListingData", ex, new { imageFiles = imageFiles?.Select(x=>x.FileName), clientData }, UseTechnicalMessages))
                    }
                };

            }
        }

        public async Task<(ReturnModelDTO<List<ListingRentDTO>>, PaginationMetadataDTO)> GetFeaturedListings(long userId, int? countryId, int pageNumber, int pageSize, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO<List<ListingRentDTO>> result = new ReturnModelDTO<List<ListingRentDTO>>();
            PaginationMetadataDTO paginatonResult = new PaginationMetadataDTO
            {
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
            try
            {
                (List<TlListingRent> listings, PaginationMetadata paginaton) = await _listingRentRepository.GetFeatureds(userId, pageNumber, pageSize, countryId);

                if (listings?.Count > 0)
                {
                    string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                    _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");
                    foreach (var list in listings)
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

                result = new ReturnModelDTO<List<ListingRentDTO>>
                {
                    Data = _mapper.Map<List<ListingRentDTO>>(listings),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
                paginatonResult = _mapper.Map<PaginationMetadataDTO>(paginaton);
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.GetAllListingsRentsData", ex, new { countryId, pageNumber, pageSize }, UseTechnicalMessages));
            }
            return (result, paginatonResult);
        }

        public async Task<ReturnModelDTO<List<ListingRentDTO>>> GetByOwner(Dictionary<string, string> clientData, bool userTechnicalMessages)
        {
            string userId = clientData["UserId"] ?? "-50";
            int _userID = -1;
            int.TryParse(userId, out _userID);
            var result = await GetByUserId(_userID, clientData, userTechnicalMessages);
            return result;
        }

        public async Task<ReturnModelDTO<List<ListingRentDTO>>> GetByUserId(int userId, Dictionary<string, string> clientData, bool userTechnicalMessages)
        {
            ReturnModelDTO<List<ListingRentDTO>> result = new ReturnModelDTO<List<ListingRentDTO>>();
            try
            {
                List<TlListingRent> listings = await _listingRentRepository.GetAll(userId);
                if (listings?.Count > 0)
                {
                    string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                    _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");
                    foreach (var list in listings)
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
                result = new ReturnModelDTO<List<ListingRentDTO>>
                {
                    Data = _mapper.Map<List<ListingRentDTO>>(listings),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.GetByOwner", ex, new { userId }, UseTechnicalMessages));
            }
            return result;
        }

        public async Task<ReturnModelDTO<List<PhotoDTO>>> GetPhotoByListigRent(long listinRentId, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            string userId = clientData["UserId"] ?? "-50";
            int _userID = -1;
            int.TryParse(userId, out _userID);

            ReturnModelDTO<List<PhotoDTO>> result = new ReturnModelDTO<List<PhotoDTO>>();
            try
            {
                List<TlListingPhoto> listings = await _listingPhotoRepository.GetByListingRentId(listinRentId, _userID);

                string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");
                foreach (var item in listings)
                {
                    item.PhotoLink = $"{requestContext.HttpContext?.Request.Scheme}://{requestContext.HttpContext?.Request.Host}/{_basePath}/{item.Name}";
                }

                result = new ReturnModelDTO<List<PhotoDTO>>
                {
                    Data = _mapper.Map<List<PhotoDTO>>(listings),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.GetByOwner", ex, new { userId }, UseTechnicalMessages));
            }
            return result;
        }

        public async Task<ReturnModelDTO<List<ReviewDTO>>> GetListingRentReviews(int listingRentId, bool UseTechnicalMessages, Dictionary<string, string> requestInfo)
        {

            ReturnModelDTO<List<ReviewDTO>> result = new ReturnModelDTO<List<ReviewDTO>>();
            try
            {
                List<TlListingReview> listings = await _listingReviewRepository.GetByListingRent(listingRentId);
                result = new ReturnModelDTO<List<ReviewDTO>>
                {
                    Data = _mapper.Map<List<ReviewDTO>>(listings),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.GetListingRentReviews", ex, new { listingRentId }, UseTechnicalMessages));
            }
            return result;
        }


        public async Task<ReturnModelDTO<List<ReviewDTO>>> GetByOwnerId(int userId, bool UseTechnicalMessages, Dictionary<string, string> requestInfo)
        {

            ReturnModelDTO<List<ReviewDTO>> result = new ReturnModelDTO<List<ReviewDTO>>();
            try
            {
                List<TlListingReview> listings = await _listingReviewRepository.GetByOwnerId(userId);
                result = new ReturnModelDTO<List<ReviewDTO>>
                {
                    Data = _mapper.Map<List<ReviewDTO>>(listings),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.GetByOwnerId", ex, new { userId }, UseTechnicalMessages));
            }
            return result;
        }
        public async Task<ReturnModelDTO<ListingReviewSummaryDTO>> GetListingRentReviewsSummary(long listingRentId, int topCount, bool UseTechnicalMessages, Dictionary<string, string> requestInfo)
        {

            ReturnModelDTO<ListingReviewSummaryDTO> result = new ReturnModelDTO<ListingReviewSummaryDTO>();
            try
            {
                ListingReviewSummary listings = await _listingReviewRepository.GetReviewSummary(listingRentId, topCount);
                result = new ReturnModelDTO<ListingReviewSummaryDTO>
                {
                    Data = _mapper.Map<ListingReviewSummaryDTO>(listings),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.GetListingRentReviewsSummary", ex, new { listingRentId, topCount }, UseTechnicalMessages));
            }
            return result;
        }


        public async Task<List<ReturnModelDTO>> UploadImagesDescription(long listingRentId, List<UploadImageRequest> images, Dictionary<string, string> clientData)
        {
            string userId = clientData["UserId"] ?? "-50";
            int _userID = -1;
            int.TryParse(userId, out _userID);

            List<ReturnModelDTO> result = [];
            try
            {
                var imageFiles = _mapper.Map<List<UploadImageListingRent>>(images);
                int i = -1;
                var savingResult = await _imageService.SaveListingRentImages(listingRentId, imageFiles, _userID, true);
                result = _mapper.Map<List<ReturnModelDTO>>(savingResult);
                return result;
            }
            catch (Exception ex)
            {
                return new List<ReturnModelDTO>
                {
                    new ReturnModelDTO
                    {
                        StatusCode = ResultStatusCode.InternalError,
                        HasError = true,
                        ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.UploadImages", ex, new { images, clientData }, UseTechnicalMessages))
                    }
                };

            }
        }


        public async Task<ReturnModelDTO> DeletePhoto(long listingRentId, int photoId, Dictionary<string, string> clientData)
        {
            ReturnModelDTO result = null;
            try
            {
                string userId = clientData["UserId"] ?? "-50";
                int _userID = -1;
                int.TryParse(userId, out _userID);
                ReturnModel savingResult = await _imageService.DeleteListingRentImage(listingRentId, photoId, _userID, true);
                result = _mapper.Map<ReturnModelDTO>(savingResult);
                return result;
            }
            catch (Exception ex)
            {
                return new ReturnModelDTO
                {
                    StatusCode = ResultStatusCode.InternalError,
                    HasError = true,
                    ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.DeletePhoto", ex, new { listingRentId, photoId, clientData }, UseTechnicalMessages))

                };
            }
        }

        public async Task<ReturnModelDTO<PhotoDTO>> UpdatePhoto(long listingRentId, int photoId, UploadImageListingRent request, Dictionary<string, string> clientData)
        {
            ReturnModelDTO<PhotoDTO> result = null;
            try
            {
                string userId = clientData["UserId"] ?? "-50";
                int _userID = -1;
                int.TryParse(userId, out _userID);
                ReturnModel<TlListingPhoto> savingResult = await _imageService.UpdatePhoto(listingRentId, photoId, request, _userID, true);
                result = new ReturnModelDTO<PhotoDTO>
                {
                    Data = _mapper.Map<PhotoDTO>(savingResult.Data),
                    HasError = savingResult.HasError,
                    ResultError = _mapper.Map<ErrorCommonDTO>(savingResult.ResultError),
                    StatusCode = savingResult.StatusCode
                };
                return result;
            }
            catch (Exception ex)
            {
                return new ReturnModelDTO<PhotoDTO>
                {
                    StatusCode = ResultStatusCode.InternalError,
                    HasError = true,
                    ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.UpdatePhoto", ex, new { listingRentId, photoId, request, clientData }, UseTechnicalMessages))

                };
            }
        }

        public async Task<ReturnModelDTO<string>> UpdateBasicData(long listingRentId,
            BasicListingRentData basicData, int userId)
        {
            ReturnModelDTO<string> result = null;
            try
            {
                string updatedResult = await _listingRentRepository.UpdateBasicData(listingRentId, basicData.Title, basicData.Description, basicData.AspectTypeIdList, userId);
                result = _mapper.Map<ReturnModelDTO<string>>(updatedResult);
                return result;
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, basicData);
                throw new Exceptions.ApplicationException(ex.Message);
            }
        }

        public async Task<ReturnModelDTO<string>> UpdatePricesAndDiscounts(long listingRentId,
            PricesAndDiscountRequest pricingData, int userId)
        {
            ReturnModelDTO<string> result = null;
            try
            {
                await _listingPriceRepository.SetPricing(listingRentId, pricingData.NightlyPrice,
                    pricingData.WeekendNightlyPrice, userId, pricingData.CurrencyId);

                if (pricingData.DiscountPrices != null)
                {
                    string response = await _listingDiscountForRateRepository.SetDiscounts(listingRentId, pricingData.DiscountPrices
                                .Select(d => (d.DiscountId, d.Percentage)).ToList());
                }

                result = _mapper.Map<ReturnModelDTO<string>>("UPDATE");
                return result;
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, pricingData);
                throw new Exceptions.ApplicationException(ex.Message);
            }
        }

        public async Task<ReturnModelDTO<ListingRentDTO>> Get(long listingRentId, long userId, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            ReturnModelDTO<ListingRentDTO> result = new ReturnModelDTO<ListingRentDTO>();
            try
            {
                TlListingRent listings = await _listingRentRepository.Get(listingRentId, userId);
                if (listings == null)
                {
                    return new ReturnModelDTO<ListingRentDTO>
                    {
                        StatusCode = ResultStatusCode.NotFound,
                        ResultError = new ErrorCommonDTO
                        {
                            Code = ResultStatusCode.NotFound,
                            Message = "El listing no ha podido ser encontrado."
                        }
                    };
                }
                else
                {
                    if (listings?.TlListingPhotos?.Count > 0)
                    {
                        string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                        _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");
                        foreach (var item in listings.TlListingPhotos)
                        {
                            item.PhotoLink = $"{requestContext.HttpContext?.Request.Scheme}://{requestContext.HttpContext?.Request.Host}/{_basePath}/{item.Name}";
                        }
                    }
                    ListingRentDTO listingRentReult = _mapper.Map<ListingRentDTO>(listings);
                    if (listings.OwnerUser?.RegisterDate != null)
                    {
                        int[] registerDetails = AppUtils.GetTimeElapsed((DateTime)listings.OwnerUser.RegisterDate);
                        listingRentReult.Owner.RegisterDateDays = registerDetails[0];
                        listingRentReult.Owner.RegisterDateMonths = registerDetails[1];
                        listingRentReult.Owner.RegisterDateYears = registerDetails[2];
                    }
                    result = new ReturnModelDTO<ListingRentDTO>
                    {
                        Data = listingRentReult,
                        HasError = false,
                        StatusCode = ResultStatusCode.OK
                    };
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.GetAllListingsRentsData", ex, new { listingRentId }, useTechnicalMessages));
            }
            return result;
        }

        public async Task<ReturnModelDTO<List<ListingRentResumeDTO>>> GetByOwnerResumed(Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            string userId = clientData["UserId"] ?? "-50";
            int _userID = -1;
            int.TryParse(userId, out _userID);

            ReturnModelDTO<List<ListingRentResumeDTO>> result = new ReturnModelDTO<List<ListingRentResumeDTO>>();
            try
            {
                List<TlListingRent> listings = await _listingRentRepository.GetAllResumed(_userID, true);
                //if (listings?.Count > 0)
                //{
                //    string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                //    _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");
                //    foreach (var list in listings)
                //    {
                //        if (list?.TlListingPhotos?.Count > 0)
                //        {
                //            foreach (var item in list.TlListingPhotos)
                //            {
                //                item.PhotoLink = $"{requestContext.HttpContext?.Request.Scheme}://{requestContext.HttpContext?.Request.Host}/{_basePath}/{item.Name}";
                //            }
                //        }
                //    }
                //}
                result = new ReturnModelDTO<List<ListingRentResumeDTO>>
                {
                    Data = _mapper.Map<List<ListingRentResumeDTO>>(listings),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.GetByOwner", ex, new { userId }, UseTechnicalMessages));
            }
            return result;
        }

        public async Task<ReturnModelDTO<string>> UpdatePropertyAndAccomodationTypes(long listingRentId,
            int? propertyTypeId, int? accomodationTypeId, int userId)
        {
            if (propertyTypeId is not null && propertyTypeId > 0)
            {
                var tpProperty = await _propertyRepository.GetFromListingId(listingRentId);
                var _ = await _propertyRepository
                    .SetPropertySubType(tpProperty.PropertyId, propertyTypeId!);
            }

            if (accomodationTypeId is not null && accomodationTypeId > 0)
            {
                var _ = await _listingRentRepository
                    .SetAccomodationType(listingRentId, accomodationTypeId!, userId);
            }

            return new ReturnModelDTO<string>
            {
                Data = "UPDATED",
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        public async Task<ReturnModelDTO<string>> UpdateCapacity(long listingRentId,
            int beds, int bedrooms, int bathrooms, int maxGuests,
            int privateBathroom, int privateBathroomLodging, int sharedBathroom, int userId)
        {
            await _listingRentRepository.SetCapacity(listingRentId, beds, bedrooms,
                bathrooms, maxGuests, privateBathroom, privateBathroomLodging, sharedBathroom, userId);

            return new ReturnModelDTO<string>
            {
                Data = "UPDATED",
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        //public async Task<ReturnModelDTO<string>> UpdatePropertyLocation(long listingRentId,
        //    int cityId, int countyId, int stateId, double latitude, double longitude,
        //    string address1, string address2, string zipCode, string Country, string State, string County, string City, string Street)
        //{
        //    var property = await _propertyRepository.GetFromListingId(listingRentId);

        //    if(property is null)
        //        throw new ApplicationException("No se ha encontrado el listado de renta, verifica los datos e intenta de nuevo.");

        //    TpPropertyAddress addresInput = new TpPropertyAddress
        //    {
        //        Address1 = address1,
        //        Address2 = address2,
        //        ZipCode = zipCode,
        //    };

        //    if (cityId > 0) addresInput.CityId = cityId;
        //    if (countyId > 0) addresInput.CountyId = countyId;
        //    if (stateId > 0) addresInput.StateId = stateId;

        //    TpPropertyAddress addressResult = await _propertyAddressRepository.Set(
        //        addresInput, property.PropertyId);

        //    if (latitude != 0 && longitude != 0)
        //    {
        //        if (!GeoUtils.ValidateLatitudLongitude(latitude, longitude))
        //            throw new ApplicationException("La Latitud o Longitud ingresada no es válida, verifica los datos e intenta de nuevo.");

        //        await _propertyRepository.SetLocation(property.PropertyId, 
        //            latitude, longitude);
        //    }

        //    return new ReturnModelDTO<string>
        //    {
        //        Data = "UPDATED",
        //        HasError = false,
        //        StatusCode = ResultStatusCode.OK
        //    };
        //}

        public async Task<ReturnModelDTO<string>> UpdatePropertyLocation(long listingRentId,
            int cityId, int countyId, int stateId, double? latitude, double? longitude,
            string address1, string address2, string zipCode, string Country, string State, string County, string City, string Street)
        {
            var property = await _propertyRepository.GetFromListingId(listingRentId);

            if (property is null)
                throw new ApplicationException("No se ha encontrado el listado de renta, verifica los datos e intenta de nuevo.");

            LocationModel? location = null;

            if (!Country.IsNullOrEmpty() || !State.IsNullOrEmpty() || !County.IsNullOrEmpty() || !City.IsNullOrEmpty() || !Street.IsNullOrEmpty())
            {
                location = await _locationService.ResolveLocationAdRegister(Country, State, County, City, Street);
            }

            TpPropertyAddress addresInput = new TpPropertyAddress
            {
                Address1 = address1,
                Address2 = address2,
                ZipCode = zipCode,
            };
            if (location != null)
            {
                if (location.CityId > 0)
                {
                    addresInput.CityId = location.CityId;
                    addresInput.CountyId = null;
                    addresInput.StateId = null;
                }
                else
                {
                    if (location.CountyId > 0)
                    {
                        addresInput.CityId = null;
                        addresInput.CountyId = location.CountyId;
                        addresInput.StateId = null;
                    }
                    else
                    {
                        if (location.StateId > 0)
                        {
                            addresInput.CityId = null;
                            addresInput.CountyId = null;
                            addresInput.StateId = location.StateId;
                        }
                    }
                }
            }
            else
            {
                if (cityId > 0)
                {
                    addresInput.CityId = cityId;
                    addresInput.CountyId = null;
                    addresInput.StateId = null;
                }
                else
                {
                    if (countyId > 0)
                    {
                        addresInput.CityId = null;
                        addresInput.CountyId = countyId;
                        addresInput.StateId = null;
                    }
                    else
                    {
                        if (stateId > 0)
                        {
                            addresInput.CityId = null;
                            addresInput.CountyId = null;
                            addresInput.StateId = stateId;
                        }
                    }
                }
            }

            if (latitude != null && longitude != null && latitude != 0 && longitude != 0)
            {
                if (!GeoUtils.ValidateLatitudLongitude(latitude, longitude))
                {
                    return new ReturnModelDTO<string>
                    {
                        HasError = true,
                        StatusCode = ResultStatusCode.BadRequest,
                        ResultError = new ErrorCommonDTO
                        {
                            Code = ResultStatusCode.BadRequest,
                            Message = "La Latitud o Longitud ingresada no es válida, verifica los datos e intenta de nuevo.",
                        }
                    };
                }
            }
            else
            {
                latitude = null;
                longitude = null;
            }

            TpPropertyAddress addressResult = await _propertyAddressRepository.Set(addresInput, property.PropertyId);
            if (latitude != 0 && longitude != 0)
            {
                Thread.Sleep(1000);
                await _propertyRepository.SetLocation(property.PropertyId, latitude, longitude);
            }

            return new ReturnModelDTO<string>
            {
                Data = "UPDATED",
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        public async Task<ReturnModelDTO<string>> UpdateCharasteristics(long listingRentId,
            Dictionary<string, string> clientData, List<int> featuredAmenities, List<int> featureAspects,
            List<int> securityItems)
        {
            //if (featuredAmenities is { Count: > 0 })
            await _listingAmenitiesRepository.SetListingAmmenities(listingRentId,
                featuredAmenities, clientData, true);

            //if (featureAspects is { Count: > 0 })
            await _listingFeaturedAspectRepository.SetListingFeaturesAspects(listingRentId,
                featureAspects);

            //if (securityItems is { Count: > 0 })
            await _listingSecurityItemsRepository.SetListingSecurityItems(listingRentId,
                securityItems);

            return new ReturnModelDTO<string>
            {
                Data = "UPDATED",
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        public async Task<ReturnModelDTO<string>> UpdateCancellationPolicy(long listingRentId,
            int cancellationPolicyId, int userId)
        {
            if (cancellationPolicyId <= 0)
                throw new ApplicationException("El ID de la política de cancelación no es válido, verifica los datos e intenta de nuevo.");

            await _listingRentRepository.SetCancellationPolicy(listingRentId, cancellationPolicyId, userId);

            return new ReturnModelDTO<string>
            {
                Data = "UPDATED",
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        public async Task<ReturnModelDTO<string>> UpdateReservation(long listingRentId,
           int approvalPolicyTypeId, int minimunNoticeDays, int preparationDays, int userId)
        {
            if (approvalPolicyTypeId <= 0 || minimunNoticeDays <= 0 || preparationDays <= 0)
                throw new ApplicationException("Los datos de la política de reserva no son válidos, verifica los datos e intenta de nuevo.");

            await _listingRentRepository.SetReservationTypeApprobation(listingRentId,
                approvalPolicyTypeId, minimunNoticeDays, preparationDays, userId);

            return new ReturnModelDTO<string>
            {
                Data = "UPDATED",
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        public async Task<ReturnModelDTO<string>> UpdatePricingAndDiscounts(long listingRentId,
            decimal pricing, decimal weekendPrice, int currencyId,
            List<(int, decimal)> discounts, int userId)
        {
            if (!(pricing > 0))
                throw new ApplicationException("Debe definir el precio de alquiler por noche  de la propiedad.");

            if (!(weekendPrice > 0))
                throw new ApplicationException("Debe definir el precio de alquiler por noche los fines de semana de la propiedad.");

            await _listingPriceRepository.SetPricing(listingRentId, pricing, weekendPrice, userId, currencyId);
            await _listingDiscountRepository.SetDiscounts(listingRentId, discounts, userId);

            return new ReturnModelDTO<string>
            {
                Data = "UPDATED",
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        public async Task<ReturnModelDTO<string>> UpdateWeekendPricing(long listingRentId, decimal weekendPrice, int currencyId, int userId)
        {
            if (!(weekendPrice > 0))
                throw new ApplicationException("Debe definir el precio de alquiler por noche los fines de semana de la propiedad.");

            await _listingPriceRepository.SetWeekendPricing(listingRentId, weekendPrice, userId, currencyId);

            return new ReturnModelDTO<string>
            {
                Data = "UPDATED",
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        public async Task<ReturnModelDTO<string>> UpdateCheckInOutAndRules(long listingRentId,
            string checkinTime, string checkoutTime, string maxCheckinTime, string instructions,
            List<int> rules, int userId)
        {
            if (string.IsNullOrEmpty(checkinTime) || string.IsNullOrEmpty(checkoutTime))
                throw new ApplicationException("Debe definir las horas de Checin y checkout.");

            await _listingRentRepository.SetCheckInPolicies(listingRentId, checkinTime, checkoutTime, maxCheckinTime, instructions, userId);

            if (rules is { Count: > 0 })
                await _listingRentRulesRepository.Set(listingRentId, rules);

            return new ReturnModelDTO<string>
            {
                Data = "UPDATED",
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        public async Task<ReturnModelDTO<string>> UpdateRules(long listingRentId,
            List<int> rules)
        {
            if (rules is not { Count: > 0 })
                throw new ApplicationException("Debe definir al menos una regla para la publicacion.");

            await _listingRentRulesRepository.Set(listingRentId, rules);

            return new ReturnModelDTO<string>
            {
                Data = "UPDATED",
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        public async Task<ReturnModelDTO<List<ListingRentCalendarDTO>>> GetCalendarByOwner(Dictionary<string, string> clientData, bool v)
        {
            string userId = clientData["UserId"] ?? "-50";
            int _userID = -1;
            int.TryParse(userId, out _userID);

            ReturnModelDTO<List<ListingRentCalendarDTO>> result = new ReturnModelDTO<List<ListingRentCalendarDTO>>();
            try
            {
                List<TlListingRent> listings = await _listingRentRepository.GetCalendarData(_userID);
                if (listings?.Count > 0)
                {
                    string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                    _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");
                    foreach (var list in listings)
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
                result = new ReturnModelDTO<List<ListingRentCalendarDTO>>
                {
                    Data = _mapper.Map<List<ListingRentCalendarDTO>>(listings),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.GetByOwner", ex, new { userId }, UseTechnicalMessages));
            }
            return result;
        }

        public async Task<ReturnModelDTO<ProcessDataResult>> GetLastView(long listinRentId, int ownerId)
        {
            ReturnModelDTO<ProcessDataResult> result = new ReturnModelDTO<ProcessDataResult>();
            try
            {
                ReturnModel<ListingProcessDataResultModel> viewResult = await _listingRentService.GetLastView(listinRentId, ownerId);

                if (viewResult.StatusCode == ResultStatusCode.OK)
                {
                    if (viewResult.Data?.ListingData?.ListingPhotos?.Count > 0)
                    {
                        string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                        _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");
                        foreach (var item in viewResult.Data.ListingData.ListingPhotos)
                        {
                            item.PhotoLink = $"{requestContext.HttpContext?.Request.Scheme}://{requestContext.HttpContext?.Request.Host}/{_basePath}/{item.Name}";
                        }

                    }

                    result = _mapper.Map<ReturnModelDTO<ProcessDataResult>>(viewResult);
                }
                else
                {
                    result = _mapper.Map<ReturnModelDTO<ProcessDataResult>>(viewResult);
                }

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.GetLastView", ex, new { listinRentId, ownerId }, UseTechnicalMessages));
            }
            return result;
        }

        public async Task<ReturnModelDTO<List<ListingRentDTO>>> GetUnfinished(int ownerId)
        {
            ReturnModelDTO<List<ListingRentDTO>> result = new ReturnModelDTO<List<ListingRentDTO>>();
            try
            {
                List<TlListingRent> listings = await _listingRentRepository.GetUnfinishedList(ownerId);

                result = new ReturnModelDTO<List<ListingRentDTO>>
                {
                    Data = _mapper.Map<List<ListingRentDTO>>(listings),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.GetUnfinished", ex, new { ownerId }, UseTechnicalMessages));
            }
            return result;
        }

        public async Task<ReturnModelDTO<string>> UpdatePhotoPosition(long listingRentId, long listingPhotoId, int newPostition)
        {
            try
            {
                var updateResult = await _listingPhotoRepository
                    .UpdatePhotoPosition(listingRentId, listingPhotoId, newPostition);

                return new ReturnModelDTO<string>
                {
                    HasError = false,
                    StatusCode = ResultStatusCode.OK,
                    Data = updateResult
                };
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { listingRentId, listingPhotoId, newPostition });
                throw new Exceptions.ApplicationException(ex.Message);
            }
        }

        public async Task<ReturnModelDTO<string>> SortListingRentPhotos()
        {
            try
            {
                var sortResult = await _listingPhotoRepository.SortListingRentPhotos();

                return new ReturnModelDTO<string>
                {
                    HasError = false,
                    StatusCode = ResultStatusCode.OK,
                    Data = sortResult
                };

            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, 0);
                throw new Exceptions.ApplicationException(ex.Message);
            }
        }

        public async Task<ReturnModelDTO<string>> UpsertMaxMinStay(UpsertMaxMinStayRequestDTO request, int userId)
        {
            if (!request.SetMaxStay && !request.SetMinStay)
                throw new ApplicationException("Requiere modificar al menos minimo o maximo de estancia.");

            var result = await _listingRentRepository.SetMaxMinStay(request.ListingRentId, request.SetMaxStay,
                request.MaxStay, request.SetMinStay, request.MinStay, userId);

            return new ReturnModelDTO<string>
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK,
                Data = result
            };
        }

        public async Task<ReturnModelDTO<string>> UpsertReservationNotice(UpsertMinimumNoticeRequestDTO request, int userId)
        {
            var result = await _listingRentRepository.SetMinimumNotice(request.ListingRentId,
                request.MinimumNoticeDay, request.MinimumNoticeHours, userId);

            return new ReturnModelDTO<string>
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK,
                Data = result
            };
        }

        public async Task<ReturnModelDTO<string>> UpsertPreparationDay(UpsertPreparationDayRequestDTO request, int userId)
        {
            var result = await _listingRentRepository.SetPreparationDay(request.ListingRentId,
                request.PreparationDayValue, userId);

            return new ReturnModelDTO<string>
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK,
                Data = result
            };
        }

        public async Task<ReturnModelDTO<string>> UpsertAdditionalFee(
            UpsertAdditionalFeeRequestDTO request)
        {
            var result = await _listingRentRepository.SetAdditionalFee(request.ListingRentId,
                request.AdditionalFeeIds, request.AdditionalFeeValues);

            return new ReturnModelDTO<string>
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK,
                Data = result
            };
        }

        public async Task<ReturnModelDTO<List<ListingAdditionalFeeDTO>>> GetAdditionalFee(long listingRentId)
        {
            var result = new ReturnModelDTO<List<ListingAdditionalFeeDTO>>()
            { StatusCode = ResultStatusCode.OK, HasError = false };

            var response = await _listingRentRepository.GetAdditionalFeesByListingRentId(listingRentId);

            result.Data = _mapper.Map<List<ListingAdditionalFeeDTO>>(response);

            return result;
        }

        public async Task<ReturnModelDTO<HostProfileAndListingRentDTO>> GetHotProfileAndListingRent(long hostId, long listingRentId)
        {
            var result = new ReturnModelDTO<HostProfileAndListingRentDTO>()
            { StatusCode = ResultStatusCode.OK, HasError = false };

            var response = await _listingRentRepository.GetHotProfileAndListingRent(hostId, listingRentId);

            result.Data = _mapper.Map<HostProfileAndListingRentDTO>(response);

            return result;
        }

    }
}
