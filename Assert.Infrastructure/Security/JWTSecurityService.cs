using Assert.Domain.Common;
using Assert.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Assert.Infrastructure.Security
{
    public class JWTSecurityService : IJWTSecurity
    {
        private readonly JwtConfiguration _jwtConfig;

        public JWTSecurityService(IOptions<JwtConfiguration> jwtConfig)
        {
            _jwtConfig = jwtConfig.Value;
        }

        public async Task<string> GenerateJwt(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var expiryDate = _jwtConfig.TimeLife <= 0
                ? DateTime.UtcNow.AddMinutes(120)
                : DateTime.UtcNow.AddMinutes(_jwtConfig.TimeLife);

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: expiryDate,
                signingCredentials: credentials,
                notBefore: DateTime.UtcNow

            );
            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token)); // await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(token));
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
            if (jwtToken?.Subject == user)
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

        private static TimeSpan GetTokenRemainingTime(string token)
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

        public (List<Claim> claims, bool isValid) GetClaimsFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _jwtConfig.Issuer,
                ValidAudience = _jwtConfig.Audience,
                ValidateAudience = true,
                ValidateLifetime = false, // ¡No validar la vida útil!
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                var jwtSecurityToken = validatedToken as JwtSecurityToken;

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase))
                {
                    return (null, false);
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                principal.Claims.Append(new Claim(JwtRegisteredClaimNames.Sub, jwtToken?.Subject));

                return (principal.Claims.ToList(), true);
            }
            catch (SecurityTokenException)
            {
                return (null, false);
            }
            catch (Exception ex)
            {
                // Registra la excepción o realiza un manejo de errores adecuado
                Console.WriteLine($"Error al obtener claims del token expirado: {ex.Message}");
                return (null, false); // Considera diferentes valores de retorno para diferentes tipos de error
            }
        }
    }
}
