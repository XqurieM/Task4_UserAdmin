using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Task4.UserAdmin.Application.Features.CQRS.Handlers.AuthHandlers;
using Task4.UserAdmin.Application.Features.CQRS.Handlers.UserHandlers;
using Task4.UserAdmin.Application.Interfaces;
using Task4.UserAdmin.Application.Services;

namespace Task4.UserAdmin.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTask4Application(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<Features.CQRS.ICQRS.IRegisterUser, RegisterUserHandler>();
        services.AddScoped<Features.CQRS.ICQRS.IAuthenticateUser, AuthenticateUserHandler>();
        services.AddScoped<Features.CQRS.ICQRS.IVerifyUserEmail, VerifyUserEmailHandler>();
        services.AddScoped<Features.CQRS.ICQRS.IGetUsersForManagement, GetUsersForManagementHandler>();
        services.AddScoped<Features.CQRS.ICQRS.IGetCurrentUserSessionState, GetCurrentUserSessionStateHandler>();
        services.AddScoped<Features.CQRS.ICQRS.IBlockUsers, BlockUsersHandler>();
        services.AddScoped<Features.CQRS.ICQRS.IUnblockUsers, UnblockUsersHandler>();
        services.AddScoped<Features.CQRS.ICQRS.IDeleteUsers, DeleteUsersHandler>();
        services.AddScoped<Features.CQRS.ICQRS.IDeleteUnverifiedUsers, DeleteUnverifiedUsersHandler>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        return services;
    }
}
