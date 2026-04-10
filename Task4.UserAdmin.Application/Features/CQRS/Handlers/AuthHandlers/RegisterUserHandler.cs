using Dapper;
using Microsoft.Data.SqlClient;
using Task4.UserAdmin.Application.Features.CQRS.Commands.AuthCommands;
using Task4.UserAdmin.Application.Features.CQRS.Results.AuthResults;
using Task4.UserAdmin.Application.Interfaces;
using Task4.UserAdmin.Application.Constants;

namespace Task4.UserAdmin.Application.Features.CQRS.Handlers.AuthHandlers;

public sealed class RegisterUserHandler : Features.CQRS.ICQRS.IRegisterUser
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailQueue _emailQueue;

    public RegisterUserHandler(
        ISqlConnectionFactory sqlConnectionFactory,
        IPasswordHasher passwordHasher,
        IEmailQueue emailQueue)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _passwordHasher = passwordHasher;
        _emailQueue = emailQueue;
    }

    public async Task<RegisterUserResult> RegisterUser(RegisterUserCommand registerUserCommand)
    {
        if (string.IsNullOrWhiteSpace(registerUserCommand.FullName) ||
            string.IsNullOrWhiteSpace(registerUserCommand.Email) ||
            string.IsNullOrWhiteSpace(registerUserCommand.Password))
        {
            return new RegisterUserResult
            {
                Success = false,
                Message = "Full name, email, and password are required."
            };
        }

        var email = registerUserCommand.Email.Trim();
        var normalizedEmail = email.ToUpperInvariant();
        var salt = _passwordHasher.GenerateSalt();
        var hash = _passwordHasher.HashPassword(registerUserCommand.Password, salt);
        var verificationToken = Guid.NewGuid().ToString("N");

        try
        {
            using var connection = _sqlConnectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@FullName", registerUserCommand.FullName.Trim());
            parameters.Add("@Email", email);
            parameters.Add("@NormalizedEmail", normalizedEmail);
            parameters.Add("@PasswordHash", hash);
            parameters.Add("@PasswordSalt", salt);
            parameters.Add("@EmailVerificationToken", verificationToken);
            parameters.Add("@EmailVerificationTokenExpiresAtUtc", DateTime.UtcNow.AddDays(3));
            parameters.Add("@NewUserId", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            await connection.ExecuteAsync(
                StoredProcedureNames.UsersRegister,
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);

            var newUserId = parameters.Get<int>("@NewUserId");
            var verificationUrl = $"{registerUserCommand.VerificationLinkBase}{verificationToken}";

            await _emailQueue.QueueAsync(new EmailMessage
            {
                To = email,
                Subject = "E-posta doğrulama",
                HtmlBody = $"<p>Hello {registerUserCommand.FullName},</p><p>Your account has been created. Click the link below to verify your email.:</p><p><a href='{verificationUrl}'>Verify my email.</a></p>"
            });

            return new RegisterUserResult
            {
                Success = true,
                UserId = newUserId,
                Message = "Registration successful. A verification email has been sent asynchronously."
            };
        }
        catch (SqlException ex) when (ex.Number is 2601 or 2627)
        {
            return new RegisterUserResult
            {
                Success = false,
                Message = "This email address is already registered."
            };
        }
    }
}
