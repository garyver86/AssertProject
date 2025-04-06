using Assert.Domain.Models;

namespace Assert.Domain.Services
{
    public interface IUserService
    {
        Task<ReturnModel> EnableHostRole(long userId);
        Task<ReturnModel> DisableHostRole(long userId);
    }
}
