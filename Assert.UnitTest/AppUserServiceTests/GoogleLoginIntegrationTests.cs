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
        // Arrange
        //var validGoogleToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjA3YjgwYTM2NTQyODUyNWY4YmY3Y2QwODQ2ZDc0YThlZTRlZjM2MjUiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiIxMDkwMzk2NTQ1MjQyLTJiYW9oaTBxZnNsM21tNzhvZmhnMDhqcWltdWE3cnYwLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwiYXVkIjoiMTA5MDM5NjU0NTI0Mi0yYmFvaGkwcWZzbDNtbTc4b2ZoZzA4anFpbXVhN3J2MC5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsInN1YiI6IjExNjc3NTM0NjE3OTk5NDA1NjM5OSIsImVtYWlsIjoiZi5pcmlhcnRlMTJAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOnRydWUsIm5iZiI6MTc0NjQ3NDQzMiwibmFtZSI6IkZyYW56IElyaWFydGUgQXJpc3BlIiwicGljdHVyZSI6Imh0dHBzOi8vbGgzLmdvb2dsZXVzZXJjb250ZW50LmNvbS9hL0FDZzhvY0pJN3hGZ3hhdE1rclRRUzc1ZENDWFFJc2U4ckhHLWFOTzFXMHFPWlpyeTdBTnNRVm51PXM5Ni1jIiwiZ2l2ZW5fbmFtZSI6IkZyYW56IiwiZmFtaWx5X25hbWUiOiJJcmlhcnRlIEFyaXNwZSIsImlhdCI6MTc0NjQ3NDczMiwiZXhwIjoxNzQ2NDc4MzMyLCJqdGkiOiIxOWViY2IwMWMwYzQ2YjZiNzA0ZTQ1ZjNiY2Q3ZGI5ZGM5YjE1YWIxIn0";
        var validGoogleToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjA3YjgwYTM2NTQyODUyNWY4YmY3Y2QwODQ2ZDc0YThlZTRlZjM2MjUiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiIxMDkwMzk2NTQ1MjQyLTJiYW9oaTBxZnNsM21tNzhvZmhnMDhqcWltdWE3cnYwLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwiYXVkIjoiMTA5MDM5NjU0NTI0Mi0yYmFvaGkwcWZzbDNtbTc4b2ZoZzA4anFpbXVhN3J2MC5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsInN1YiI6IjExNjc3NTM0NjE3OTk5NDA1NjM5OSIsImVtYWlsIjoiZi5pcmlhcnRlMTJAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOnRydWUsIm5iZiI6MTc0NjQ4MTU4NiwibmFtZSI6IkZyYW56IElyaWFydGUgQXJpc3BlIiwicGljdHVyZSI6Imh0dHBzOi8vbGgzLmdvb2dsZXVzZXJjb250ZW50LmNvbS9hL0FDZzhvY0pJN3hGZ3hhdE1rclRRUzc1ZENDWFFJc2U4ckhHLWFOTzFXMHFPWlpyeTdBTnNRVm51PXM5Ni1jIiwiZ2l2ZW5fbmFtZSI6IkZyYW56IiwiZmFtaWx5X25hbWUiOiJJcmlhcnRlIEFyaXNwZSIsImlhdCI6MTc0NjQ4MTg4NiwiZXhwIjoxNzQ2NDg1NDg2LCJqdGkiOiIzMTE2ODljZThlNmU2MDJiNTVkZGUwYjNjYzMwODQ1NjM5MTVhODIxIn0.qOGY94BH091CdM_gOHJcfyPruY8k4dkDb0vk75JRVU64auEDfhP4mAVWvluPaMiyipqR7GfPXFu9UevFy8I-OEvPzGzhL8ysnM1x4GkdJV2oR_lIUIyVV3M-yX2UoWL1yoH3JLqmdPidnGCPiS1mbjP58y6ie4anCbxIOAKBaLpWt73cfziWZalGgHDTzfSyY7KsfuiuDYK7EsUiBXKY9TVwayYDrVhBxuLJ1aIhuHRES8WP_Gtim-bcVs85jhOd2ug0E1ULgC-GZNd4hCMU_upvv0rG9qvwJLS-FOO-CvW9Uz_CNwHLOzKKrorTJblm-tej4dOw5rKrW4_G24-B0g";
        var tokenParts = validGoogleToken.Split('.');
        var platform = "google";        
        string userName = "f.iriarte12@gmail.com";
        string password = "";

        // Act
        var result = await _appUserService.LoginAndEnrollment(platform, validGoogleToken, userName, password);
                
        // Assert
        Xunit.Assert.NotNull(result);
        Xunit.Assert.False(result.HasError);
        Xunit.Assert.Equal("200", result.StatusCode);
        Xunit.Assert.NotNull(result.Data);

        Console.WriteLine($"Login exitoso con: {System.Text.Json.JsonSerializer.Serialize(result.Data)}");
    }
}
