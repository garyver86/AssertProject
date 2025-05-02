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
                    builder.WithOrigins(allowedOrigins)
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                }
            });
            options.AddPolicy("DefaultPolicy", builder =>
            {
                builder.DisallowCredentials()
                       .WithMethods("GET", "POST", "PUT", "DELETE")
                       .AllowAnyHeader();
                //.SetIsOriginAllowed(origin => false);
            });
        });

        services.AddScoped(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<RestrictAllowOriginFilter>>();
            return new RestrictAllowOriginFilter(allowedOrigins, logger);
        });

        return services;
    }
}
