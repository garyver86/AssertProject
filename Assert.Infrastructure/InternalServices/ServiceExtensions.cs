using Assert.Domain.Implementation;
using Assert.Domain.Interfaces.Notifications;
using Assert.Domain.Services;
using Assert.Infrastructure.Common;
using Assert.Infrastructure.ExternalServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Infrastructure.InternalServices
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddSignalRServices(this IServiceCollection services)
        {
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                options.HandshakeTimeout = TimeSpan.FromSeconds(30);
                options.KeepAliveInterval = TimeSpan.FromSeconds(15);
            });

            services.AddScoped<IRealTimeMessageService, SignalRService>(); 
            services.AddScoped<IEventDispatcher, UserNotificationEventHandler>();

            return services;
        }
    }
}
