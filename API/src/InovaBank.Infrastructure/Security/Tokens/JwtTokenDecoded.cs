using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using InovaBank.Domain.Security.Tokens;

namespace InovaBank.Infrastructure.Security.Tokens
{
    public class JwtTokenDecoded : IJwtTokenDecoded
    {
        public string Decoded(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var id = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            return id!;
        }
    }
}
