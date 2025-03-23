using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Assert.API
{
    public class RestrictAllowOriginFilter : ActionFilterAttribute
    {
        private readonly IEnumerable<string> _allowedOrigins;
        private readonly ILogger<RestrictAllowOriginFilter> _logger;

        public RestrictAllowOriginFilter(IEnumerable<string> allowedOrigins, ILogger<RestrictAllowOriginFilter> logger)
        {
            _allowedOrigins = allowedOrigins;
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var requestOrigin = context.HttpContext.Request?.Headers?.Origin.FirstOrDefault();
            var remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;

            if (_allowedOrigins.Contains("*"))
            {
                base.OnActionExecuting(context);
                return;
            }

            if (requestOrigin == null)
            {
                if (remoteIpAddress == null || !_allowedOrigins.Contains(remoteIpAddress.ToString()))
                {
                    _logger.LogWarning($"Access attempt from unauthorized IP: {remoteIpAddress}");
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
            else
            {
                if (!_allowedOrigins.Contains(requestOrigin))
                {
                    _logger.LogWarning($"Access attempt from unauthorized origin: {requestOrigin}");
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
