using Assert.API.Extensions.Config;
using Assert.API.Extensions.Cors;
using Assert.API.Extensions.Http;
using Assert.API.Extensions.Jwt;
using Assert.API.Extensions.Queque;
using Assert.API.Extensions.Swagger;
using Assert.API.Filters;
using Assert.API.Middleware;
using Assert.Application;
using Assert.Infrastructure;
using Microsoft.Extensions.FileProviders;

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

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Assert.API");
});

app.UseHttpsRedirection();
app.UseMiddleware<RequestInfoMiddleware>();
app.UseRouting();
app.UseCors("AllowedOriginsPolicy");
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run();