using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PedidosApp.Data;
using PedidosApp.Interfaces;
using PedidosApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PedidosAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PedidosAppContext")
    ?? throw new InvalidOperationException("Connection string 'PedidosAppContext' not found.")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Access/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        option.SlidingExpiration = true;
        option.AccessDeniedPath = "/Access/AccessDenied";
    });

builder.Services.AddScoped<IAccessService, AccessService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOrAdmin", policy => policy.RequireRole("Admin", "User"));
});

// Crear HttpClient para llamado a apis:
builder.Services.AddHttpClient("PedidosAppiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7273/api");
});

var app = builder.Build();

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
    pattern: "{controller=Access}/{action=Login}/{id?}");

app.Run();
