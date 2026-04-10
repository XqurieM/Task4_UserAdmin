using Dapper;
using Task4.UserAdmin.Application.Features.CQRS.Queries.AuthQueries;
using Task4.UserAdmin.Application.Features.CQRS.Results.AuthResults;
using Task4.UserAdmin.Application.Interfaces;
using Task4.UserAdmin.Application.Constants;

namespace Task4.UserAdmin.Application.Features.CQRS.Handlers.AuthHandlers;

public sealed class AuthenticateUserHandler : Features.CQRS.ICQRS.IAuthenticateUser
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IPasswordHasher _passwordHasher;

    public AuthenticateUserHandler(ISqlConnectionFactory sqlConnectionFactory, IPasswordHasher passwordHasher)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthenticateUserResult> AuthenticateUser(AuthenticateUserQuery authenticateUserQuery)
    {
        if (string.IsNullOrWhiteSpace(authenticateUserQuery.Email) || string.IsNullOrWhiteSpace(authenticateUserQuery.Password))
        {
            return new AuthenticateUserResult { Success = false, Message = "Email and password are required." };
        }

        using var connection = _sqlConnectionFactory.CreateConnection();
        var user = await connection.QueryFirstOrDefaultAsync<LoginLookupDto>(
            StoredProcedureNames.UsersGetByNormalizedEmail,
            new { NormalizedEmail = authenticateUserQuery.Email.Trim().ToUpperInvariant() },
            commandType: System.Data.CommandType.StoredProcedure);

        if (user is null)
        {
            return new AuthenticateUserResult { Success = false, Message = "Invalid email or password." };
        }

        if (user.IsBlocked)
        {
            return new AuthenticateUserResult { Success = false, Message = "This user is blocked. (or This user account has been blocked.)" };
        }

        if (!_passwordHasher.Verify(authenticateUserQuery.Password, user.PasswordSalt, user.PasswordHash))
        {
            return new AuthenticateUserResult { Success = false, Message = "Invalid email or password. (This is the same as the second phrase)." };
        }

        await connection.ExecuteAsync(
            StoredProcedureNames.UsersUpdateLoginInfo,
            new { UserId = user.Id, LastLoginAtUtc = DateTime.UtcNow },
            commandType: System.Data.CommandType.StoredProcedure);

        return new AuthenticateUserResult
        {
            Success = true,
            Message = user.IsEmailVerified ? "Giriş başarılı." : "Giriş başarılı. E-postanız henüz doğrulanmamış.",
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            IsBlocked = user.IsBlocked,
            IsEmailVerified = user.IsEmailVerified
        };
    }

    private sealed class LoginLookupDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public bool IsBlocked { get; set; }
        public bool IsEmailVerified { get; set; }
    }
}
