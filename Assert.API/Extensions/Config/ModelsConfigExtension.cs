
using Assert.Domain.Common;

namespace Assert.API.Extensions.Config;

public static class ModelsConfigExtension
{
    public static IServiceCollection AddModelsConfigExtension(this IServiceCollection services)
    {
        services.AddScoped<RequestMetadata>();

        return services;
    }
}
