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
                    (acc, a) => new { JoinArticuloCategoria = acc, Articulo = a })
                .Join(_context.Precios,
                    arp => arp.Articulo.Id_Articulo,
                    p => p.Id_Articulo,
                    (arp, p) => new { JoinArticuloPrecio = arp, Precio = p })
                .Where(p => p.Precio.Precio != null && p.Precio.Precio != 0)
                .Select(result => result.JoinArticuloPrecio.JoinArticuloCategoria.Categoria)
                .Distinct()
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

        [Route("Home/Menu/{id}/{tipo}")]
        public async Task<IActionResult> PedidosPorRubrosOCategorias(int id, string tipo)
        {
            var articulos = await _context.Articulos
                .Include(r => r.Rubro)
                .Include(p => p.Precio)
                .Include(ca => ca.Articulos_Categorias)
                    .ThenInclude(caa => caa.Categoria)
                .ToListAsync();

            if (tipo.ToLower() == "rubro")
            {
                articulos = articulos
                    .Where(a => a.Rubro.Id_Rubro == id
                        && (a.Precio.Precio != 0 && a.Precio.Precio != null)
                        && a.Activo == true)
                    .ToList();
            }
            else if(tipo.ToLower() == "categoria")
            {
                articulos = articulos
                    .Where(a => a.Articulos_Categorias.Any(ac => ac.Categoria.Id_Categoria == id)
                        && (a.Precio.Precio != 0 && a.Precio.Precio != null)
                        && a.Activo == true)
                    .ToList();
            }

            return View(articulos);
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
