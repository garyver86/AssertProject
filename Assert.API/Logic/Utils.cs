
using Microsoft.Extensions.Primitives;

namespace Assert.API.Logic
{
    public class Utils
    {
        public const int pageNumber = 1;
        public const int pageSize = 50;

        public static string GetClientIpAddress(HttpContext context)
        {
            string clientIp = GetRemoteIpAddress(context) ?? GetForwardedIpAddress(context);

            return clientIp;
        }

        private static string GetRemoteIpAddress(HttpContext context)
        {
            string remoteIp = context.Connection.RemoteIpAddress?.ToString();
            return remoteIp;
        }

        private static string GetForwardedIpAddress(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out StringValues headerValues))
            {
                string forwardedFor = headerValues.FirstOrDefault();

                if (!string.IsNullOrEmpty(forwardedFor))
                {
                    string[] ips = forwardedFor.Split(',');
                    string clientIp = ips.FirstOrDefault()?.Trim();

                    return clientIp;
                }
            }

            return null;
        }
    }
}
