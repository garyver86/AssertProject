using Assert.Domain.Interfaces.Infraestructure.External;
using Assert.Domain.Models;
using Assert.Domain.Repositories;

namespace Assert.Infrastructure.External.AuthProviderValidator;

public class LocalAuthValidator(IUserRepository _userRepository) : IAuthProviderValidator
{
    public async Task<ReturnModel> LoginAsync(string user, string password)
    {
        var authenticationResult = await _userRepository.Login(user, password);

        return authenticationResult;
    }

    public Task<ReturnModel> ValidateTokenAsync(string token)
    {
        throw new NotImplementedException();
    }
}
