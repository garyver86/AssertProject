using Assert.Domain.Common;
using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Interfaces.Queque;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
                    var context = scope.ServiceProvider.GetRequiredService<AssertDbContext>();

                    var message = logModel.Exception.Message;

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
                        DataRequest = JsonConvert.SerializeObject(logModel.Data,
                            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })
                    };

                    context.TExceptionLogs.Add(entity);
                    await context.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error al guardar log de excepción.");
                }
            }
            else
            {
                await Task.Delay(500, stoppingToken);
            }
        }
    }
}

