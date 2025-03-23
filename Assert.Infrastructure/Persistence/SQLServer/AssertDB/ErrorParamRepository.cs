using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ErrorParamRepository : ITErrorParamRepository
    {
        private readonly InfraAssertDbContext _context;
        public ErrorParamRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }

        public TErrorParam GetErrorByCode(string code)
        {
            return _context.TErrorParams.FirstOrDefault(x => x.Code == code);
        }

        public TErrorParam GetDefaultError()
        {
            return _context.TErrorParams.FirstOrDefault(x => x.Code == ConstantsHelp.DEFAULT);
        }

        public void LogException(TExceptionLog exceptionLog)
        {
            _context.TExceptionLogs.Add(exceptionLog);
            _context.SaveChanges();
        }
    }
}
