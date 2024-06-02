using SimpleProjectTemplate.Infrastructure.DataAccess;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleProjectTemplate.Integration.Test;

public abstract class BaseUserIntegrationTest : IClassFixture<TestWebApplicationFactory>
{
    protected readonly ISender Sender;
    protected readonly AppDbContext AppDbContext;

    protected BaseUserIntegrationTest(TestWebApplicationFactory factory)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

        var scope = factory.Services.CreateScope();
        Sender = scope.ServiceProvider.GetRequiredService<ISender>();
        AppDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }
}