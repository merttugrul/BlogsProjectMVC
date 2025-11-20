using APP.Domain;
using APP.Services;
using APP.Models;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext ekleme (SQLite)
builder.Services.AddDbContext<Db>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("UsersDB")));

// DbContext base type'ını da register et
builder.Services.AddScoped<DbContext>(provider => provider.GetService<Db>());

// Service Dependency Injection
builder.Services.AddScoped<IService<GroupRequest, GroupResponse>, GroupService>();
builder.Services.AddScoped<IService<RoleRequest, RoleResponse>, RoleService>();
builder.Services.AddScoped<IService<UserRequest, UserResponse>, UserService>();
builder.Services.AddScoped<IService<BlogRequest, BlogResponse>, BlogService>();
builder.Services.AddScoped<IService<TagRequest, TagResponse>, TagService>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();