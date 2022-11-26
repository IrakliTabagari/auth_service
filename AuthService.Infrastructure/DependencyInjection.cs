using AuthService.Application.Common.Interfaces.Login;
using AuthService.Application.Common.Interfaces.Persistence;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

}