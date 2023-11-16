using Core.Controllers;
using Core.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.RegularExpressions;

void Log(string part, string? msg = null)
{
    var log = $"LOG > {part}{(!string.IsNullOrEmpty(msg) ? $": {msg}" : "")}";
    Debug.WriteLine(log);
}

var traceMather = new Regex("\\s*at (.+) in ([^\n]+)|\\s*at (.+)\n", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Enable UseMvc
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

// Enable UseControllers
builder.Services.AddControllers(options => options.EnableEndpointRouting = false);

var app = builder.Build();

// Log start and end of request
app.Use(async (ctx, next) =>
{
    Log("Begin Requests", $"<-- Requests starts HERE ({ctx.Request?.Path})");

    try
    {
        await next(ctx);
    }
    catch (TraceException ex)
    {
        var matches = traceMather.Matches(ex.StackTrace).Cast<Match>();
        var steps = matches.Select(m => m.Groups[1]?.Length > 0 ? m.Groups[1].Value : m.Groups[3]?.Value);

        Log("Trace", $"\n\t{string.Join("\n\t", steps)}");
    }

    Log("End Request", $"<-- Requests ends HERE");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Prepares routing values
// Exists since ASP.NET Core 3.0
app.UseRouting();

// Handles user logins
app.UseAuthentication();
app.UseAuthorization();

// MVC / Razor pages /...
// Uses endpoints middleware
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Verbose version
// Exists since ASP.NET Core 3.0
// - Needs UseRounting to be called before
// - Can be anywhere between UseRouting and end of pipeline
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

// MVC middleware 
// Exists since ASP.NET Core 1.0
// - Must have services registered
// - Must be last middleware
// - Does both routing and endpoints
app.UseMvc(mvc =>
{
    mvc.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
