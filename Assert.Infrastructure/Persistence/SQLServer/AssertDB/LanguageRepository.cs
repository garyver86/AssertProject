using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Assert.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB;

public class LanguageRepository(
    IExceptionLoggerService _exceptionLoggerService,
    InfraAssertDbContext _dbContext)
    : ILanguageRepository
{
    public async Task<List<TLanguage>> GetAsync()
    {
        try
        {
            return (await _dbContext.TLanguages.ToListAsync());
        }
        catch(Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, "Language");

            throw new InfrastructureException(ex.Message);
        }
    }
}
