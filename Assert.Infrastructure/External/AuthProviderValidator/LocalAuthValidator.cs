using Assert.Domain.Interfaces.Infraestructure.External;
using Assert.Domain.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Infrastructure.External.AuthProviderValidator;

public class LocalAuthValidator : IAuthProviderValidator
{
    public Task<AuthValidationResult> ValidateTokenAsync(string token)
    {
        throw new NotImplementedException();
    }
}
