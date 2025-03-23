using Assert.Domain.Entities;
using Assert.Domain.Models;

namespace Assert.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<ReturnModel<TuUser>> Get(int ownerUserId);
        Task<ReturnModel> Login(string username, string password, string ip, string browseInfo);
    }
}
