using Assert.Domain.Interfaces.Infraestructure.External;
using Assert.Domain.Models;

namespace Assert.Infrastructure.External.AuthProviderValidator;

public class GoogleAuthValidator : IAuthProviderValidator
{
    public Task<ReturnModel> LoginAsync(string user, string password)
    {
        throw new NotImplementedException();
    }

    public Task<ReturnModel> ValidateTokenAsync(string token)
    {
        var result = new ReturnModel
        {
            StatusCode = ResultStatusCode.OK,
            HasError = false
        };

        return Task.FromResult(result);
    }
}
