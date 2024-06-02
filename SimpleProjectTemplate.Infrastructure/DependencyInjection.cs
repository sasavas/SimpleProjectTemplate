using SimpleProjectTemplate.Infrastructure.DataAccess;
using SimpleProjectTemplate.Infrastructure.DataAccess.MultiTenancy;
using SimpleProjectTemplate.Infrastructure.DataAccess.RepositoryAdaptors;
using SimpleProjectTemplate.Infrastructure.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleProjectTemplate.Application.Ports;
using SimpleProjectTemplate.Domain.DataAccess;
using SimpleProjectTemplate.Domain.Features.Authentication;
using SimpleProjectTemplate.Domain.Features.Authentication.DataAccess;
using SimpleProjectTemplate.Domain.Features.Authentication.RoleModule;

namespace SimpleProjectTemplate.Infrastructure;

public static class DependencyInjection
{
    public static IServiceProvider ServiceProvider = null!;
    
    public static IServiceCollection AddInfrastructureDependencies(
        this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString("__CONNECTIONSTRING__");

        services.AddDbContext<AppDbContext>(
            options =>
            {
                options.UseNpgsql(connectionString)
                    .UseSnakeCaseNamingConvention()
                    .EnableSensitiveDataLogging();
            });

        // Repositories
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserIdProvider, UserIdProvider>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IBlacklistedTokenRepository, BlacklistedTokenRepository>();
        
        services.AddTransient<IMessageSenderGateway, MessageSenderGatewayMock>();
        
        // Cloud-Native Services
        if (environment.IsDevelopment())
        {
            // DEV ENVIRONMENT specific settings
            // services.AddTransient<IMessageSenderGateway, MessageSenderGatewayMock>();
        }
        else
        {
            // PROD ENVIRONMENT specific settings
            // services.AddTransient<IMessageSenderGateway, AzureBusMessageSenderGateway>();
        }

        ServiceProvider = services.BuildServiceProvider();
        
        return services;
    }
}