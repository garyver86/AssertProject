using Assert.Application.DTOs;
using Assert.Application.Interfaces;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace Assert.Application.Services
{
    internal class AppListingRentService : IAppListingRentService
    {
        private IListingRentService _listingRentService;

        private bool UseTechnicalMessages { get; set; } = false;
        private readonly IListingRentRepository _listingRentRepository;
        private readonly IListingStatusRepository _listingStatusRepository;
        private readonly IImageService _imageService;

        private readonly IMapper _mapper;
        private readonly IErrorHandler _errorHandler;

        public AppListingRentService(IListingRentRepository listingRentRepository, IMapper mapper, IErrorHandler errorHandler,
            IListingStatusRepository listingStatusRepository, IListingRentService listingRentService, IImageService imageService)
        {
            _listingRentRepository = listingRentRepository;
            _mapper = mapper;
            _errorHandler = errorHandler;
            _listingStatusRepository = listingStatusRepository;
            _listingRentService = listingRentService;
            _imageService = imageService;
        }

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
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("L_ListingRentView.ChangeStatus", ex, new { listingRentId, ownerUserId, newStatusCode, clientData }, UseTechnicalMessages));
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
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("L_ListingRentView.GetAllListingsRentsData", ex, new { ownerUserId }, UseTechnicalMessages));
            }
            return result;
        }


        public async Task<ReturnModelDTO<ListingRentDTO>> Get(long listingRentId, bool onlyActive, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            ReturnModelDTO<ListingRentDTO> result = new ReturnModelDTO<ListingRentDTO>();
            try
            {
                TlListingRent listings = await _listingRentRepository.Get(listingRentId, onlyActive);
                result = new ReturnModelDTO<ListingRentDTO>
                {
                    Data = _mapper.Map<ListingRentDTO>(listings),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("L_ListingRentView.GetAllListingsRentsData", ex, new { listingRentId }, useTechnicalMessages));
            }
            return result;
        }

        public async Task<ReturnModelDTO<ProcessDataResult>> ProcessListingData(long listinRentId, ProcessDataRequest request, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            ReturnModelDTO<ProcessDataResult> result = new ReturnModelDTO<ProcessDataResult>();
            try
            {
                ListingProcessDataModel request_ = _mapper.Map<ListingProcessDataModel>(request);
                ReturnModel<ListingProcessDataResultModel> changeResult = await _listingRentService.ProcessData(listinRentId, request.ViewCode, request_, clientData, useTechnicalMessages);

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
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("L_ListingRentView.ProcessListingData", ex, new { listinRentId, request, clientData }, UseTechnicalMessages));
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
                        ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("L_ListingRentView.ProcessListingData", ex, new { imageFiles = imageFiles?.Select(x=>x.FileName), clientData }, UseTechnicalMessages))
                    }
                };

            }
        }

        public async Task<ReturnModelDTO<List<ListingRentDTO>>> GetFeaturedListings(int? countryId, int? limit, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO<List<ListingRentDTO>> result = new ReturnModelDTO<List<ListingRentDTO>>();
            try
            {
                List<TlListingRent> listings = await _listingRentRepository.GetFeatureds(countryId, limit);
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
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("L_ListingRentView.GetAllListingsRentsData", ex, new { countryId, limit }, UseTechnicalMessages));
            }
            return result;
        }
    }
}
