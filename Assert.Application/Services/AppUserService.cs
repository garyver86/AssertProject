using Assert.Application.DTOs.Responses;
using Assert.Application.Exceptions;
using Assert.Domain.Common;
using Assert.Domain.Entities;
using Assert.Domain.Enums;
using Assert.Domain.Interfaces.Infraestructure.External;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Infrastructure.Security;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApplicationException = Assert.Application.Exceptions.ApplicationException;

namespace Assert.Application.Services;

public class AppUserService(
        IJWTSecurity _jwtSecurity, IMapper _mapper, IUserRepository _userRepository,
        RequestMetadata _metadata, IAccountRepository _accountRepository,
        Func<Platform, IAuthProviderValidator> _authValidatorFactory, IUserService _userService,
        IErrorHandler _errorHandler) : IAppUserService
{
    public async Task<ReturnModelDTO> LoginAndEnrollment(string platform, string token,
        string userName, string password)
    {
        #region platform context: google - apple - meta - local
        var enumPlatform = _mapper.Map<Platform>(platform);

        if (enumPlatform == Platform.None)
            throw new ApplicationException("Proveedor Invalido");
        #endregion

        #region authenticator: local - socialMedia
        var authContext = _authValidatorFactory(enumPlatform);
        ReturnModel? authenticationResult = null;

        if (enumPlatform == Platform.Local)
            authenticationResult = await authContext.LoginAsync(userName, password);
        else
        {
            authenticationResult = await authContext.ValidateTokenAsync(token);

            if (authenticationResult.StatusCode != ResultStatusCode.OK)
                throw new InvalidTokenException($"Inicio de sesion invalido en plataforma: {enumPlatform.ToString().ToUpper()}. El token proporcionado no es válido.");
        }
        #endregion

        if (authenticationResult.StatusCode == ResultStatusCode.OK)
        {
            #region user & account validation
            var userAccount = await _userRepository.ValidateUserName(userName, true);
            switch (userAccount.StatusCode)
            {
                case "NotFound":
                    var newUserId = await _userRepository.Create(userName, enumPlatform, "", "", 0, null, "", 1, "", null);
                    var newAccountId = await _accountRepository.Create(newUserId, password);
                    _metadata.UserId = newUserId;
                    _metadata.UserName = userName;
                    _metadata.AccountId = newAccountId;
                    break;
                case "NoContent":
                    var userId = Convert.ToInt32(userAccount.Data);
                    newAccountId = await _accountRepository.Create(userId, password);
                    _metadata.UserId = userId;
                    _metadata.UserName = userName;
                    _metadata.AccountId = newAccountId;
                    break;
                default:
                    _metadata.UserName = userName;
                    _metadata.UserId = ((TuUser)userAccount.Data!).UserId;
                    _metadata.AccountId = Convert.ToInt32(((TuUser)userAccount.Data!).TuAccounts.First().AccountId);
                    break;
            }
            #endregion

            #region claims
            string value = authenticationResult.ResultError?.Message;
            List<Claim> claims = new()
            {
                new("identifier", (string)authenticationResult.Data!),
                new("value", value),
                new Claim(JwtRegisteredClaimNames.Sub, userName)
            };

            foreach (var role in authenticationResult.ResultError?.Code.Split(','))
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }
            #endregion

            #region jwt generator
            string jwtToken = await _jwtSecurity.GenerateJwt(claims);
            #endregion

            #region update session info
            await _accountRepository.UpdateLastSessionInfo();
            #endregion

            var result = new ReturnModelDTO
            {
                StatusCode = ResultStatusCode.OK,
                Data = new { jwtToken }
            };

            return result;
        }
        else
            throw new UnauthorizedAccessException(authenticationResult.ResultError.Message);
    }

    public async Task<ReturnModelDTO> DisableHostRole(long userId, Dictionary<string, string> clientData, bool useTechnicalMessages)
    {
        try
        {
            var listings = await _userService.DisableHostRole(userId);
            var dataResult = _mapper.Map<ReturnModelDTO>(listings);

            return dataResult;
        }
        catch (Exception ex)
        {
            return HandleException<ReturnModelDTO>("AppUserService.DisableHostRole", ex, new { userId }, useTechnicalMessages);
        }
    }

    public async Task<ReturnModelDTO> EnableHostRole(long userId, Dictionary<string, string> clientData, bool useTechnicalMessages)
    {
        try
        {
            var listings = await _userService.EnableHostRole(userId);
            var dataResult = _mapper.Map<ReturnModelDTO>(listings);

            return dataResult;
        }
        catch (Exception ex)
        {
            return HandleException<ReturnModelDTO>("AppUserService.EnableHostRole", ex, new { userId }, useTechnicalMessages);
        }
    }

    private ReturnModelDTO<T> HandleException<T>(string action, Exception ex, object data, bool useTechnicalMessages)
    {
        var error = _errorHandler.GetErrorException(action, ex, data, useTechnicalMessages);
        return new ReturnModelDTO<T>
        {
            HasError = true,
            StatusCode = ResultStatusCode.InternalError,
            ResultError = _mapper.Map<ErrorCommonDTO>(error)
        };
    }
}
