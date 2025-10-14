using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Assert.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class BookStatusRepository(
        InfraAssertDbContext _dbContext,
        IExceptionLoggerService _exceptionLoggerService, IServiceProvider serviceProvider)
        : IBookStatusRepository
    {
        private readonly DbContextOptions<InfraAssertDbContext> dbOptions = serviceProvider.GetRequiredService<DbContextOptions<InfraAssertDbContext>>();


        public async Task<List<TbBookStatus>> Get()
        {
            using (var dbContext = new InfraAssertDbContext(dbOptions))
            {
                var statuses = await dbContext.TbBookStatuses
                    .ToListAsync();

                return statuses;
            }
        }

        public async Task<TbBookStatus> GetStatusByCode(string statusCode)
        {
            using (var dbContext = new InfraAssertDbContext(dbOptions))
            {
                var statuses = await dbContext.TbBookStatuses
                    .FirstOrDefaultAsync(x=>x.Code == statusCode);

                if(statuses is null)
                    throw new NotFoundException($"No existe el estado de reserva con codigo: {statusCode}");

                return statuses;
            }
        }
    }
}
