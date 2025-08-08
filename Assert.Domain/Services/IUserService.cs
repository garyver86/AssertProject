using Assert.Domain.Models;

namespace Assert.Domain.Services
{
    public interface IUserService
    {
        Task<ReturnModel> EnableHostRole(long userId);
        Task<ReturnModel> DisableHostRole(long userId);
        Task<ReturnModel> BlockAsHost(int userId, int userBlockedId);
        Task<ReturnModel> UnblockAsHost(int userId, int userBlockedId);
    }
}
