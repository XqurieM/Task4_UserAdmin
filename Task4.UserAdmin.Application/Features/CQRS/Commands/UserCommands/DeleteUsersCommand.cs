namespace Task4.UserAdmin.Application.Features.CQRS.Commands.UserCommands;

public sealed class DeleteUsersCommand
{
    public List<int> UserIds { get; set; } = new();
}
