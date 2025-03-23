using Assert.API;
using Assert.Application;
using Assert.Application.DTOs;
using Assert.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

var allowedOriginsSection = builder.Configuration.GetSection("SystemConfiguration:AllowedOrigins");
var allowedOrigins = allowedOriginsSection.Get<string[]>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityDefinition("Header", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter your username",
        Name = "user",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "header"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        },
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Header"
                }
            },
            Array.Empty<string>()
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


builder.Services.AddHttpClient();
builder.Services.AddHttpClient("WS_atc");

builder.Services.ApiInjectionDependences(builder.Configuration);

builder.Services.AddCors(options =>
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

builder.Services.AddScoped(provider =>
{
    var logger = provider.GetRequiredService<ILogger<RestrictAllowOriginFilter>>();
    return new RestrictAllowOriginFilter(allowedOrigins, logger);
});




builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder?.Configuration["JWT:Secret"] ?? "")),
        ValidateIssuer = true,
        ValidateAudience = false,
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
            ServiceProvider? serviceProvider = builder?.Services?.BuildServiceProvider();
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

//builder.Services.AddAuthorization();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GuestOnly", policy => policy.RequireRole("Guest", "Application"));
    options.AddPolicy("HostOnly", policy => policy.RequireRole("Host", "Application"));
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin", "Application"));
    options.AddPolicy("GuestOrHostOrAdmin", policy => policy.RequireRole("Host", "Admin", "Guest", "Application"));
    options.AddPolicy("GuestOrHost", policy => policy.RequireRole("Host", "Guest"));
});
builder.Services.Configure<JWTConfiguration>(builder.Configuration.GetSection("JWT"));


var app = builder.Build();

app.UseMiddleware<RequestInfoMiddleware>();
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
//app.UseSwaggerUI();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Assert.API"); // Reemplaza "Tu API v1" con el nombre de tu API
});
//}

app.UseCors("AllowedOriginsPolicy");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
