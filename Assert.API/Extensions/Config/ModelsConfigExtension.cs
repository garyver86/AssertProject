﻿using Assert.Domain.Common.Metadata;

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

        var metaAuthConfig = config.GetSection("MetaAuth").Get<MetaAuthConfig>()
            ?? new MetaAuthConfig();
        services.AddSingleton(metaAuthConfig!);

        return services;
    }
}
