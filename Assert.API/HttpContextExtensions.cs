using System.IdentityModel.Tokens.Jwt;

namespace Assert.API.Helpers
{
    public static class HttpContextExtensions
    {
        public static string GetClientIP(this HttpContext context)
        {
            return context.Items["ClientIP"] as string;
        }
        public static string ClientIsMobile(this HttpContext context)
        {
            return context.Items["IsMobile"] as string;
        }

        public static string GetUserAgent(this HttpContext context)
        {
            return context.Items["UserAgent"] as string;
        }

        public static string GetUserId(this HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return null;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "value");
            return userIdClaim?.Value;
        }

        // Nuevo método para devolver un diccionario con los tres valores
        public static Dictionary<string, string> GetRequestInfo(this HttpContext context)
        {
            //userInfo["BrowserInfo"] 
            //userInfo["IsMobile"]
            //userInfo["IpAddress"]
            //userInfo["AdditionalData"]
            //userInfo["ApplicationCode"]
            return new Dictionary<string, string>
            {
                { "IpAddress", context.GetClientIP() },
                { "BrowserInfo", context.GetUserAgent() },
                { "IsMobile", context.ClientIsMobile() },
                { "UserId", context.GetUserId() },
                { "ApplicationCode", "ASSERT_WEB" }
            };
        }
    }
}
