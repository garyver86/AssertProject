using Assert.Domain.Models;

namespace Assert.Infrastructure.Security
{
    public interface IAuthentication
    {
        Task<ReturnModel> UserLogin(string user, string password, string ip, string browserInfo);
    }
}
