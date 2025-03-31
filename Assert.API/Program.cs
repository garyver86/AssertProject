using Assert.API.Extensions.Cors;
using Assert.API.Extensions.Jwt;
using Assert.API.Middleware;
using Assert.Application;
using Assert.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

var allowedOriginsSection = builder.Configuration.GetSection("SystemConfiguration:AllowedOrigins");
var allowedOrigins = allowedOriginsSection.Get<string[]>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplicationInjections(builder.Configuration);
builder.Services.AddInfrastructureInjections(builder.Configuration);

builder.Services.AddFeaturesCors(builder.Configuration, allowedOrigins!);

builder.Services.AddAuthJwt(builder.Configuration);

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
