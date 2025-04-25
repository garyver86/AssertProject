using Assert.Application.DTOs;
using Assert.Application.Interfaces;
using Assert.Domain.Interfaces.Infraestructure.External;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Security;
using AutoMapper;
using System.Security.Claims;

namespace Assert.Application.Services.Security
{
    public class SecurityService : ISecurityService
    {
        private readonly IJWTSecurity _jwtSecurity;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public SecurityService(IJWTSecurity jwtSecurity, IMapper mapper,
            IUserRepository userRepository)
        {
            _jwtSecurity = jwtSecurity;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<string> GenerateJwt(string secretKey, string issuer, string audience, int expireMinutes, List<Claim> claims)
        {
            var result = await _jwtSecurity.GenerateJwt(claims);
            return result;
        }

        public async Task<List<Claim>?> GetTokenClaims(string token)
        {
            var result = await _jwtSecurity.GetTokenClaims(token);
            return result;
        }

        public async Task<ReturnModelDTO> UserLogin(string user, string password, string ip, string? browseInfo, string jwtSecret, string jwtIssuer)
        {
            var authenticationResult = await _userRepository.Login(user, password);

            if (authenticationResult.StatusCode == ResultStatusCode.OK)
            {
                int tokenTime = 30;

                string value = authenticationResult.ResultError?.Message;
                List<Claim> claims = new()
                    {
                        new("identifier", (string)authenticationResult.Data),
                        new("value", value)
                    };

                foreach (var role in authenticationResult.ResultError?.Code.Split(','))
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }

                string jwtToken = await GenerateJwt(jwtSecret, jwtIssuer, user, tokenTime, claims);
                var result = new ReturnModelDTO
                {
                    StatusCode = ResultStatusCode.OK,
                    Data = new
                    {
                        jwtToken
                    }
                };
                return result;
            }
            else
            {
                return new ReturnModelDTO
                {
                    StatusCode = ResultStatusCode.Unauthorized,
                    ResultError = _mapper.Map<ErrorCommonDTO>(authenticationResult.ResultError)
                };
            }
        }

        public async Task<ReturnModelDTO> ValidateToken(string token, string credentials)
        {
            var result = await _jwtSecurity.ValidateToken(token, credentials);
            var resultDto = _mapper.Map<ReturnModelDTO>(result);
            return resultDto;
        }

        public async Task<ReturnModelDTO> ValidateTokenUser(string token, string user)
        {
            var result = await _jwtSecurity.ValidateTokenUser(token, user);
            var resultDto = _mapper.Map<ReturnModelDTO>(result);
            return resultDto;
        }

    }
}
