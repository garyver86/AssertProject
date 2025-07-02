using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.Services
{
    public class AppBookService : IAppBookService
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly IErrorHandler _errorHandler;
        public AppBookService(IBookService bookService,
            IMapper mapper, IErrorHandler errorHandler)
        {
            _bookService = bookService;
            _mapper = mapper;
            _errorHandler = errorHandler;
        }
        public async Task<ReturnModelDTO<PayPriceCalculationDTO>> CalculatePrice(long listingRentId, DateTime startDate, DateTime endDate, int guestId, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            ReturnModelDTO<PayPriceCalculationDTO> result = new ReturnModelDTO<PayPriceCalculationDTO>();
            try
            {
                ReturnModel<PayPriceCalculation> returnModel = await _bookService.CalculatePrice(listingRentId, startDate, endDate, guestId, clientData, useTechnicalMessages);

                if (returnModel.StatusCode == ResultStatusCode.OK)
                {
                    result = new ReturnModelDTO<PayPriceCalculationDTO>
                    {
                        Data = _mapper.Map<PayPriceCalculationDTO>(returnModel.Data),
                        HasError = false,
                        StatusCode = ResultStatusCode.OK
                    };
                }
                else
                {
                    result.StatusCode = returnModel.StatusCode;
                    result.HasError = true;
                    result.ResultError = _mapper.Map<ErrorCommonDTO>(returnModel.ResultError);
                }

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppListingRentService.GetListingRentReviews", ex, new { listingRentId, startDate, endDate, guestId, clientData }, useTechnicalMessages));
            }
            return result;
        }
    }
}
