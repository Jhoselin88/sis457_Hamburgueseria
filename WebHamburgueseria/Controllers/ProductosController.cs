using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebHamburgueseria.Models;

namespace WebHamburgueseria.Controllers
{
    public class ProductosController : Controller
    {
        private readonly LabHamburgueseriaContext _context;

        public ProductosController(LabHamburgueseriaContext context)
        {
            _context = context;
        }

        // GET: Productos
        public async Task<IActionResult> Index(string search, int? categoria)
        {
            // Cargar categorías para el dropdown
            ViewBag.Categorias = await _context.Categoria
                .Where(c => c.Estado == 1)
                .Select(c => new { c.Id, c.Nombre })
                .ToListAsync();

            // Consulta base
            var productos = _context.Producto
                .Include(p => p.IdCategoriaNavigation)
                .AsQueryable();

            // Filtrar por búsqueda
            if (!string.IsNullOrEmpty(search))
            {
                productos = productos.Where(p =>
                    p.Codigo.Contains(search) ||
                    p.Nombre.Contains(search) ||
                    (p.Descripcion != null && p.Descripcion.Contains(search)));
            }

            // Filtrar por categoría
            if (categoria.HasValue)
            {
                productos = productos.Where(p => p.IdCategoria == categoria.Value);
            }

            return View(await productos.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .Include(p => p.IdCategoriaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            // Cargar categorías activas con Id y Nombre
            ViewData["IdCategoria"] = new SelectList(
                _context.Categoria.Where(c => c.Estado == 1),
                "Id",
                "Nombre"
            );

            return View();
        }

        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCategoria,Codigo,Nombre,Descripcion,Saldo,PrecioVenta,UsuarioRegistro,FechaRegistro,Estado")] Producto producto)
        {
            // SOLUCIÓN CRÍTICA: Remover la validación de propiedades de navegación
            ModelState.Remove("IdCategoriaNavigation");
            ModelState.Remove("DetalleVentas");

            // DEBUGGING: Ver qué errores tiene el ModelState
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new {
                        Field = x.Key,
                        Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    })
                    .ToList();

                // Esto mostrará los errores en la consola de Visual Studio
                foreach (var error in errors)
                {
                    Console.WriteLine($"Campo: {error.Field}");
                    foreach (var msg in error.Errors)
                    {
                        Console.WriteLine($"  Error: {msg}");
                    }
                }

                // También podemos pasar los errores a la vista
                ViewBag.ValidationErrors = errors;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(producto);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Producto creado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Capturar cualquier error de base de datos
                    Console.WriteLine($"Error al guardar: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }

                    ModelState.AddModelError("", "Error al guardar el producto: " + ex.Message);
                }
            }

            // Recargar categorías si hay error
            ViewData["IdCategoria"] = new SelectList(
                _context.Categoria.Where(c => c.Estado == 1),
                "Id",
                "Nombre",
                producto.IdCategoria
            );

            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            // Cargar categorías con Id y Nombre
            ViewData["IdCategoria"] = new SelectList(
                _context.Categoria.Where(c => c.Estado == 1),
                "Id",
                "Nombre",
                producto.IdCategoria
            );

            return View(producto);
        }

        // POST: Productos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdCategoria,Codigo,Nombre,Descripcion,Saldo,PrecioVenta,UsuarioRegistro,FechaRegistro,Estado")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            // SOLUCIÓN: Remover validación de propiedades de navegación
            ModelState.Remove("IdCategoriaNavigation");
            ModelState.Remove("DetalleVentas");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Producto actualizado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al actualizar: {ex.Message}");
                    ModelState.AddModelError("", "Error al actualizar el producto: " + ex.Message);
                }
            }

            // Recargar categorías si hay error
            ViewData["IdCategoria"] = new SelectList(
                _context.Categoria.Where(c => c.Estado == 1),
                "Id",
                "Nombre",
                producto.IdCategoria
            );

            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Incluir la navegación de categoría
            var producto = await _context.Producto
                .Include(p => p.IdCategoriaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var producto = await _context.Producto.FindAsync(id);
                if (producto != null)
                {
                    _context.Producto.Remove(producto);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Producto eliminado exitosamente";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar: {ex.Message}");
                TempData["ErrorMessage"] = "Error al eliminar el producto. Puede que esté siendo usado en ventas.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Producto.Any(e => e.Id == id);
        }
    }
}