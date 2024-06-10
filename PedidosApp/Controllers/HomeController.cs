using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedidosApp.Data;
using PedidosApp.Models;
using System.Diagnostics;

namespace PedidosApp.Controllers
{
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
            var articulosModel = await _context.Articulos
                .Include(a => a.Precio)
                .Include(a => a.Rubro)
                .Include(a => a.Articulos_Categorias)
                    .ThenInclude(ac => ac.Categoria)
                .Where(a => a.Precio.Precio != null && a.Precio.Precio != 0)
                .ToListAsync();

            return View(articulosModel);
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
