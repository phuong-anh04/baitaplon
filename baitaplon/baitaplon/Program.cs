using baitaplon.Models;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EazydealsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connect")));

builder.Services.AddRazorPages().AddNToastNotifyNoty(new NotyOptions
{
    ProgressBar = true,
    Timeout = 5000
});

builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
    config.HasRippleEffect = true;
});

builder.Services.AddControllersWithViews();
//add session
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".anhntp.session";
    options.Cookie.HttpOnly = true;
});

// Authentication - Authorize Servicess
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    //options.AccessDeniedPath = new PathString("/Manager/Home/Index");
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true,
        Name = "anhntp.cookie",
        Path = "/",
        SameSite = SameSiteMode.Lax,
        SecurePolicy = CookieSecurePolicy.SameAsRequest
    };
    options.LoginPath = "/Account/Login";
    options.ReturnUrlParameter = "returnUrl";
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseSession();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Using toast notify and notyf in this app
app.UseNToastNotify();
app.UseNotyf();


// Add Controller Route in area Admin
app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "account",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
