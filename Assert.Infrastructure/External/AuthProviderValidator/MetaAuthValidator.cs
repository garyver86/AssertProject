using Assert.Domain.Common.Metadata;
using Assert.Domain.Interfaces.Infraestructure.External;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Models;
using Assert.Infrastructure.Exceptions;
using Assert.Shared.Extensions;
using Facebook;
using Microsoft.Extensions.Logging;

namespace Assert.Infrastructure.External.AuthProviderValidator;

public class MetaAuthValidator(MetaAuthConfig _metaConfig,
    RequestMetadata _metadata,
    IExceptionLoggerService _exceptionLoggerService,
    ILogger<GoogleAuthValidator> _logger)
    : IAuthProviderValidator
{
    public Task<ReturnModel> LoginAsync(string user, string password)
    {
        throw new NotImplementedException();
    }

    public async Task<ReturnModel> ValidateTokenAsync(string token)
    {
        try
        {
            _logger.LogInformation("Validando token de Facebook: {Token}", token);

            var client = new FacebookClient(token);

            dynamic userInfo = await client.GetTaskAsync("/me?fields=id,name,email");

            string userId = userInfo.id;
            string name = userInfo.name;
            string email = userInfo.email;

            var nameParts = ParseFullName(name); ;
            var firstName = nameParts.FirstName ?? "none";
            var lastName = nameParts.LastName ?? "none";

            var providerUser = new ProviderUser
            {
                Email = email ?? "",
                Name = firstName,
                LastName = lastName,
                UserId = userId
            };

            return new ReturnModel
            {
                StatusCode = ResultStatusCode.OK,
                HasError = false,
                Data = providerUser
            };
        }
        catch (FacebookApiException ex)
        {
            _logger.LogError(ex, $"FacebookApiException: Token inválido - CorrelationId: {_metadata.CorrelationId}");
            return new ReturnModel
            {
                StatusCode = ResultStatusCode.Unauthorized,
                HasError = true,
                ResultError = new ErrorCommon
                {
                    Title = "Token inválido",
                    Code = "401",
                    Message = "El token de autenticación de Facebook no es válido."
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error inesperado al validar token de Facebook - CorrelationId: {_metadata.CorrelationId}");
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, token);
            throw new InfrastructureException(ex.Message);
        }
    }

    #region private funcs
    private (string FirstName, string LastName) ParseFullName(string? name)
    {
        var nameParts = name?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? [];

        if (nameParts.Length >= 4)
        {
            var firstName = string.Join(" ", nameParts.Take(2));
            var lastName = string.Join(" ", nameParts.Skip(2));
            return (firstName, lastName);
        }
        else
        {
            var firstName = nameParts.FirstOrDefault() ?? "none";
            var lastName = string.Join(" ", nameParts.Skip(1)) ?? "none";
            return (firstName, lastName);
        }
    }
    #endregion
}
