using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Task4.UserAdmin.Application.Features.CQRS;
using Task4.UserAdmin.Application.Features.CQRS.Queries.UserQueries;

namespace Task4.UserAdmin.Mvc.Middleware;

public sealed class EnsureCurrentUserIsActiveMiddleware
{
    private readonly RequestDelegate _next;

    public EnsureCurrentUserIsActiveMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICQRS.IGetCurrentUserSessionState getCurrentUserSessionState)
    {
        if (context.User.Identity?.IsAuthenticated == true && !IsExcludedPath(context.Request.Path))
        {
            var rawUserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(rawUserId, out var userId))
            {
                var state = await getCurrentUserSessionState.GetCurrentUserSessionState(new GetCurrentUserSessionStateQuery
                {
                    UserId = userId
                });

                if (state is null || !state.Exists || state.IsBlocked)
                {
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    var message = state is null || !state.Exists
                        ? "Your account has been deleted or no longer exists."
                        : "Your account has been blocked, and you have been logged out.";

                    context.Response.Redirect($"/Auth/Login?message={Uri.EscapeDataString(message)}");
                    return;
                }
            }
        }

        await _next(context);
    }

    private static bool IsExcludedPath(PathString path)
    {
        if (!path.HasValue)
        {
            return true;
        }

        var value = path.Value!.ToLowerInvariant();

        return value.StartsWith("/auth/login")
               || value.StartsWith("/auth/register")
               || value.StartsWith("/auth/verifyemail")
               || value.StartsWith("/favicon")
               || value.StartsWith("/css")
               || value.StartsWith("/js")
               || value.StartsWith("/lib");
    }
}
