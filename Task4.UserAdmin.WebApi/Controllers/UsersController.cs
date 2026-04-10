using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task4.UserAdmin.Application.Features.CQRS;
using Task4.UserAdmin.Application.Features.CQRS.Commands.UserCommands;
using Task4.UserAdmin.Application.Features.CQRS.Queries.UserQueries;

namespace Task4.UserAdmin.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    private readonly ICQRS.IGetUsersForManagement _getUsersForManagement;
    private readonly ICQRS.IBlockUsers _blockUsers;
    private readonly ICQRS.IUnblockUsers _unblockUsers;
    private readonly ICQRS.IDeleteUsers _deleteUsers;
    private readonly ICQRS.IDeleteUnverifiedUsers _deleteUnverifiedUsers;

    public UsersController(
        ICQRS.IGetUsersForManagement getUsersForManagement,
        ICQRS.IBlockUsers blockUsers,
        ICQRS.IUnblockUsers unblockUsers,
        ICQRS.IDeleteUsers deleteUsers,
        ICQRS.IDeleteUnverifiedUsers deleteUnverifiedUsers)
    {
        _getUsersForManagement = getUsersForManagement;
        _blockUsers = blockUsers;
        _unblockUsers = unblockUsers;
        _deleteUsers = deleteUsers;
        _deleteUnverifiedUsers = deleteUnverifiedUsers;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? searchTerm)
        => Ok(await _getUsersForManagement.GetUsersForManagement(new GetUsersForManagementQuery { SearchTerm = searchTerm }));

    [HttpPost("block")]
    public async Task<IActionResult> Block([FromBody] BlockUsersCommand command, [FromQuery] int currentUserId)
        => Ok(await _blockUsers.BlockUsers(command, currentUserId));

    [HttpPost("unblock")]
    public async Task<IActionResult> Unblock([FromBody] UnblockUsersCommand command, [FromQuery] int currentUserId)
        => Ok(await _unblockUsers.UnblockUsers(command, currentUserId));

    [HttpPost("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteUsersCommand command, [FromQuery] int currentUserId)
        => Ok(await _deleteUsers.DeleteUsers(command, currentUserId));

    [HttpPost("delete-unverified")]
    public async Task<IActionResult> DeleteUnverified([FromBody] DeleteUnverifiedUsersCommand command, [FromQuery] int currentUserId)
        => Ok(await _deleteUnverifiedUsers.DeleteUnverifiedUsers(command, currentUserId));
}
