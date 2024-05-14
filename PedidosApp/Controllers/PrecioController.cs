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
    public class PrecioController : Controller
    {
        private readonly PedidosAppContext _context;

        public PrecioController(PedidosAppContext context)
        {
            _context = context;
        }

        // GET: Precio
        public async Task<IActionResult> Index()
        {
            var pedidosAppContext = _context.Precios;
            return View(await pedidosAppContext.ToListAsync());
        }

        // GET: Precio/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var precioModel = await _context.Precios
                .FirstOrDefaultAsync(m => m.Id_Articulo == id);
            if (precioModel == null)
            {
                return NotFound();
            }

            return View(precioModel);
        }

        // GET: Precio/Create
        public IActionResult Create()
        {
            ViewData["Id_Articulo"] = new SelectList(_context.Articulos, "Id_Articulo", "Id_Articulo");
            return View();
        }

        // POST: Precio/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_Articulo,Precio")] PrecioModel precioModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(precioModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id_Articulo"] = new SelectList(_context.Articulos, "Id_Articulo", "Id_Articulo", precioModel.Id_Articulo);
            return View(precioModel);
        }

        // GET: Precio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var precioModel = await _context.Precios.FindAsync(id);
            if (precioModel == null)
            {
                return NotFound();
            }
            ViewData["Id_Articulo"] = new SelectList(_context.Articulos, "Id_Articulo", "Id_Articulo", precioModel.Id_Articulo);
            return View(precioModel);
        }

        // POST: Precio/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_Articulo,Precio")] PrecioModel precioModel)
        {
            if (id != precioModel.Id_Articulo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(precioModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrecioModelExists(precioModel.Id_Articulo))
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
            ViewData["Id_Articulo"] = new SelectList(_context.Articulos, "Id_Articulo", "Id_Articulo", precioModel.Id_Articulo);
            return View(precioModel);
        }

        // GET: Precio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var precioModel = await _context.Precios
                .FirstOrDefaultAsync(m => m.Id_Articulo == id);
            if (precioModel == null)
            {
                return NotFound();
            }

            return View(precioModel);
        }

        // POST: Precio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var precioModel = await _context.Precios.FindAsync(id);
            if (precioModel != null)
            {
                _context.Precios.Remove(precioModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrecioModelExists(int id)
        {
            return _context.Precios.Any(e => e.Id_Articulo == id);
        }
    }
}