using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Queque;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Persistence.SQLServer.AssertDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Assert.Infrastructure.Logging;

public class ExceptionLogBackgroundService(
    IServiceProvider serviceProvider,
    IExceptionLogQueue logQueue,
    ILogger<ExceptionLogBackgroundService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logQueue.TryDequeue(out var logModel))
            {
                try
                {
                    using var scope = serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<InfraAssertDbContext>();

                    var message = logModel.Exception.Message;

                    var serializedData = JsonConvert.SerializeObject(logModel.Data,
                            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    var entity = new TExceptionLog
                    {
                        Action = logModel.Action,
                        Module = logModel.Module,
                        Message = message,
                        BrowseInfo = logModel.BrowseInfo,
                        DateException = DateTime.UtcNow,
                        IpAddress = logModel.IpAddress,
                        UserId = logModel.UserId,
                        StackTrace = logModel.Exception.StackTrace,
                        DataRequest = serializedData
                    };

                    logger.LogInformation($"Log Data: {serializedData}");
                    context.TExceptionLogs.Add(entity);
                    await context.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error to save exception LOG");
                }
            }
            else
            {
                await Task.Delay(500, stoppingToken);
            }
        }
    }
}

