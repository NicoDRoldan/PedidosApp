using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PedidosApp.Data;
using PedidosApp.Models;

namespace PedidosApp.Controllers
{
    [Authorize]
    public class ArticuloController : Controller
    {
        private readonly PedidosAppContext _context;

        public ArticuloController(PedidosAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Articulos.Include(a => a.Precio).Include(a => a.Rubro).ToListAsync());
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

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Nombre,Descripcion,Activo,Id_Rubro,Url_Imagen, Precio")] ArticuloModel articuloModel, IFormFile imagen)
        {
            if(imagen != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imagen.FileName;
                var path = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imagen.CopyToAsync(stream);
                }

                articuloModel.Url_Imagen = "/images/" + uniqueFileName;
            }

            articuloModel.FechaCreacion = DateTime.Now;

            _context.Add(articuloModel);

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

            var articuloModel = await _context.Articulos
                .Include(a => a.Precio)
                .Where(a => a.Id_Articulo == id)
                .FirstOrDefaultAsync();

            if (articuloModel == null)
            {
                return NotFound();
            }
            return View(articuloModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_Articulo,Nombre,Descripcion,Activo,Id_Rubro,Url_Imagen,Precio")]
            ArticuloModel articuloModel, IFormFile imagen)
        {
            if (id != articuloModel.Id_Articulo)
            {
                return NotFound();
            }

            var articuloModelOriginal = await _context.Articulos
                .Include(a => a.Precio)
                .AsNoTracking()
                .Where(a => a.Id_Articulo == id)
                .FirstOrDefaultAsync();

            if(imagen == null)
                articuloModel.Url_Imagen = articuloModelOriginal.Url_Imagen;
            else
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imagen.FileName;
                var path = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imagen.CopyToAsync(stream);
                }

                articuloModel.Url_Imagen = "/images/" + uniqueFileName;
            }

            articuloModel.FechaCreacion = articuloModelOriginal.FechaCreacion;

            if(articuloModel.Precio != null)
            {
                if(articuloModelOriginal.Precio != null)
                {
                    articuloModel.Precio.Id_Articulo = articuloModelOriginal.Precio.Id_Articulo;
                    _context.Entry(articuloModel.Precio).State = EntityState.Modified;
                }
            }
            else if(articuloModel.Precio == null && articuloModelOriginal.Precio != null)
            {
                _context.Entry(articuloModelOriginal.Precio).State = EntityState.Deleted;
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

            var precioModel = await _context.Precios
                .FindAsync(id);

            if (articuloModel != null)
            {
                if(precioModel != null)
                {
                    _context.Precios.Remove(precioModel);
                }

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
