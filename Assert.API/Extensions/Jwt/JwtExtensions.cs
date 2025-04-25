using Assert.Application.DTOs;
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
                }
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("GuestOnly", policy => policy.RequireRole("Guest", "Application"));
            options.AddPolicy("HostOnly", policy => policy.RequireRole("Host", "Application"));
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin", "Application"));
            options.AddPolicy("GuestOrHostOrAdmin", policy => policy.RequireRole("Host", "Admin", "Guest", "Application"));
            options.AddPolicy("GuestOrHost", policy => policy.RequireRole("Host", "Guest"));
        });

        services.Configure<JwtConfiguration>(configuration.GetSection("JWT"));

        return services;
    }
}
