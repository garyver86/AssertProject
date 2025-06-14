﻿using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Domain.ValueObjects;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.Services
{
    public class AppListingCalendarService : IAppListingCalendarService
    {
        private readonly IListingCalendarRepository _listingCalendarRepository;

        private readonly IMapper _mapper;
        private readonly IErrorHandler _errorHandler;
        public AppListingCalendarService(IListingCalendarRepository listingCalendarRepository,
            IMapper mapper, IErrorHandler errorHandler)
        {
            _listingCalendarRepository = listingCalendarRepository;
            _mapper = mapper;
            _errorHandler = errorHandler;
        }
        public async Task<ReturnModelDTO<CalendarResultPaginatedDTO>> GetCalendarDays(int listingRentId, DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 30)
        {
            ReturnModelDTO<CalendarResultPaginatedDTO> result = new ReturnModelDTO<CalendarResultPaginatedDTO>();
            try
            {
                (List<TlListingCalendar>,PaginationMetadata) result_ = await _listingCalendarRepository.GetCalendarDaysAsync(listingRentId, startDate, endDate, pageNumber, pageSize);

                result = new ReturnModelDTO<CalendarResultPaginatedDTO>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = _mapper.Map<CalendarResultPaginatedDTO>(result_)
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingCalendarService.GetCalendarDaysAsync", ex, new { listingRentId, startDate, endDate, pageNumber , pageSize }, false));
            }
            return result;
        }

        public async Task<ReturnModelDTO<CalendarResultPaginatedDTO>> GetCalendarDaysWithDetails(int listingRentId, DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 31)
        {
            ReturnModelDTO<CalendarResultPaginatedDTO> result = new ReturnModelDTO<CalendarResultPaginatedDTO>();
            try
            {
                (List<TlListingCalendar>, PaginationMetadata) result_ = await _listingCalendarRepository.GetCalendarDaysWithDetailsAsync(listingRentId, startDate, endDate, pageNumber, pageSize);

                result = new ReturnModelDTO<CalendarResultPaginatedDTO>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = _mapper.Map<CalendarResultPaginatedDTO>(result_)
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingCalendarService.GetCalendarDaysWithDetails", ex, new { listingRentId, startDate, endDate, pageNumber, pageSize }, false));
            }
            return result;
        }
        public async Task<ReturnModelDTO<List<CalendarDayDto>>> BlockDays(BulkBlockCalendarDaysRequest request)
        {
            ReturnModelDTO<List<CalendarDayDto>> result = new ReturnModelDTO<List<CalendarDayDto>>();
            try
            {
                List<TlListingCalendar> result_ = await _listingCalendarRepository.BulkBlockDaysAsync(request.ListingRentId, request.Dates, request.BlockType, request.BlockReason, request.BookId);

                result = new ReturnModelDTO<List<CalendarDayDto>>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = _mapper.Map<List<CalendarDayDto>>(result_)
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingCalendarService.BlockDays", ex, new { request }, false));
            }
            return result;
        }

        public async Task<ReturnModelDTO<List<CalendarDayDto>>> UnblockDays(BulkBlockCalendarDaysRequest request)
        {
            ReturnModelDTO<List<CalendarDayDto>> result = new ReturnModelDTO<List<CalendarDayDto>>();
            try
            {
                List<TlListingCalendar> result_ = await _listingCalendarRepository.BulkUnblockDaysAsync(request.ListingRentId, request.Dates);

                result = new ReturnModelDTO<List<CalendarDayDto>>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = _mapper.Map<List<CalendarDayDto>>(result_)
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingCalendarService.UnblockDays", ex, new { request }, false));
            }
            return result;
        }
    }
}
