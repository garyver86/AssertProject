namespace Assert.API
{
    public class RequestInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            var userAgent = context.Request.Headers["User-Agent"].ToString();
            var isMobile = IsMobileRequest(userAgent).ToString();

            context.Items["ClientIP"] = ipAddress;
            context.Items["UserAgent"] = userAgent;
            context.Items["IsMobile"] = isMobile;

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
