using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedidosApp.Data;
using PedidosApp.Models;
using System.Diagnostics;

namespace PedidosApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PedidosAppContext _context;

        public HomeController(ILogger<HomeController> logger, PedidosAppContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var carrito = new List<CarritoModel>
                {
                    new CarritoModel { NombreArticulo = "Artículo 1", Cantidad = 2, PrecioUnitario = 15, Total = 30},
                    new CarritoModel { NombreArticulo = "Artículo 2", Cantidad = 1, PrecioUnitario = 15, Total = 15}
                };

            ViewBag.Carrito = carrito; // Usamos ViewBag para pasar la lista a la vista

            return View(await _context.Articulos.Include(a => a.Precio).Where(a => a.Precio.Precio != null && a.Precio.Precio != 0).ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
