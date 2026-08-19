using F1.Data;
using F1.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;


var builder = WebApplication.CreateBuilder(args);

// =========================
// Conexion y contexto
// =========================
string? connectionString = builder.Configuration.GetConnectionString("SqlF1");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("Falta ConnectionStrings__SqlF1. Configúrala como secreto o variable de entorno.");
builder.Services.AddDbContext<UsuariosContext>(options =>
	options.UseSqlServer(connectionString));

// =========================
// Repositorio
// =========================
builder.Services.AddTransient<IRepositoryF1, RepositoryF1>();

// =========================
// Session
// =========================
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
	options.Cookie.SameSite = SameSiteMode.Strict;
});

// =========================
// Authentication cookie
// =========================
//builder.Services.AddAuthentication(options =>
//{
//	options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//	options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//	options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//})
//.AddCookie(options =>
//{
//	options.LoginPath = "/Managed/LogIn";
//	options.AccessDeniedPath = "/Managed/ErrorAcceso";

//	options.Cookie.Name = "F1Auth";
//	options.Cookie.HttpOnly = true;
//	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//	options.Cookie.SameSite = SameSiteMode.Strict;

//	options.ExpireTimeSpan = TimeSpan.FromHours(1);
//	options.SlidingExpiration = true;
//});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Managed/LogIn";
        options.AccessDeniedPath = "/Managed/ErrorAcceso";
        options.Cookie.Name = "NerfoF1.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("auth", limiter =>
    {
        limiter.PermitLimit = 5;
        limiter.Window = TimeSpan.FromMinutes(15);
        limiter.QueueLimit = 0;
        limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});


// =========================
// MVC
// =========================
builder.Services.AddControllersWithViews(options =>
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute()))
    .AddSessionStateTempDataProvider();
builder.Services.AddControllers(); // Para API si hace falta

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=F1}/{action=Index}/{id?}"
);
app.MapControllers();

app.Run();
