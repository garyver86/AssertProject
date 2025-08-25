using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Assert.API.Extensions.Jwt;

public static class JwtExtensions
{
    public static IServiceCollection AddAuthJwt(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? "")),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = async context =>
                {
                    // 1. PRIMERO: Manejar SignalR (WebSockets)
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    // Para SignalR - WebSockets usan query string
                    if (!string.IsNullOrEmpty(accessToken) &&
                        path.StartsWithSegments("/notifications-hub"))
                    {
                        context.Token = accessToken;
                        return; // Salir temprano para SignalR
                    }

                    string? authorizationHeader = await Task.FromResult(context.Request.Headers["Authorization"]);

                    if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                    {
                        string existingToken = authorizationHeader.Substring("Bearer ".Length).Trim();
                        string newToken = existingToken;

                        context.Request.Headers.Authorization = "Bearer " + newToken;
                    }
                },
                OnTokenValidated = async context =>
                {
                    // Solo validar para HTTP tradicional, no para SignalR
                    var path = context.HttpContext.Request.Path;
                    if (path.StartsWithSegments("/notifications-hub"))
                    {
                        // Para SignalR, skip la validación adicional si es necesario
                        // o implementa una validación más ligera
                        return;
                    }

                    ServiceProvider? serviceProvider = services?.BuildServiceProvider();
                    ISecurityService? tokenValidator = serviceProvider?.GetRequiredService<ISecurityService>();

                    var authorizationHeader = context.Request.Headers.Authorization.FirstOrDefault();

                    var user = context.Request.Headers["user"].FirstOrDefault();

                    var token = authorizationHeader?.Substring("Bearer ".Length).Trim();
                    ReturnModelDTO? resultValidation = await tokenValidator?.ValidateTokenUser(token, user);

                    if (resultValidation.StatusCode != StatusCodes.Status200OK.ToString())
                    {
                        context.Fail(resultValidation?.ResultError?.Message ?? "Token validation failed.");
                    }
                },

                // Nuevo evento para manejar autenticación fallida en SignalR
                OnAuthenticationFailed = context =>
                {
                    var path = context.HttpContext.Request.Path;
                    if (path.StartsWithSegments("/notifications-hub"))
                    {
                        Console.WriteLine($"SignalR auth failed: {context.Exception.Message}");
                        // No fallar inmediatamente para permitir reconexión
                        context.NoResult();
                        return Task.CompletedTask;
                    }

                    // Comportamiento normal para APIs HTTP
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("GuestOnly", policy => policy.RequireRole("Guest"));
            options.AddPolicy("Guest", policy => policy.RequireRole("Guest", "Application"));
            options.AddPolicy("HostOnly", policy => policy.RequireRole("Host", "Application"));
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin", "Application"));
            options.AddPolicy("GuestOrHostOrAdmin", policy => policy.RequireRole("Host", "Admin", "Guest", "Application"));
            options.AddPolicy("GuestOrHost", policy => policy.RequireRole("Host", "Guest"));

            // Política específica para SignalR si es necesario
            options.AddPolicy("SignalR", policy =>
                policy.RequireAuthenticatedUser());
        });

        services.Configure<JwtConfiguration>(configuration.GetSection("JWT"));

        return services;
    }

    // Método opcional para solo SignalR si necesitas configuraciones diferentes
    public static IServiceCollection AddAuthJwtForSignalR(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? "")),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };

                // Configuración minimalista para SignalR
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/notifications-hub"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }
}
