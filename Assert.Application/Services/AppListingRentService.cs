using Assert.Application.DTOs;
using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Shared.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Http;

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
        IExceptionLoggerService _exceptionLoggerService)
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

        public async Task<ReturnModelDTO<List<ListingRentDTO>>> GetAllListingsRentsData(int ownerUserId, Dictionary<string, string> clientData, bool useTechnicalMessages = true)
        {
            ReturnModelDTO<List<ListingRentDTO>> result = new ReturnModelDTO<List<ListingRentDTO>>();
            try
            {
                List<TlListingRent> listings = await _listingRentRepository.GetAll(ownerUserId);
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


        public async Task<ReturnModelDTO<ListingRentDTO>> Get(long listingRentId, bool onlyActive, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            ReturnModelDTO<ListingRentDTO> result = new ReturnModelDTO<ListingRentDTO>();
            try
            {
                string userId = clientData["UserId"] ?? "-50";
                int _userID = -1;
                int.TryParse(userId, out _userID);
                TlListingRent listings = await _listingRentRepository.Get(listingRentId, onlyActive);
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
                //else if (listings.OwnerUserId != _userID)
                //{
                //    return new ReturnModelDTO<ListingRentDTO>
                //    {
                //        StatusCode = ResultStatusCode.NotFound,
                //        ResultError = new ErrorCommonDTO
                //        {
                //            Code = ResultStatusCode.NotFound,
                //            Message = "No tiene permisos sobre el listing rent solicitado."
                //        }
                //    };
                //}
                else if (!onlyActive || (onlyActive && listings.ListingStatusId == 3))
                {
                    result = new ReturnModelDTO<ListingRentDTO>
                    {
                        Data = _mapper.Map<ListingRentDTO>(listings),
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

        public async Task<ReturnModelDTO<ProcessDataResult>> ProcessListingData(long listinRentId, ProcessDataRequest request, Dictionary<string, string> clientData, bool useTechnicalMessages)
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
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.ProcessListingData", ex, new { listinRentId, request, clientData }, UseTechnicalMessages));
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

        public async Task<ReturnModelDTO<List<ListingRentDTO>>> GetFeaturedListings(int? countryId, int pageNumber, int pageSize, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO<List<ListingRentDTO>> result = new ReturnModelDTO<List<ListingRentDTO>>();
            try
            {
                List<TlListingRent> listings = await _listingRentRepository.GetFeatureds(pageNumber, pageSize, countryId);
                //listings = listings.OrderByDescending(x => x.ListingRentId).ToList();
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
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.GetAllListingsRentsData", ex, new { countryId, pageNumber, pageSize }, UseTechnicalMessages));
            }
            return result;
        }

        public async Task<ReturnModelDTO<List<ListingRentDTO>>> GetByOwner(Dictionary<string, string> clientData, bool userTechnicalMessages)
        {
            string userId = clientData["UserId"] ?? "-50";
            int _userID = -1;
            int.TryParse(userId, out _userID);

            ReturnModelDTO<List<ListingRentDTO>> result = new ReturnModelDTO<List<ListingRentDTO>>();
            try
            {
                List<TlListingRent> listings = await _listingRentRepository.GetAll(_userID);
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

                //string _basePath = await _SystemConfigurationRepository.GetListingResourceUrl();

                //foreach (var item in listings)
                //{
                //    item.PhotoLink = _basePath + item.Name;
                //}

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
                result = _mapper.Map<ReturnModelDTO<PhotoDTO>>(savingResult);
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
            BasicListingRentData basicData)
        {
            ReturnModelDTO<string> result = null;
            try
            {
                string updatedResult = await _listingRentRepository.UpdateBasicData(listingRentId, basicData.Title, basicData.Description, basicData.AspectTypeIdList);
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
            PricesAndDiscountRequest pricingData)
        {
            ReturnModelDTO<string> result = null;
            try
            {
                await _listingPriceRepository.SetPricing(listingRentId, pricingData.NightlyPrice,
                    pricingData.WeekendNightlyPrice, pricingData.CurrencyId);

                if (pricingData.DiscountPrices != null)
                {
                    string response = await _listingDiscountForRateRepository.SetDiscounts(listingRentId, pricingData.DiscountPrices
                                .Select(d => (d.DiscountId, d.Price)).ToList());
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
    }
}
