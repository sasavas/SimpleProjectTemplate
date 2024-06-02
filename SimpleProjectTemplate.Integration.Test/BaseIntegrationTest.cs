using SimpleProjectTemplate.Infrastructure.DataAccess;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleProjectTemplate.Integration.Test;

public abstract class BaseIntegrationTest : IClassFixture<TestWebApplicationFactory>
{
    protected TestWebApplicationFactory Factory;
    protected readonly ISender Sender;
    protected readonly AppDbContext AppDbContext;

    protected BaseIntegrationTest(TestWebApplicationFactory factory)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        
        Factory = factory;
        
        var scope = factory.Services.CreateScope();
        Sender = scope.ServiceProvider.GetRequiredService<ISender>();
        AppDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }
}