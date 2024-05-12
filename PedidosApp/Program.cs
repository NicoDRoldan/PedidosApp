using Microsoft.EntityFrameworkCore;
using PedidosApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PedidosAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PedidosAppContext")
    ?? throw new InvalidOperationException("Connection string 'PedidosAppContext' not found.")));

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
    pattern: "{controller=Articulo}/{action=Index}/{id?}");

app.Run();
