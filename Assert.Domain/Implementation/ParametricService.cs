using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Implementation
{
    public class ParametricService : IParametricService
    {
        private readonly IAccommodationTypeRepository _accommodationTypeRepository;
        private readonly IErrorHandler _errorHandler;
        public ParametricService(IAccommodationTypeRepository accommodationTypeRepository, IErrorHandler errorHandler)
        {
            _accommodationTypeRepository = accommodationTypeRepository;
            _errorHandler = errorHandler;
        }

        public async Task<ReturnModel<List<TlAccommodationType>>> GetAccomodationTypesActives()
        {
            ReturnModel<List<TlAccommodationType>> result = new ReturnModel<List<TlAccommodationType>>();
            try
            {
                var result_data = await _accommodationTypeRepository.GetActives();
                return new ReturnModel<List<TlAccommodationType>>
                {
                    StatusCode = ResultStatusCode.OK,
                    Data = result_data,
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("L_ListingRentView.GetAccomodationTypesActives", ex, null, true);
            }
            return result;
        }
    }
}
