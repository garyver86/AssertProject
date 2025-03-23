using Assert.Application.DTOs;
using System.Security.Claims;

namespace Assert.Application.Interfaces
{
    public interface ISecurityService
    {
        Task<ReturnModelDTO> UserLogin(string user, string password, string ip, string? browseInfo, string jwtSecret, string jwtIssuer);
        Task<List<Claim>?> GetTokenClaims(string token);
        Task<ReturnModelDTO> ValidateToken(string token, string credentials);
        Task<ReturnModelDTO> ValidateTokenUser(string token, string user);
    }
}
