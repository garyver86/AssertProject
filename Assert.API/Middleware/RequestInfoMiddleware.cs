using Assert.Domain.Common.Metadata;
using System.Security.Claims;

namespace Assert.API.Middleware
{
    public class RequestInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, RequestMetadata metadata)
        {
            #region ya estaria demas - mantiene para no afectar algun otro endpoint
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            var userAgent = context.Request.Headers["User-Agent"].ToString();
            var isMobile = IsMobileRequest(userAgent).ToString();

            context.Items["ClientIP"] = ipAddress;
            context.Items["UserAgent"] = userAgent;
            context.Items["IsMobile"] = isMobile;
            #endregion

            #region use request metadata
            metadata.IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            metadata.UserAgent = context.Request.Headers["User-Agent"].ToString();
            metadata.IsMobile = IsMobileRequest(metadata.UserAgent).ToString();
            metadata.User = context.User?.Identity?.Name ?? "Anonymous";
            metadata.CorrelationId = context.TraceIdentifier;
            metadata.UserId = int.TryParse(context.User.FindFirst("identifier")?.Value, out var uid) ? uid : 0;
            metadata.UserName = context.User.FindFirst("username")?.Value ?? "Unknown";

            #endregion

            await _next(context);
        }

        private bool IsMobileRequest(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
            {
                return false;
            }

            userAgent = userAgent.ToLower();

            return userAgent.Contains("mobile") ||
                   userAgent.Contains("android") ||
                   userAgent.Contains("iphone") ||
                   userAgent.Contains("ipad") ||
                   userAgent.Contains("windows phone");
        }

    }
}
