using Assert.Domain.Models;
using Assert.Domain.Models.Profile;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Domain.ValueObjects;

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

        public async Task<ReturnModel<(List<Profile>, PaginationMetadata)>> SearchHosts(SearchFilters filters, int pageNumber, int pageSize)
        {
            ReturnModel<(List<Profile>, PaginationMetadata)> result = new ReturnModel<(List<Profile>, PaginationMetadata)>();
            try
            {
                var result_data = await _userRepository.SearchHostAsync(filters,pageNumber, pageSize);
                return new ReturnModel<(List<Profile>, PaginationMetadata)>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = (result_data.Data.Item1, result_data.Data.Item2)
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("UserService.SearchHosts", ex, new { filters, pageNumber, pageSize }, true);
            }
            return result;
        }
    }
}
