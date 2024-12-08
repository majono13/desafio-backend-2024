using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using InovaBank.Application.UserSession;

namespace InovaBank.API.TokenMiddleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, UserContext userContext)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                var userId = DecodeTokenAndGetUserId(token);
                if (!String.IsNullOrEmpty(userId)) 
                { 
                    userContext.UserId = userId ?? string.Empty;
                }
            }
            await _next(context);
        }

        private string? DecodeTokenAndGetUserId(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jsonToken = jwtHandler.ReadToken(token) as JwtSecurityToken;
            return jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        }
    }
}
