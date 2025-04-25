using Assert.Application.Services.Logging;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Interfaces.Queque;
using Assert.Infrastructure.Logging;

namespace Assert.API.Extensions.Queque;

public static class QuequeExtensions
{
    public static IServiceCollection AddQuequeExtensions(this IServiceCollection services)
    {
        services.AddSingleton<IExceptionLogQueue, ExceptionLogQueue>();
        services.AddScoped<IExceptionLoggerService, ExceptionLoggerService>();
        services.AddHostedService<ExceptionLogBackgroundService>();

        return services;
    }
}
