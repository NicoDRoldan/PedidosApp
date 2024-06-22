using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PedidosApp.Data;
using PedidosApp.Interfaces;
using PedidosApp.Services;
using Polly;
using Polly.Extensions.Http;
using System.Diagnostics;

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

// Crear HttpClient para llamado a Api de PedidosAppi:
builder.Services.AddHttpClient("PedidosAppiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7273/api");
});

//// Crear HttpClient para llamado a WebService de Cupones:
//builder.Services.AddHttpClient("WSCuponesClient", client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7159/api");
//    })
//    .AddPolicyHandler(GetRetryPolicy())
//    .AddPolicyHandler(GetCircuitBreakerPolicy());

// Crear HttpClient para llamado a WebService de Cupones:
builder.Services.AddHttpClient("WSCuponesClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5203/api");
    })
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5089); //HTTP
    options.ListenAnyIP(7006, listenOptions => listenOptions.UseHttps());
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(2, retryAttemp => TimeSpan.FromSeconds(10),
        onRetry: (outcome, timespan, retryAttemp, context) =>
        {
            Debug.WriteLine($"Reintentando... Intento: {retryAttemp}");
        });
}

IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));
}