using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebHamburgueseria.Models;

namespace WebHamburgueseria.Controllers
{
    public class DetalleVentasController : Controller
    {
        private readonly LabHamburgueseriaContext _context;

        public DetalleVentasController(LabHamburgueseriaContext context)
        {
            _context = context;
        }

        // GET: DetalleVentas
        public async Task<IActionResult> Index()
        {
            var labHamburgueseriaContext = _context.DetalleVentas.Include(d => d.IdProductoNavigation).Include(d => d.IdVentaNavigation);
            return View(await labHamburgueseriaContext.ToListAsync());
        }

        // GET: DetalleVentas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleVentas = await _context.DetalleVentas
                .Include(d => d.IdProductoNavigation)
                .Include(d => d.IdVentaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalleVentas == null)
            {
                return NotFound();
            }

            return View(detalleVentas);
        }

        // GET: DetalleVentas/Create
        public IActionResult Create()
        {
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Id");
            ViewData["IdVenta"] = new SelectList(_context.Ventas, "Id", "Id");
            return View();
        }

        // POST: DetalleVentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdVenta,IdProducto,Cantidad,PrecioUnitario,Total,UsuarioRegistro,FechaRegistro,Estado")] DetalleVentas detalleVentas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalleVentas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Id", detalleVentas.IdProducto);
            ViewData["IdVenta"] = new SelectList(_context.Ventas, "Id", "Id", detalleVentas.IdVenta);
            return View(detalleVentas);
        }

        // GET: DetalleVentas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleVentas = await _context.DetalleVentas.FindAsync(id);
            if (detalleVentas == null)
            {
                return NotFound();
            }
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Id", detalleVentas.IdProducto);
            ViewData["IdVenta"] = new SelectList(_context.Ventas, "Id", "Id", detalleVentas.IdVenta);
            return View(detalleVentas);
        }

        // POST: DetalleVentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdVenta,IdProducto,Cantidad,PrecioUnitario,Total,UsuarioRegistro,FechaRegistro,Estado")] DetalleVentas detalleVentas)
        {
            if (id != detalleVentas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleVentas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleVentasExists(detalleVentas.Id))
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
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Id", detalleVentas.IdProducto);
            ViewData["IdVenta"] = new SelectList(_context.Ventas, "Id", "Id", detalleVentas.IdVenta);
            return View(detalleVentas);
        }

        // GET: DetalleVentas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleVentas = await _context.DetalleVentas
                .Include(d => d.IdProductoNavigation)
                .Include(d => d.IdVentaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalleVentas == null)
            {
                return NotFound();
            }

            return View(detalleVentas);
        }

        // POST: DetalleVentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalleVentas = await _context.DetalleVentas.FindAsync(id);
            if (detalleVentas != null)
            {
                _context.DetalleVentas.Remove(detalleVentas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetalleVentasExists(int id)
        {
            return _context.DetalleVentas.Any(e => e.Id == id);
        }
    }
}
