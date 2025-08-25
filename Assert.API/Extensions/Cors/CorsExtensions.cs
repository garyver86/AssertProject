using Microsoft.Extensions.Configuration;

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

            // Política específica para SignalR
            options.AddPolicy("SignalRCors", builder =>
            {
                var allowedOrigins = configuration.GetSection("SignalR:AllowedOrigins").Get<string[]>()
                    ?? new[]
                    {
                        "http://localhost",
                        "https://localhost:44317",
                        "http://localhost:3000",
                        "https://localhost:3000",
                        "http://127.0.0.1:5500",
                        "http://localhost/SignalRClient"
                    };

                builder.WithOrigins(allowedOrigins)
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });
        });

        if (configuration.GetValue<bool>("EnableOriginFilter", false))
        {
            services.AddScoped<RestrictAllowOriginFilter>();
        }

        return services;
    }
}
