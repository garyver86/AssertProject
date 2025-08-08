using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;

namespace Assert.Domain.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IErrorHandler _errorHandler;
        public UserService(IUserRoleRepository userRoleRepository, IErrorHandler errorHandler, IUserRepository userRepository)
        {
            _userRoleRepository = userRoleRepository;
            _errorHandler = errorHandler;
            _userRepository = userRepository;
        }
        public async Task<ReturnModel> DisableHostRole(long userId)
        {
            ReturnModel result = new ReturnModel();
            try
            {
                var result_data = await _userRoleRepository.DisableHostPermission(userId);
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("UserService.DisableHostRole", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel> EnableHostRole(long userId)
        {
            ReturnModel result = new ReturnModel();
            try
            {
                var result_data = await _userRoleRepository.EnableHostPermission(userId);
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("UserService.EnableHostRole", ex, null, true);
            }
            return result;
        }
        public async Task<ReturnModel> BlockAsHost(int userId, int userBlockedId)
        {
            ReturnModel result = new ReturnModel();
            try
            {
                var result_data = await _userRepository.BlockAsHost(userId, userBlockedId);
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("UserService.BlockAsHost", ex, new { userBlockedId, userId}, true);
            }
            return result;
        }
        public async Task<ReturnModel> UnblockAsHost(int userId, int userBlockedId)
        {
            ReturnModel result = new ReturnModel();
            try
            {
                var result_data = await _userRepository.UnblockAsHost(userId, userBlockedId);
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("UserService.UnblockAsHost", ex, new { userBlockedId, userId }, true);
            }
            return result;
        }
    }
}
