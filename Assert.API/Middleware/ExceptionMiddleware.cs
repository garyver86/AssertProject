using Assert.Application.DTOs;
using Assert.Application.Exceptions;
using Assert.Domain.Exceptions;
using Assert.Infrastructure.Exceptions;
using ApplicationException = Assert.Application.Exceptions.ApplicationException;

namespace Assert.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        var correlationId = context.TraceIdentifier;

        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = correlationId
        }))
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error no controlado: {ex.Message} - CorrelationId: {correlationId}");
                await HandleExceptionAsync(context, ex, correlationId);
            }
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception, string correlationId)
    {
        var statusCode = GetStatusCode(exception);

        var response = new ReturnModelDTO
        {
            StatusCode = statusCode.ToString(),
            Data = null,
            HasError = true,
            ResultError = new ErrorCommonDTO
            {
                Title = "Error en la solicitud",
                Code = statusCode.ToString(),
                Message = ProcessMessage(exception),
                CorrelationId = correlationId
            }
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsJsonAsync(response);
    }

    private static int GetStatusCode(Exception ex) => ex switch
        {
            //externals
            HttpRequestException => StatusCodes.Status503ServiceUnavailable,
            TimeoutException => StatusCodes.Status504GatewayTimeout,
            
            //users
            NotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedException => StatusCodes.Status401Unauthorized,

            //layers & bussinesExceptions
            InvalidTokenException => StatusCodes.Status401Unauthorized,
            ApplicationException => StatusCodes.Status400BadRequest,
            DomainException => StatusCodes.Status422UnprocessableEntity,
            InfrastructureException => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

    private string ProcessMessage(Exception exception) => exception switch
    {
        ApplicationException or DomainException or InfrastructureException => exception.Message,
        _ => _env.IsDevelopment()
            ? $"{exception.Message} | StackTrace: {exception.StackTrace} | InnerException: {exception.InnerException?.Message}"
            : exception.Message
    };

}
