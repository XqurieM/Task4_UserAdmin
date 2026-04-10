using Microsoft.AspNetCore.Mvc;
using Task4.UserAdmin.Application.Features.CQRS;
using Task4.UserAdmin.Application.Features.CQRS.Commands.AuthCommands;
using Task4.UserAdmin.Application.Features.CQRS.Queries.AuthQueries;

namespace Task4.UserAdmin.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly ICQRS.IRegisterUser _registerUser;
    private readonly ICQRS.IAuthenticateUser _authenticateUser;
    private readonly ICQRS.IVerifyUserEmail _verifyUserEmail;

    public AuthController(ICQRS.IRegisterUser registerUser, ICQRS.IAuthenticateUser authenticateUser, ICQRS.IVerifyUserEmail verifyUserEmail)
    {
        _registerUser = registerUser;
        _authenticateUser = authenticateUser;
        _verifyUserEmail = verifyUserEmail;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        => Ok(await _registerUser.RegisterUser(command));

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateUserQuery query)
        => Ok(await _authenticateUser.AuthenticateUser(query));

    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] VerifyUserEmailCommand command)
        => Ok(await _verifyUserEmail.VerifyUserEmail(command));
}
