using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Exceptions;
using Assert.Domain.Common.Metadata;
using Assert.Domain.Entities;
using Assert.Domain.Enums;
using Assert.Domain.Interfaces.Infraestructure.External;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Domain.ValueObjects;
using Assert.Infrastructure.Security;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApplicationException = Assert.Application.Exceptions.ApplicationException;

namespace Assert.Application.Services;

public class AppUserService(
        IJWTSecurity _jwtSecurity, IMapper _mapper, IUserRepository _userRepository,
        RequestMetadata _metadata, IAccountRepository _accountRepository,
        IUserRolRepository _userRolRespository,
        Func<Platform, IAuthProviderValidator> _authValidatorFactory, IUserService _userService,
        IErrorHandler _errorHandler, ILogger<AppUserService> _logger)
        : IAppUserService
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
            var userAccount = await _userRepository.ValidateUserName(userName, true, enumPlatform);
            int userId;
            int accountId;
            int userRolId;

            var providerUser = (ProviderUser)authenticationResult.Data!;

            List<string> userRoles = new();
            switch (userAccount.StatusCode)
            {
                case "404": //notFound user
                    userId = await _userRepository.Create(userName, enumPlatform, providerUser.Name,
                        providerUser.LastName, 0, null, "", 1, "", null, "");
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
                new("value", _metadata.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim("platform", platform)
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

    public async Task<ReturnModelDTO> LocalUserEnrollment(LocalUserRequest userRequest)
    {
        var existUserValidation = await _userRepository.ExistLocalUser(userRequest.Email);

        if (existUserValidation.Data)
        {
            _logger.LogError($"Enrollment error, user already exist: {userRequest.Email}", userRequest);
            throw new ApplicationException($"El usuario ya se encuentra registrado en Assert: {userRequest.Email}");
        }

        var userId = await _userRepository.Create(userRequest.Email, Platform.Local,
            userRequest.Name, userRequest.LastName, 3, null, "", 1, "",
        userRequest.CountryId, userRequest.PhoneNumber);
        var accountId = await _accountRepository.Create(userId, userRequest.Password);
        var userRolId = await _userRolRespository.CreateGuest(userId);

        if (userId > 0)
        {
            _logger.LogInformation($"Enrollment local user: {userRequest.Email}", userRequest);
            return await LoginAndEnrollment("local", "", userRequest.Email, userRequest.Password);
        }

        _logger.LogError($"Enrollment error to create local user: {userRequest.Email}", userRequest);
        throw new ApplicationException($"No se pudo realizar el enrollamiento del usuario {userRequest.Email}");
    }

    public async Task<ReturnModelDTO> RenewJwtToken(string expiredToken)
    {
        try
        {
            var (claims, isValid) = _jwtSecurity.GetClaimsFromExpiredToken(expiredToken);

            if (!isValid || claims == null)
            {
                return new ReturnModelDTO
                {
                    StatusCode = ResultStatusCode.Unauthorized,
                    HasError = true,
                    ResultError = new ErrorCommonDTO { Message = "Invalid or malformed token." }
                };
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(expiredToken);
            string userName = jwtToken?.Subject;
            string? platform = jwtToken.Claims.FirstOrDefault(c => c.Type == "platform")?.Value;


            if (string.IsNullOrEmpty(userName))
            {
                return new ReturnModelDTO
                {
                    StatusCode = ResultStatusCode.Unauthorized,
                    HasError = true,
                    ResultError = new ErrorCommonDTO { Message = "Token does not contain username." }
                };
            }
            var enumPlatform = _mapper.Map<Platform>(platform);

            var userAccount = await _userRepository.ValidateUserName(userName, true, enumPlatform);
            int userId;
            int accountId;
            int userRolId;

            if (userAccount.StatusCode != "200")
            {
                return new ReturnModelDTO
                {
                    StatusCode = ResultStatusCode.Unauthorized,
                    HasError = true,
                    ResultError = new ErrorCommonDTO { Message = "User not found or inactive." }
                };
            }


            List<string> userRoles = new();

            var user = (TuUser)userAccount.Data!;
            userId = user.UserId;
            accountId = Convert.ToInt32(user.TuAccounts.First().AccountId);
            userRoles = await _userRolRespository.GetUserRoles(userId);
            
                userRoles = await _userRolRespository.GetUserRoles(userId);
            _metadata.UserId = userId;
            _metadata.UserName = userName;
            _metadata.AccountId = accountId;


            #region claims
            List<Claim> _claims = new()
            {
                new("identifier", _metadata.UserId.ToString()),
                new("value", _metadata.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim("platform", platform)
            };

            foreach (var role in userRoles)
                _claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            #endregion

            #region jwt generator
            string newJwtToken = await _jwtSecurity.GenerateJwt(_claims);
            #endregion

            #region update session info
            await _accountRepository.UpdateLastSessionInfo();
            #endregion


            return new ReturnModelDTO
            {
                StatusCode = ResultStatusCode.OK,
                Data = new { jwtToken = newJwtToken }
            };
        }
        catch (Exception ex)
        {
            return HandleException<ReturnModelDTO>("AppUserService.RenewJwtToken", ex, new { expiredToken }, useTechnicalMessages: true); // Consider the useTechnicalMessages flag
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
