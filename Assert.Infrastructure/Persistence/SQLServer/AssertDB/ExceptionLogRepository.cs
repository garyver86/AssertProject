using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Newtonsoft.Json;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ExceptionLogRepository : IExceptionLogRepository
    {
        private readonly InfraAssertDbContext _context;
        public ExceptionLogRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task SaveError(string message, string action, string module, object data, int? userId, string? ipAddress, string browseInfo)
        {
            TExceptionLog TExceptionLog = new TExceptionLog
            {
                Action = action ?? "",
                BrowseInfo = browseInfo,
                UserId = userId,
                DateException = DateTime.UtcNow,
                IpAddress = ipAddress,
                Module = module ?? "",
                StackTrace = "",
                Message = message,
                DataRequest = JsonConvert.SerializeObject(data, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }) ?? ""//, Formatting.Indented)
            };
            _context.TExceptionLogs.Add(TExceptionLog);
            await _context.SaveChangesAsync();
        }
        public async Task SaveException(Exception ex, string action, string module, object data, int? userId, string? ipAddress, string browseInfo)
        {
            string exceptionMessaje = await ProcessExceptionMessage(ex);

            TExceptionLog TExceptionLog = new TExceptionLog
            {
                Action = action ?? "",
                BrowseInfo = browseInfo,
                UserId = userId,
                DateException = DateTime.UtcNow,
                IpAddress = ipAddress,
                Module = module ?? "",
                StackTrace = ex.StackTrace,
                Message = exceptionMessaje,
                DataRequest = JsonConvert.SerializeObject(data, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }) ?? ""//, Formatting.Indented)
            };
            _context.TExceptionLogs.Add(TExceptionLog);
            await _context.SaveChangesAsync();
        }

        public async Task<string> ProcessExceptionMessage(Exception ex)
        {
            string message = ex.Message;
            try
            {
                //if (new DbEntityValidationException().GetType() == ex.GetType())
                //{
                //    json = JsonMGR.ConvertToJSON_WONull(((DbEntityValidationException)ex).EntityValidationErrors?.FirstOrDefault()?.ValidationErrors, 8000);
                //}
                //else 
                if (ex.InnerException != null)
                {
                    message = ex.InnerException.Message;
                    //if (new DbEntityValidationException().GetType() == ex.InnerException.GetType())
                    //{
                    //    json = JsonMGR.ConvertToJSON_WONull(((DbEntityValidationException)ex).EntityValidationErrors?.FirstOrDefault()?.ValidationErrors, 8000);
                    //}
                    //else 
                    if (ex.InnerException?.InnerException != null)
                    {
                        message = ex.InnerException.InnerException.Message;
                        //if (new DbEntityValidationException().GetType() == ex.InnerException.InnerException.GetType())
                        //{
                        //    json = JsonMGR.ConvertToJSON_WONull(((DbEntityValidationException)ex).EntityValidationErrors?.FirstOrDefault()?.ValidationErrors, 8000);
                        //}
                    }

                }
            }
            catch { }
            return await Task.FromResult(message);
        }
    }
}
