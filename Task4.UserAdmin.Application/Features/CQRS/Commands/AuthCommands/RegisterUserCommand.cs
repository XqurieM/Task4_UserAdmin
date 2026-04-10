namespace Task4.UserAdmin.Application.Features.CQRS.Commands.AuthCommands;

public sealed class RegisterUserCommand
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VerificationLinkBase { get; set; } = string.Empty;
}
