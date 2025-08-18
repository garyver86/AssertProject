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
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using SocketIO.Core;
using System.Diagnostics;

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

builder.Logging.ClearProviders()
               .AddConsole(options => options.IncludeScopes = true)
               .AddDebug();

builder.Services.AddQuequeExtensions();

builder.Services.AddModelsConfigExtension(builder.Configuration);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(Int32.Parse(Environment.GetEnvironmentVariable("PORT") ?? "8081"));

    // Puerto para WebSockets (Socket.IO)
    serverOptions.ListenAnyIP(3001, listenOptions =>
    {
        listenOptions.UseConnectionLogging();
        listenOptions.UseHttps(); // Opcional si necesitas WSS
    });
});

// Configuración CORS para Socket.IO
builder.Services.AddCors(options =>
{
    options.AddPolicy("SocketIoPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:19006", "http://yourapp.com")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

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

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Assert.API");
});

app.UseWebSockets(); // <-- Esto es crítico para Socket.IO

app.UseHttpsRedirection();
app.UseMiddleware<RequestInfoMiddleware>();
app.UseRouting();
app.UseCors("AllowedOriginsPolicy");
app.UseCors("SocketIoPolicy"); // <-- CORS específico para Socket.IO
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.MapGet("/socketio-health", (ISocketIoServer socketIo) =>
{
    return Results.Ok(new
    {
        status = "healthy",
        uptime = DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime(),
        connections = socketIo.GetActiveConnectionsCount(),
        lastActivity = socketIo.GetLastActivityTime(),
        memoryUsage = Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024 + " MB"
    });
});

// Configuración Socket.IO
var socketIoServer = app.Services.GetRequiredService<ISocketIoServer>();
socketIoServer.OnConnection(socket =>
{
    socket.On("authenticate", async (data) =>
    {
        var userId = data?.ToString();
        if (!string.IsNullOrEmpty(userId))
        {
            var connectionManager = app.Services.GetRequiredService<IConnectionManager>();
            connectionManager.AddConnection(userId, socket.Id);
            socket.Join(userId);

            socket.OnDisconnect(() =>
            {
                connectionManager.RemoveConnection(userId, socket.Id);
            });
        }
    });
});

// Inicia Socket.IO en segundo plano
_ = Task.Run(async () =>
{
    try
    {
        await socketIoServer.StartAsync();
    }
    catch (Exception ex)
    {
    }
});

app.Run();