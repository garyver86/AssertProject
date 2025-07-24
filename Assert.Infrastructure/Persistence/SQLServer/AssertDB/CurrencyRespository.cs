using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Assert.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB;

public class CurrencyRespository(
    InfraAssertDbContext _context,
    IExceptionLoggerService _exceptionLoggerService,
    ILogger<ListingRentRepository> _logger) 
    : ICurrencyRespository
{
    public async Task<List<TCurrency>> GetAllAsync()
    {
        try
        {
            var currencies = await _context.TCurrencies
                .ToListAsync();

            if (currencies is not { Count: > 0 })
            {
                _logger.LogError("There are not available currency list");
                throw new NotFoundException("No se encontraron registros de monedas disponibles");
            }

            return currencies;
        }
        catch(Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className,"T_Currency");

            throw new DatabaseUnavailableException(ex.Message);
        }
    }

}
