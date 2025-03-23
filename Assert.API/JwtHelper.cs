using System.IdentityModel.Tokens.Jwt;

namespace Assert.API
{
    public class JwtHelper
    {
        public static string GetUserIdFromToken(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return null;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            //var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "identifier");
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "value");


            return userIdClaim?.Value;
        }
    }
}
