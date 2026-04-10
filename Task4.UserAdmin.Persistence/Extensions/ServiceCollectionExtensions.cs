using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Task4.UserAdmin.Application.Interfaces;
using Task4.UserAdmin.Persistence.Context;

namespace Task4.UserAdmin.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTask4Persistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        return services;
    }
}
