using APP.Domain;
using APP.Services;
using APP.Models;
using CORE.APP.Services.MVC;
using CORE.APP.Services.Authentication.MVC;
using CORE.APP.Services.Session.MVC;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext ekleme (SQLite)
builder.Services.AddDbContext<DbContext, Db>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("UsersDB")));

// HttpContextAccessor for accessing HTTP context in services
builder.Services.AddHttpContextAccessor();

// Service Dependency Injection
builder.Services.AddScoped<IService<GroupRequest, GroupResponse>, GroupService>();
builder.Services.AddScoped<IService<RoleRequest, RoleResponse>, RoleService>();
builder.Services.AddScoped<IService<UserRequest, UserResponse>, UserService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IService<BlogRequest, BlogResponse>, BlogService>();
builder.Services.AddScoped<IService<TagRequest, TagResponse>, TagService>();

// Cookie Authentication Service
builder.Services.AddScoped<ICookieAuthService, CookieAuthService>();

// Session Services
builder.Services.AddScoped<SessionServiceBase, SessionService>();
builder.Services.AddScoped<IRecentBlogsService, RecentBlogsService>();

// Session configuration
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Authentication configuration
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login";
        options.AccessDeniedPath = "/Users/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();