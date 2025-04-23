using Assert.API.Helpers;
using Assert.API.Models;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Common;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Assert.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SecurityController : Controller
    {
        private readonly ISecurityService _securityService;
        private readonly JwtConfiguration _JWTConfiguration;
        private static IConfiguration _configuration;
        public SecurityController(ISecurityService securityService, IOptions<JwtConfiguration> JWTConfiguration)
        {
            _securityService = securityService;
            _JWTConfiguration = JWTConfiguration.Value;
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }


        [HttpPost("Login")]
        [EnableCors("AllowedOriginsPolicy")]
        public async Task<ReturnModelDTO> Login([FromBody] LoginModel user)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            //{ "IpAddress", context.GetClientIP() },
            //{ "BrowserInfo", context.GetUserAgent() },
            //{ "IsMobile", context.ClientIsMobile() },
            //{ "UserId", context.GetUserId() }
            if (!string.IsNullOrEmpty(user.User) && !string.IsNullOrEmpty(user.Password))
            {
                var result = await _securityService.UserLogin(user.User, user.Password, 
                    requestInfo["IpAddress"], requestInfo["BrowserInfo"], 
                    _JWTConfiguration.Secret, _JWTConfiguration.Issuer);
                return result;
            }
            else
            {
                return new ReturnModelDTO
                {
                    StatusCode = StatusCodes.Status401Unauthorized.ToString(),
                    ResultError = new ErrorCommonDTO { Message = "Unauthorized" }
                };
            }
        }
    }
}
