using Assert.Application.DTOs.Responses;
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

        string errorMessage = ProcessMessage(exception);
        if (exception is ValidationException validationEx)
            errorMessage = ProcessValidationsErrors(validationEx);

        var response = new ReturnModelDTO
        {
            StatusCode = statusCode.ToString(),
            Data = null,
            HasError = true,
            ResultError = new ErrorCommonDTO
            {
                Title = statusCode == 200 ? "No existe el elemento" : "Error en la solicitud",
                Code = statusCode.ToString(),
                Message = errorMessage,
                CorrelationId = correlationId
            }
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsJsonAsync(response);
    }

    private static string ProcessValidationsErrors(ValidationException ex)
    {
        var validationErrors = string.Join(" | ",
                ex.Errors.SelectMany(e =>
                    e.Value.Select(msg => $"{e.Key}: {msg}"))
            );
        return $"Errores de validación: {validationErrors}";
    }

    private static int GetStatusCode(Exception ex) => ex switch
    {
        //externals
        HttpRequestException => StatusCodes.Status503ServiceUnavailable,
        TimeoutException => StatusCodes.Status504GatewayTimeout,

        //users
        NotFoundException => StatusCodes.Status200OK,
        UnauthorizedException => StatusCodes.Status401Unauthorized,
        InvalidTokenException => StatusCodes.Status401Unauthorized,

        //validations
        ValidationException => StatusCodes.Status400BadRequest,

        //dataBase
        DatabaseUnavailableException => StatusCodes.Status503ServiceUnavailable,

        //layers & bussinesExceptions
        DomainException => StatusCodes.Status422UnprocessableEntity,
        ApplicationException => StatusCodes.Status400BadRequest,
        InfrastructureException => StatusCodes.Status500InternalServerError,

        _ => StatusCodes.Status500InternalServerError
    };

    private string ProcessMessage(Exception exception) => exception switch
    {
        ApplicationException or DomainException or InfrastructureException
        or DatabaseUnavailableException or NotFoundException or UnauthorizedException
        or InvalidTokenException or ValidationException => exception.Message,
        _ => _env.IsDevelopment()
            ? $"{exception.Message} | StackTrace: {exception.StackTrace} | InnerException: {exception.InnerException?.Message}"
            : exception.Message
    };

}
