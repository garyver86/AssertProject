using Assert.Domain.Common.Metadata;
using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Assert.Infrastructure.Utils;
using Assert.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB;

public class AccountRepository
    (IExceptionLoggerService _exceptionLoggerService, InfraAssertDbContext _dbContext,
    RequestMetadata _metadata)
    : IAccountRepository
{
    public async Task<string> ChangePassword(string newPassword)
    {
        try
        {
            var userTypeLocal = await _dbContext.TuPlatforms
            .FirstOrDefaultAsync(x => x.Code!.ToLower() == "local");

            if (userTypeLocal is null) throw new NotFoundException("No existe tipo de cuenta LOCAL");

            var currentUser = await _dbContext.TuUsers
                .FirstOrDefaultAsync(x => x.UserId == _metadata.UserId);

            if (currentUser is null) throw new NotFoundException("No existe usuario con sesion iniciada");

            if (currentUser.PlatformId != userTypeLocal.PlatformId)
                throw new UnauthorizedAccessException("El usuario no puede modificar su contrasena. Asocio cuenta desde una plataforma externa.");

            string pass = UtilsMgr.GetHash512(newPassword);

            var currentAccount = await _dbContext.TuAccounts
                .FirstOrDefaultAsync(x => x.UserId == _metadata.UserId);

            if (currentAccount is null) throw new NotFoundException("No existe cuenta para el usuario con sesion iniciada");

            currentAccount.Password = pass;

            await _dbContext.SaveChangesAsync();

            return "UPDATED";
        }
        catch (Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, new { _metadata.UserId });
            throw new InfrastructureException(ex.Message);
        }
    }

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
