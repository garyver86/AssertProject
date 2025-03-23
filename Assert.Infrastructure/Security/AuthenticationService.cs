using Assert.Domain.Models;
using Assert.Domain.Repositories;

namespace Assert.Infrastructure.Security
{
    public class AuthenticationService : IAuthentication
    {
        private IUserRepository _userRepository;

        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ReturnModel> UserLogin(string user, string password, string ip, string browseInfo)
        {
            ReturnModel? resultLogin = await _userRepository.Login(user, password, ip, browseInfo);
            return resultLogin;
        }
    }
}
