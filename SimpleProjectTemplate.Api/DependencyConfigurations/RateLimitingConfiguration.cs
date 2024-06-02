using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace SimpleProjectTemplate.Api.DependencyConfigurations;

public static class RateLimitingConfiguration
{
    public const string MoreLimitedPolicy = "ImportFilePolicy";
    
    public static IServiceCollection AddRateLimitingConfiguration(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.GlobalLimiter = PartitionedRateLimiter.CreateChained<HttpContext>(
                PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
                        factory: partition => new FixedWindowRateLimiterOptions()
                        {
                            AutoReplenishment = true,
                            PermitLimit = 15,
                            QueueLimit = 1,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            Window = TimeSpan.FromMinutes(1)
                        }
                    )
                ),
                PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
                        factory: partition => new FixedWindowRateLimiterOptions()
                        {
                            AutoReplenishment = true,
                            PermitLimit = 1_000,
                            QueueLimit = 1,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            Window = TimeSpan.FromHours(1)
                        }
                    )
                )); // global rate limiter

            options.AddFixedWindowLimiter(MoreLimitedPolicy, configOptions =>
            {
                configOptions.PermitLimit = 10;
                configOptions.Window = TimeSpan.FromDays(1);
                configOptions.QueueLimit = 1;
                configOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });
        });

        return services;
    }
}