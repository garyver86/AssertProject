using Assert.Domain.Interfaces.Infraestructure.External;
using Assert.Domain.Models;
using Assert.Domain.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
