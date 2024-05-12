using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PedidosApp.Data;
using PedidosApp.Models;

namespace PedidosApp.Controllers
{
    public class ArticuloController : Controller
    {
        private readonly PedidosAppContext _context;

        public ArticuloController(PedidosAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Articulos.Include(a => a.Precio).Where(a => a.Precio.Precio != null && a.Precio.Precio != 0).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articuloModel = await _context.Articulos
                .FirstOrDefaultAsync(m => m.Id_Articulo == id);
            if (articuloModel == null)
            {
                return NotFound();
            }

            return View(articuloModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Descripcion,Activo,FechaCreacion,Id_Rubro, Url_Imagen")] ArticuloModel articuloModel)
        {
            _context.Add(articuloModel);

            Console.WriteLine(articuloModel.Id_Articulo);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Articulo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articuloModel = await _context.Articulos.FindAsync(id);
            if (articuloModel == null)
            {
                return NotFound();
            }
            return View(articuloModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_Articulo,Nombre,Descripcion,Activo,FechaCreacion,Id_Rubro")] ArticuloModel articuloModel)
        {
            if (id != articuloModel.Id_Articulo)
            {
                return NotFound();
            }

            try
            {
                _context.Update(articuloModel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticuloModelExists(articuloModel.Id_Articulo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articuloModel = await _context.Articulos
                .FirstOrDefaultAsync(m => m.Id_Articulo == id);
            if (articuloModel == null)
            {
                return NotFound();
            }

            return View(articuloModel);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articuloModel = await _context.Articulos.FindAsync(id);
            if (articuloModel != null)
            {
                _context.Articulos.Remove(articuloModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticuloModelExists(int id)
        {
            return _context.Articulos.Any(e => e.Id_Articulo == id);
        }
    }
}
