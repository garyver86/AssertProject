using Assert.Domain.Common.Metadata;
using Assert.Domain.Interfaces.Infraestructure.External;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Models;
using Assert.Domain.Models.Auth;
using Assert.Infrastructure.Exceptions;
using Assert.Shared.Extensions;
using Google.Apis.Auth;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Infrastructure.External.AuthProviderValidator;

public class GoogleAuthValidator(GoogleAuthConfig _googleConfig,
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
            _logger.LogInformation("Token recibido para validar: {Token}", token);
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = _googleConfig.ClientIds
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);

            if(payload != null && !string.IsNullOrEmpty(payload.Subject))
            {
                var googleUser = new ProviderUser { Email = payload.Email ?? "",
                    Name = payload.GivenName ?? "none", LastName = payload.FamilyName ?? "none",
                    UserId = payload.Subject ?? "" };
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = googleUser
                };
            }
            else
            {
                _logger.LogError($"Validation error with the google token: {token} - CorrelationId: {_metadata.CorrelationId}");
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.Unauthorized,
                    HasError = true,
                    ResultError = new ErrorCommon { Title="Token invalido", Code="401", Message="Token de autenticacion de google invalido." }
                };
            }            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception while validating google token: {token} - CorrelationId: {_metadata.CorrelationId}");
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, token);
            throw new InfrastructureException(ex.Message);
        }
    }
}
