using Assert.Application.DTOs;
using Assert.Application.Interfaces;
using Assert.Domain.Models;
using Assert.Domain.Services;
using AutoMapper;

namespace Assert.Application.Services
{
    internal class AppUserService : IAppUserService
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IErrorHandler _errorHandler;
        public AppUserService(IUserService userService, IMapper mapper, IErrorHandler errorHandler)
        {
            _userService = userService;
            _mapper = mapper;
            _errorHandler = errorHandler;
        }
        public async Task<ReturnModelDTO> DisableHostRole(long userId, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            try
            {
                var listings = await _userService.DisableHostRole(userId);
                var dataResult = _mapper.Map<ReturnModelDTO>(listings);

                return dataResult;
            }
            catch (Exception ex)
            {
                return HandleException<ReturnModelDTO>("AppUserService.DisableHostRole", ex, new { userId }, useTechnicalMessages);
            }
        }

        public async Task<ReturnModelDTO> EnableHostRole(long userId, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            try
            {
                var listings = await _userService.EnableHostRole(userId);
                var dataResult = _mapper.Map<ReturnModelDTO>(listings);

                return dataResult;
            }
            catch (Exception ex)
            {
                return HandleException<ReturnModelDTO>("AppUserService.EnableHostRole", ex, new { userId }, useTechnicalMessages);
            }
        }

        private ReturnModelDTO<T> HandleException<T>(string action, Exception ex, object data, bool useTechnicalMessages)
        {
            var error = _errorHandler.GetErrorException(action, ex, data, useTechnicalMessages);
            return new ReturnModelDTO<T>
            {
                HasError = true,
                StatusCode = ResultStatusCode.InternalError,
                ResultError = _mapper.Map<ErrorCommonDTO>(error)
            };
        }
    }
}
