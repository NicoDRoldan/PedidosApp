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

        private async Task SetViewBagsRubrosCategorias()
        {
            ViewBag.RubrosActivos = await _context.Rubros
                .Join(_context.Articulos,
                    ar => ar.Id_Rubro,
                    a => a.Id_Rubro,
                    (ar, a) => new { Rubro = ar, Articulo = a })
                .Join(_context.Precios,
                    arp => arp.Articulo.Id_Articulo,
                    p => p.Id_Articulo,
                    (arp, p) => new { JoinArticuloPrecio = arp, Precio = p })
                .Where(p => p.Precio.Precio != null && p.Precio.Precio != 0)
                .Select(result => result.JoinArticuloPrecio.Rubro)
                .Distinct()
                .ToListAsync();

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
        }

        public async Task<IActionResult> Index()
        {
            await SetViewBagsRubrosCategorias();

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

                ViewBag.RubroCategoriaActivos = await _context.Rubros
                    .Join(_context.Articulos,
                        ar => ar.Id_Rubro,
                        a => a.Id_Rubro,
                        (ar, a) => new { Rubro = ar, Articulo = a })
                    .Join(_context.Precios,
                        arp => arp.Articulo.Id_Articulo,
                        p => p.Id_Articulo,
                        (arp, p) => new { JoinArticuloPrecio = arp, Precio = p })
                    .Where(rp => rp.Precio.Precio != null && rp.Precio.Precio != 0
                        && rp.JoinArticuloPrecio.Rubro.Id_Rubro == id)
                    .Select(result => result.JoinArticuloPrecio.Rubro)
                    .Distinct()
                    .ToListAsync();
            }
            else if(tipo.ToLower() == "categoria")
            {
                articulos = articulos
                    .Where(a => a.Articulos_Categorias.Any(ac => ac.Categoria.Id_Categoria == id)
                        && (a.Precio.Precio != 0 && a.Precio.Precio != null)
                        && a.Activo == true)
                    .ToList();

                ViewBag.RubroCategoriaActivos = await _context
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
                    .Where(cap => cap.Precio.Precio != null && cap.Precio.Precio != 0
                        && cap.JoinArticuloPrecio.JoinArticuloCategoria.Categoria.Id_Categoria == id)
                    .Select(result => result.JoinArticuloPrecio.JoinArticuloCategoria.Categoria)
                    .Distinct()
                    .ToListAsync();
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
