using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SimpleProjectTemplate.Api.DependencyConfigurations;

public static class AuthenticationProviderConfiguration
{
    public static IServiceCollection AddAuthenticationProviders(
        this IServiceCollection services, ConfigurationManager configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddAuthentication()
            .AddCookie("Cookies");

        return services;
    }
}