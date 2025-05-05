using Assert.Application.DTOs.Responses;
using Assert.Application.Exceptions;
using Assert.Domain.Common.Metadata;
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
        RequestMetadata _metadata,IAccountRepository _accountRepository,
        IUserRolRepository _userRolRespository,
        IUserService _userService,
        Func<Platform, IAuthProviderValidator> _authValidatorFactory,
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
            int userId;
            int accountId;
            int userRolId;

            var providerUser = (ProviderUser)authenticationResult.Data!;

            List<string> userRoles = new();
            switch (userAccount.StatusCode)
            {
                case "404": //notFound user
                    userId = await _userRepository.Create(userName, enumPlatform, providerUser.Name, 
                        providerUser.LastName, 0, null, "", 1, "", null);
                    accountId = await _accountRepository.Create(userId, password);
                    userRolId = await _userRolRespository.CreateGuest(userId);
                    userRoles.Add("Guest");
                    break;

                case "204": //notFound account
                    userId = Convert.ToInt32(userAccount.Data);
                    accountId = await _accountRepository.Create(userId, password);
                    userRolId = await _userRolRespository.CreateGuest(userId);
                    userRoles.Add("Guest");
                    break;

                default:
                    var user = (TuUser)userAccount.Data!;
                    userId = user.UserId;
                    accountId = Convert.ToInt32(user.TuAccounts.First().AccountId);
                    if (enumPlatform == Platform.Local)
                        foreach (var role in authenticationResult.ResultError?.Code.Split(','))
                            userRoles.Add(role);
                    else
                        userRoles = await _userRolRespository.GetUserRoles(userId);
                    break;
            }
            _metadata.UserId = userId;
            _metadata.UserName = userName;
            _metadata.AccountId = accountId;
            #endregion

            #region claims
            List<Claim> claims = new()
            {
                new("identifier", _metadata.UserId.ToString()),
                new("value", _metadata.UserId.ToString())
            };

            foreach (var role in userRoles)
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
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
        {
            if (enumPlatform == Platform.Local)
                throw new UnauthorizedAccessException(authenticationResult.ResultError.Message);
            else throw new InvalidTokenException(authenticationResult.ResultError.Message);
        }
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
