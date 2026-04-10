using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task4.UserAdmin.Application.Features.CQRS;
using Task4.UserAdmin.Application.Features.CQRS.Commands.UserCommands;
using Task4.UserAdmin.Application.Features.CQRS.Queries.UserQueries;
using Task4.UserAdmin.Mvc.Models;

namespace Task4.UserAdmin.Mvc.Controllers;

[Authorize]
public sealed class UsersController : Controller
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
    public async Task<IActionResult> Index(string? searchTerm)
    {
        var users = await _getUsersForManagement.GetUsersForManagement(new GetUsersForManagementQuery { SearchTerm = searchTerm });

        return View(new UserManagementPageViewModel
        {
            SearchTerm = searchTerm,
            Users = users
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BlockSelected(BulkActionRequest request)
    {
        var result = await _blockUsers.BlockUsers(new BlockUsersCommand { UserIds = request.SelectedUserIds }, GetCurrentUserId());
        return await HandleBulkResult(result, request.SearchTerm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UnblockSelected(BulkActionRequest request)
    {
        var result = await _unblockUsers.UnblockUsers(new UnblockUsersCommand { UserIds = request.SelectedUserIds }, GetCurrentUserId());
        return await HandleBulkResult(result, request.SearchTerm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSelected(BulkActionRequest request)
    {
        var result = await _deleteUsers.DeleteUsers(new DeleteUsersCommand { UserIds = request.SelectedUserIds }, GetCurrentUserId());
        return await HandleBulkResult(result, request.SearchTerm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUnverifiedSelected(BulkActionRequest request)
    {
        var result = await _deleteUnverifiedUsers.DeleteUnverifiedUsers(new DeleteUnverifiedUsersCommand { UserIds = request.SelectedUserIds }, GetCurrentUserId());
        return await HandleBulkResult(result, request.SearchTerm);
    }

    private int GetCurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private async Task<IActionResult> HandleBulkResult(Task4.UserAdmin.Application.Features.CQRS.Results.UserResults.BulkOperationResult result, string? searchTerm)
    {
        TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

        if (result.TouchedCurrentUser)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth", new { message = "This action affected your current session; please log in again." });
        }

        return RedirectToAction(nameof(Index), new { searchTerm });
    }
}
