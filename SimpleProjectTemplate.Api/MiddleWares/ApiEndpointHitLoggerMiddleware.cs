using System.Security.Claims;

namespace SimpleProjectTemplate.Api.MiddleWares;

public class ApiEndpointHitLoggerMiddleware : IMiddleware
{
    private readonly ILogger<ApiEndpointHitLoggerMiddleware> _logger;

    public ApiEndpointHitLoggerMiddleware(ILogger<ApiEndpointHitLoggerMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _logger.LogTrace("SimpleProjectTemplate.Api endpoint hit {APIPath}, {User}", 
            context.Request.Path.Value,
            context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier));

        await next(context);
    }
}