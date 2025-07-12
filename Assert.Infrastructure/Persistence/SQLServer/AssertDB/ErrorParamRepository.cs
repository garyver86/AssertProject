using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ErrorParamRepository : ITErrorParamRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly DbContextOptions<InfraAssertDbContext> dbOptions;
        public ErrorParamRepository(InfraAssertDbContext infraAssertDbContext,
            IServiceProvider serviceProvider)
        {
            _context = infraAssertDbContext;
            dbOptions = serviceProvider.GetRequiredService<DbContextOptions<InfraAssertDbContext>>();
        }

        public TErrorParam GetErrorByCode(string code)
        {
            using (var dbContedt = new InfraAssertDbContext(dbOptions))
            {
                return dbContedt.TErrorParams.FirstOrDefault(x => x.Code == code);
            }
        }

        public TErrorParam GetDefaultError()
        {
            using (var dbContedt = new InfraAssertDbContext(dbOptions))
            {
                return dbContedt.TErrorParams.FirstOrDefault(x => x.Code == ConstantsHelp.DEFAULT);
            }
        }

        public void LogException(TExceptionLog exceptionLog)
        {
            using (var dbContedt = new InfraAssertDbContext(dbOptions))
            {
                dbContedt.TExceptionLogs.Add(exceptionLog);
                dbContedt.SaveChanges();
            }
        }
    }
}
