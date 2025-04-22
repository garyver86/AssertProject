using Assert.Domain.Models;
using System.Security.Claims;

namespace Assert.Infrastructure.Security
{
    public interface IJWTSecurity
    {
        Task<string> GenerateJwt(List<Claim> claims);
        Task<List<Claim>?> GetTokenClaims(string token);
        Task<ReturnModel> ValidateToken(string token, string credentials);
        Task<ReturnModel> ValidateTokenUser(string token, string user);
    }
}
