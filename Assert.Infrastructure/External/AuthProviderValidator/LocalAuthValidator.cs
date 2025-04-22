using Assert.Domain.Common;
using Assert.Domain.Interfaces.Infraestructure.External;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Models;
using Assert.Domain.Models.Auth;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Assert.Infrastructure.Persistence.SQLServer.AssertDB;
using Assert.Infrastructure.Utils;
using Assert.Shared.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
