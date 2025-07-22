using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Assert.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class MethodOfPaymentRepository(IExceptionLoggerService _exceptionLoggerService,
    InfraAssertDbContext _dbContext) : IMethodOfPaymentRepository
    {
        public async Task<List<PayMethodOfPayment>> GetAllAsync(int countryId)
        {
            try
            {
                var methodsOfPayment = await _dbContext.PayCountryConfigurations
                    .Where(mp => mp.CountryId == countryId).Select(x => x.MethodOfPayment).ToListAsync();

                if (methodsOfPayment != null && methodsOfPayment.Any())
                {
                    methodsOfPayment = methodsOfPayment.Where(mp => mp.Active == true).ToList();
                }

                return methodsOfPayment;
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { countryId });

                throw new InfrastructureException(ex.Message);
            }
        }

        public async Task<PayMethodOfPayment> GetByIdAsync(int id)
        {
            try
            {
                var methodOfPayment = await _dbContext.PayMethodOfPayments
                    .FirstOrDefaultAsync(ec => ec.MethodOfPaymentId == id);

                if (methodOfPayment == null)
                    throw new NotFoundException($"No existe registros para el metodo de pago: {id}");

                return methodOfPayment;
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { id });

                throw new InfrastructureException(ex.Message);
            }
        }
    }
}
