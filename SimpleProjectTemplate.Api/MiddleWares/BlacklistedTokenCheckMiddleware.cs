using SimpleProjectTemplate.Domain.Features.Authentication;

namespace SimpleProjectTemplate.Api.MiddleWares;

public class BlacklistedTokenCheckMiddleware
{
    private readonly RequestDelegate _next;

    public BlacklistedTokenCheckMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IBlacklistedTokenRepository blacklistedTokenRepository)
    {
        if (context.User.Identity?.IsAuthenticated ?? false)
        {
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            if (token != null && blacklistedTokenRepository.IsTokenBlacklisted(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token is not blacklisted");
                return;
            }
        }

        await _next(context);
    }
}
