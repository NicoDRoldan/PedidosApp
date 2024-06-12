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
            ViewBag.RubrosActivos = await _context.Rubros.ToListAsync();
            ViewBag.CategoriasActivas = await _context
                .Categorias
                .Join(_context.Articulos_Categorias,
                    c => c.Id_Categoria,
                    ac => ac.Id_Categoria,
                    (c, ac) => new { Categoria = c, ArticuloCategoria = ac })
                .Join(_context.Articulos,
                    acc => acc.ArticuloCategoria.Id_Articulo,
                    a => a.Id_Articulo,
                    (acc, a) => acc.Categoria) // Proyectar solo la categoría
                .Distinct() // Para asegurarte de que no haya duplicados
                .ToListAsync();

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
