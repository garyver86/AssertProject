using Assert.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Assert.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Assert.Application;
using Assert.Application.Services.Logging;
using Assert.Domain.Interfaces.Logging;
using Assert.API.Extensions.Queque;
using Assert.API.Extensions.Config;
using Newtonsoft.Json.Linq;

namespace Assert.UnitTest.AppUserService;

public class GoogleLoginIntegrationTests
{
    private readonly IAppUserService _appUserService;
    private readonly ServiceProvider _serviceProvider;

    public GoogleLoginIntegrationTests()
    {
        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        services.AddLogging();

        services.AddSingleton<IConfiguration>(configuration);
        services.AddInfrastructureInjections(configuration);
        services.AddApplicationInjections(configuration);        

        services.AddQuequeExtensions();
        services.AddModelsConfigExtension(configuration);

        _serviceProvider = services.BuildServiceProvider();

        _appUserService = _serviceProvider.GetRequiredService<IAppUserService>();
    }

    [Fact]
    public async Task LoginAndEnrollment_WithValidGoogleToken_ShouldReturnSuccess()
    {
        //// Arrange
        //var validGoogleToken = "";
        //var platform = "google";        
        //string userName = "f.iriarte12@gmail.com";
        //string password = "";

        //// Act
        //var result = await _appUserService.LoginAndEnrollment(platform, validGoogleToken, userName, password);
                
        //// Assert
        //Xunit.Assert.NotNull(result);
        //Xunit.Assert.False(result.HasError);
        //Xunit.Assert.Equal("200", result.StatusCode);
        //Xunit.Assert.NotNull(result.Data);

        //Console.WriteLine($"Login exitoso con: {System.Text.Json.JsonSerializer.Serialize(result.Data)}");
    }
}
