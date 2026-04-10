namespace Task4.UserAdmin.Application.Features.CQRS.Queries.AuthQueries;

public sealed class AuthenticateUserQuery
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
