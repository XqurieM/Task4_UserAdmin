namespace Task4.UserAdmin.Application.Features.CQRS.Commands.UserCommands;

public sealed class DeleteUnverifiedUsersCommand
{
    public List<int> UserIds { get; set; } = new();
}
