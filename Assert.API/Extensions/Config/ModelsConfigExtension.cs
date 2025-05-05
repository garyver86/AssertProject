using Assert.Domain.Common.Metadata;

namespace Assert.API.Extensions.Config;

public static class ModelsConfigExtension
{
    public static IServiceCollection AddModelsConfigExtension(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddScoped<RequestMetadata>();

        var googleAuthConfig = config.GetSection("GoogleAuth").Get<GoogleAuthConfig>() 
            ?? new GoogleAuthConfig();

        services.AddSingleton(googleAuthConfig!);

        return services;
    }
}
