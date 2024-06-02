using SimpleProjectTemplate.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using SimpleProjectTemplate.Domain.DataAccess;
using Testcontainers.PostgreSql;

namespace SimpleProjectTemplate.Integration.Test;

public class TestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public static ServiceProvider ServiceProvider { get; private set; } = null!;


    private readonly FakeUserIdProvider _fakeUserIdProvider = new();
    public void OverrideUserIdProviderValue(Guid actualUserId)
    {
        _fakeUserIdProvider.SetUserId(actualUserId);
    }
    
    private readonly PostgreSqlContainer _dbContainer =
        new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("__APPNAME__") //TODO:appname
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithCleanUp(true)
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            OverrideDbContext(services);
            OverrideUserIdProvider(services);
            
            ServiceProvider = services.BuildServiceProvider();
        });
    }
    
    private void OverrideDbContext(IServiceCollection services)
    {
        var depDbContext = services
            .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<AppDbContext>));
        if (depDbContext is not null) services.Remove(depDbContext);
        services.AddDbContext<AppDbContext>(options =>
        {
            var connStr = _dbContainer.GetConnectionString();
            options.UseNpgsql(connStr).UseSnakeCaseNamingConvention();
        });
    }

    public Task InitializeAsync() => _dbContainer.StartAsync();

    public new Task DisposeAsync() => _dbContainer.DisposeAsync().AsTask();

    private void OverrideUserIdProvider(IServiceCollection serviceCollection)
    {
        var depUserIdProvider = serviceCollection
            .SingleOrDefault(s => s.ServiceType == typeof(IUserIdProvider));
        if (depUserIdProvider != null) serviceCollection.Remove(depUserIdProvider);
        serviceCollection.AddSingleton<IUserIdProvider>(_fakeUserIdProvider);
    }
    
    private class FakeUserIdProvider : IUserIdProvider
    {
        private Guid UserId;
        public void SetUserId(Guid guid)
        {
            UserId = guid;
        }

        public Guid GetUserId()
        {
            return UserId == default(Guid) ? Guid.NewGuid() : UserId;
        }
    }
}