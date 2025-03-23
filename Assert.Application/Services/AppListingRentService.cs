using Assert.Application.DTOs;
using Assert.Application.Interfaces;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Domain.ValueObjects;
using AutoMapper;

namespace Assert.Application.Services
{
    internal class AppListingRentService : IAppListingRentService
    {
        private IListingRentService _listingRentService;

        private bool UseTechnicalMessages { get; set; } = false;
        private readonly IListingRentRepository _listingRentRepository;
        private readonly IListingStatusRepository _listingStatusRepository;

        private readonly IMapper _mapper;
        private readonly IErrorHandler _errorHandler;

        public AppListingRentService(IListingRentRepository listingRentRepository, IMapper mapper, IErrorHandler errorHandler,
            IListingStatusRepository listingStatusRepository, IListingRentService listingRentService)
        {
            _listingRentRepository = listingRentRepository;
            _mapper = mapper;
            _errorHandler = errorHandler;
            _listingStatusRepository = listingStatusRepository;
            _listingRentService = listingRentService;
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

        public Task<string> GetCalendar(long listingRentId, int ownerUserId, string startDate, string endDate)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetListingData(int? ownerId, int listingRentId, bool UseTechnicalMessages)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetOwnerCalendar(int ownerUserId, string startDate, string endDate)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPriceNightlySuggestion(int spaces, int year, int amenities, bool UseTechnicalMessages)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetViewsData(int ownerUserId, int listingRentId)
        {
            throw new NotImplementedException();
        }

        //public async Task<ReturnModelDTO<ListingRentDTO>> ProccessListingRentViewData(long listingRentId, string currentViewCode, string nextViewCode, string nextStepView, int ownerUserId,
        //    Dictionary<string, object> currentViewData, Dictionary<string, string> clientData, bool useTechnicalMessages = true)
        //{
        //    UseTechnicalMessages = useTechnicalMessages;
        //    ReturnModelDTO<ListingRentDTO> result = new ReturnModelDTO<ListingRentDTO>();


        //    try
        //    {
        //        var esult = await _listingRentService.ProccessListingRentViewData(listingRentId, currentViewCode, nextViewCode, nextStepView, ownerUserId, currentViewData, clientData, useTechnicalMessages);

        //    }
        //    catch (Exception ex)
        //    {
        //        result.HasError = true;
        //        result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("L_ListingRentView.ProccessListingRentViewData", ex, new
        //        {
        //            listingRentId,
        //            currentViewCode,
        //            nextViewCode,
        //            nextStepView,
        //            ownerUserId,
        //            currentViewData,
        //            clientData,
        //            useTechnicalMessages
        //        }, UseTechnicalMessages));
        //    }
        //    return await Task.FromResult(result);
        //}

        public Task<string> SetCalendar(long listingRentId, int ownerUserId, string startDate, string endDate, List<CalendarEvent> calendarEvents)
        {
            throw new NotImplementedException();
        }

        public Task<string> SetCalendarByDay(long listingRentId, int ownerUserId, string startDate, string endDate, List<YearCalendarEvent> Years)
        {
            throw new NotImplementedException();
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
                ReturnModel<ListingProcessDataResultModel> changeResult = await _listingRentService.ProcessData(request.ListingRentId, request.ViewCode, request_, clientData, useTechnicalMessages);

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
    }
}
