using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using InovaBank.Application.Exceptions.ExceptionsBase;
using InovaBank.Application.UserSession;
using InovaBank.Domain.Repositories.User;

namespace InovaBank.API.TokenMiddleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider, UserContext userContext)
        {
            // Verifica se o endpoint tem o atributo [Authorize]
            var endpoint = context.GetEndpoint();
            var hasAuthorizeAttribute = endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>() != null;

            if (hasAuthorizeAttribute)
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var userReadOnlyRepository = scope.ServiceProvider.GetRequiredService<IUserReadOnlyRepository>();

                        var userId = DecodeTokenAndGetUserId(token);
                        if (!string.IsNullOrEmpty(userId))
                        {
                            var user = await userReadOnlyRepository.GetById(userId);
                            if (user == null)
                            {
                                throw new InvalidTokenException();
                            }
                            userContext.UserId = userId;
                        }
                    }
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
