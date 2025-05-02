using Assert.Domain.Models;

namespace Assert.Domain.Interfaces.Infraestructure.External;

public interface IAuthProviderValidator
{
    Task<ReturnModel> ValidateTokenAsync(string token);

    Task<ReturnModel> LoginAsync(string user, string password);
}
