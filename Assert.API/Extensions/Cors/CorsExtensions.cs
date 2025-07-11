namespace Assert.API.Extensions.Cors;

public static class CorsExtensions
{
    public static IServiceCollection AddFeaturesCors(this IServiceCollection services,
        IConfiguration configuration, string[] allowedOrigins)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowedOriginsPolicy", builder =>
            {
                if (allowedOrigins?.Contains("*") ?? false)
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                }
                else
                {
                    // Asegúrate que los orígenes no sean null
                    var origins = allowedOrigins ?? Array.Empty<string>();
                    builder.WithOrigins(origins)
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .SetIsOriginAllowedToAllowWildcardSubdomains();
                }

                // Si necesitas credenciales (cookies, tokens)
                // .AllowCredentials();
            });
        });

        if (configuration.GetValue<bool>("EnableOriginFilter", false))
        {
            services.AddScoped<RestrictAllowOriginFilter>();
        }

        return services;
    }
}
