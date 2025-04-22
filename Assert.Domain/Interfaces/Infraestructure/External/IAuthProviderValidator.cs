using Assert.Domain.Models;
using Assert.Domain.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Interfaces.Infraestructure.External;

public interface IAuthProviderValidator
{
    Task<ReturnModel> ValidateTokenAsync(string token);

    Task<ReturnModel> LoginAsync(string user, string password);
}
