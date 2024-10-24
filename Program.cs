using EventsMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<UniversityEventManagementContext>(u => u.UseSqlServer(builder.Configuration.GetConnectionString("DbCon")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.ExpireTimeSpan = TimeSpan.FromMinutes(15);
        option.LoginPath = "/UserMasters/Login";
        option.AccessDeniedPath = "/UserMasters/Login";
    });

builder.Services.AddSession(option =>
{
	option.IdleTimeout = TimeSpan.FromMinutes(20);
	option.Cookie.HttpOnly = true;
	option.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=UserMasters}/{action=Login}/{id?}");

app.Run();
