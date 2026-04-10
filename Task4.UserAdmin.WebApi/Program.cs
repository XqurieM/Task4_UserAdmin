using Microsoft.AspNetCore.Authentication.Cookies;
using Task4.UserAdmin.Application.Extensions;
using Task4.UserAdmin.Persistence.Extensions;
using Task4.UserAdmin.Application.Interfaces;
using Task4.UserAdmin.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTask4Application(builder.Configuration);
builder.Services.AddTask4Persistence(builder.Configuration);
builder.Services.AddSingleton<IEmailQueue, NullEmailQueue>();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        x => x.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

app.UseCors("AllowAll");    
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
