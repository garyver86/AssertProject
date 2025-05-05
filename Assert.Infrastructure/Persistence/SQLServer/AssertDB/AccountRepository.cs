using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Assert.Infrastructure.Utils;
using Assert.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assert.Shared.Extensions;
using Assert.Domain.Common.Metadata;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB;

public class AccountRepository
    (IExceptionLoggerService _exceptionLoggerService, InfraAssertDbContext _dbContext,
    RequestMetadata _metadata)
    : IAccountRepository
{
    public async Task<int> Create(int userId, string password)
    {
        string pass = UtilsMgr.GetHash512(password);
        TuAccount account = new TuAccount()
        {
            UserId = userId,
            Password = pass,
            IncorrectAccess = 0,
            IsBlocked = false,
            LastBlockDate = null,
            DateLastLogin = null,
            IpLastLogin = null,
            Status = "AC",
            TemporaryBlockTo = null,
            ForceChange = false
        };

        try
        {
            await _dbContext.TuAccounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();
            return Convert.ToInt32(account.AccountId);
        }
        catch (Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, new { account });
            throw new InfrastructureException(ex.Message);
        }
    }

    public Task<ReturnModel<TuAccount>> Get(int accountId)
    {
        throw new NotImplementedException();
    }

    public Task<ReturnModel<TuAccount>> GetByUserId(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateLastSessionInfo()
    {
        try
        {
            await _dbContext.TuAccounts
                .Where(a => a.AccountId == _metadata.AccountId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(a => a.DateLastLogin, DateTime.UtcNow)
                    .SetProperty(a => a.IpLastLogin, _metadata.IpAddress));
        }
        catch (Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className,
                new { _metadata });
            throw new InfrastructureException(ex.Message);
        }
    }
}
