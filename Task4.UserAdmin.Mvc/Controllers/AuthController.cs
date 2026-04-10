using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Task4.UserAdmin.Application.Features.CQRS;
using Task4.UserAdmin.Application.Features.CQRS.Commands.AuthCommands;
using Task4.UserAdmin.Application.Features.CQRS.Queries.AuthQueries;
using Task4.UserAdmin.Mvc.Models;

namespace Task4.UserAdmin.Mvc.Controllers;

public sealed class AuthController : Controller
{
    private readonly ICQRS.IRegisterUser _registerUser;
    private readonly ICQRS.IAuthenticateUser _authenticateUser;
    private readonly ICQRS.IVerifyUserEmail _verifyUserEmail;

    public AuthController(
        ICQRS.IRegisterUser registerUser,
        ICQRS.IAuthenticateUser authenticateUser,
        ICQRS.IVerifyUserEmail verifyUserEmail)
    {
        _registerUser = registerUser;
        _authenticateUser = authenticateUser;
        _verifyUserEmail = verifyUserEmail;
    }

    [HttpGet]
    public IActionResult Login(string? message = null)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            ViewBag.StatusMessage = message;
        }

        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _authenticateUser.AuthenticateUser(new AuthenticateUserQuery
        {
            Email = model.Email,
            Password = model.Password
        });

        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, result.UserId.ToString()),
            new(ClaimTypes.Name, result.FullName),
            new(ClaimTypes.Email, result.Email)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction("Index", "Users");
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var verificationLinkBase = Url.Action(
            action: nameof(VerifyEmail),
            controller: "Auth",
            values: null,
            protocol: Request.Scheme) + "?token=";

        var result = await _registerUser.RegisterUser(new RegisterUserCommand
        {
            FullName = model.FullName,
            Email = model.Email,
            Password = model.Password,
            VerificationLinkBase = verificationLinkBase ?? string.Empty
        });

        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public async Task<IActionResult> VerifyEmail(string token)
    {
        var result = await _verifyUserEmail.VerifyUserEmail(new VerifyUserEmailCommand { Token = token });
        TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }
}
