using Assert.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Assert.Infrastructure.Security
{
    public class JWTSecurityService : IJWTSecurity
    {
        public async Task<string> GenerateJwt(string secretKey, string issuer, string audience, int expireMinutes, List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            DateTime expiryDate = DateTime.UtcNow;
            if (expireMinutes <= 0)
            {
                expiryDate = DateTime.UtcNow.AddYears(10);
            }
            else
            {
                expiryDate = DateTime.UtcNow.AddMinutes(expireMinutes);
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiryDate,
                signingCredentials: credentials,
                notBefore: DateTime.UtcNow
            );
            return await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<List<Claim>?> GetTokenClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = await Task.Run(() => handler.ReadJwtToken(token));

            return jwtToken.Claims?.ToList();
        }

        public async Task<ReturnModel> ValidateToken(string token, string credentials)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = await Task.Run(() => handler.ReadJwtToken(token));
            if (jwtToken?.Audiences?.Any(x => x == credentials) ?? false)
            {
                return new ReturnModel
                {
                    StatusCode = "200"
                };
            }
            else
            {
                return new ReturnModel
                {
                    StatusCode = "401"
                };
            }
        }
        public async Task<ReturnModel> ValidateTokenUser(string token, string user)
        {
            TimeSpan remainingTime = GetTokenRemainingTime(token);
            var remainingTimeData = remainingTime.TotalSeconds;
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = await Task.Run(() => handler.ReadJwtToken(token));
            if (jwtToken?.Audiences?.Any(x => x == user) ?? false)
            {
                return new ReturnModel
                {
                    StatusCode = "200"
                };
            }
            else
            {
                return new ReturnModel
                {
                    StatusCode = "401"
                };
            }
        }

        public static TimeSpan GetTokenRemainingTime(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Obtener el claim de expiración (exp)
            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;

            if (expClaim != null)
            {
                // Convertir el claim de expiración (exp) de un timestamp UNIX a DateTime
                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim)).UtcDateTime;

                // Calcular el tiempo restante
                var remainingTime = expirationTime - DateTime.UtcNow;
                return remainingTime;
            }

            throw new ArgumentException("El token no contiene un claim de expiración.");
        }
    }
}
