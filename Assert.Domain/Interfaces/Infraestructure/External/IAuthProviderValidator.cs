using Assert.Domain.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Interfaces.Infraestructure.External;

public interface IAuthProviderValidator
{
    Task<AuthValidationResult> ValidateTokenAsync(string token);
}
