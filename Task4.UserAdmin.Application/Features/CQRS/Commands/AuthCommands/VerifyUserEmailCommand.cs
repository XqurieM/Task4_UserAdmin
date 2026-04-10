namespace Task4.UserAdmin.Application.Features.CQRS.Commands.AuthCommands;

public sealed class VerifyUserEmailCommand
{
    public string Token { get; set; } = string.Empty;
}
