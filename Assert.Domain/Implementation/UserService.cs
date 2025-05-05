using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;

namespace Assert.Domain.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IErrorHandler _errorHandler;
        public UserService(IUserRoleRepository userRoleRepository, IErrorHandler errorHandler)
        {
            _userRoleRepository = userRoleRepository;
            _errorHandler = errorHandler;
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
    }
}
