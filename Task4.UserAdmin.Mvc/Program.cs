using Microsoft.AspNetCore.Authentication.Cookies;
using Task4.UserAdmin.Application.Extensions;
using Task4.UserAdmin.Application.Interfaces;
using Task4.UserAdmin.Persistence.Extensions;
using Task4.UserAdmin.Mvc.Middleware;
using Task4.UserAdmin.Mvc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTask4Application(builder.Configuration);
builder.Services.AddTask4Persistence(builder.Configuration);

builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddSingleton<IEmailQueue, SmtpBackgroundEmailQueue>();
builder.Services.AddHostedService(sp => (SmtpBackgroundEmailQueue)sp.GetRequiredService<IEmailQueue>());

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/Login";
        options.Cookie.Name = "Task4UserAdminAuth";
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Auth/Login");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseMiddleware<EnsureCurrentUserIsActiveMiddleware>();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
