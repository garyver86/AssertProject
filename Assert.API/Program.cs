using Assert.API.Extensions.Config;
using Assert.API.Extensions.Cors;
using Assert.API.Extensions.Http;
using Assert.API.Extensions.Jwt;
using Assert.API.Extensions.Queque;
using Assert.API.Extensions.Swagger;
using Assert.API.Filters;
using Assert.API.Middleware;
using Assert.Application;
using Assert.Domain.Implementation;
using Assert.Domain.Services;
using Assert.Infrastructure;
using Assert.Infrastructure.Common;
using Assert.Infrastructure.InternalServices;
// Añade estos using
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

var allowedOriginsSection = builder.Configuration.GetSection("SystemConfiguration:AllowedOrigins");
var allowedOrigins = allowedOriginsSection.Get<string[]>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
}).ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddInfrastructureInjections(builder.Configuration);
builder.Services.AddApplicationInjections(builder.Configuration);
builder.Services.AddFeaturesCors(builder.Configuration, allowedOrigins!);

builder.Services.AddAuthJwt(builder.Configuration);
builder.Services.AddHttpExtension();
builder.Services.AddSwagger();
builder.Services.AddSignalRServices();

builder.Logging.ClearProviders()
               .AddConsole(options => options.IncludeScopes = true)
               .AddDebug();

builder.Services.AddQuequeExtensions();

builder.Services.AddModelsConfigExtension(builder.Configuration);


var app = builder.Build();

app.UseHttpsRedirection();
app.UseMiddleware<RequestInfoMiddleware>();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Resources", "Listings")),
    RequestPath = "/resources/listings"
});

// Expone los iconos de los tipos de propiedades
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Resources", "Icons")),
    RequestPath = "/resources/icons"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath, "Assert", "Resources", "Listings")),
    RequestPath = "/Resources/Listings"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath, "Assert", "Resources", "ProfilePhotos")),
    RequestPath = "/Resources/ProfilePhotos"
});


app.UseRouting();
app.UseCors("AllowedOriginsPolicy");
// Authentication y Authorization
app.UseAuthentication(); // ← ¡ESTO FALTABA! Es crucial para SignalR con [Authorize]
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Assert.API");
});

app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

// MAPEO DEL HUB DE SIGNALR - Esto debe ir al final
app.MapHub<NotificationHub>("/notifications-hub")
   .RequireCors("SignalRCors"); ;

app.Run();