using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Assert.API.Extensions.Http;

public static class HttpExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddHttpClient("WS_atc");

        return services;
    }
}
