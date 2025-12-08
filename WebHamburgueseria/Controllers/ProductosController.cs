using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using WebHamburgueseria.Models;

namespace WebHamburgueseria.Controllers
{
    public class ProductosController : Controller
    {
        private readonly LabHamburgueseriaContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductosController(LabHamburgueseriaContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Productos
        public async Task<IActionResult> Index(string search, int? categoria)
        {
            ViewBag.Categorias = await _context.Categoria
                .Where(c => c.Estado == 1)
                .Select(c => new { c.Id, c.Nombre })
                .ToListAsync();

            var productos = _context.Producto
                .Include(p => p.IdCategoriaNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                productos = productos.Where(p =>
                    p.Codigo.Contains(search) ||
                    p.Nombre.Contains(search) ||
                    (p.Descripcion != null && p.Descripcion.Contains(search)));
            }

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
        public async Task<IActionResult> Create([Bind("Id,IdCategoria,Codigo,Nombre,Descripcion,Saldo,PrecioVenta,UsuarioRegistro,FechaRegistro,Estado,ImagenFile")] Producto producto)
        {
            ModelState.Remove("IdCategoriaNavigation");
            ModelState.Remove("DetalleVentas");
            ModelState.Remove("RutaImagen");

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new {
                        Field = x.Key,
                        Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    })
                    .ToList();

                foreach (var error in errors)
                {
                    Console.WriteLine($"Campo: {error.Field}");
                    foreach (var msg in error.Errors)
                    {
                        Console.WriteLine($"  Error: {msg}");
                    }
                }

                ViewBag.ValidationErrors = errors;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Procesar la imagen si fue subida
                    if (producto.ImagenFile != null && producto.ImagenFile.Length > 0)
                    {
                        producto.RutaImagen = await GuardarImagen(producto.ImagenFile);
                    }

                    _context.Add(producto);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Producto creado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al guardar: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }

                    ModelState.AddModelError("", "Error al guardar el producto: " + ex.Message);
                }
            }

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
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdCategoria,Codigo,Nombre,Descripcion,Saldo,PrecioVenta,UsuarioRegistro,FechaRegistro,Estado,RutaImagen,ImagenFile")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            ModelState.Remove("IdCategoriaNavigation");
            ModelState.Remove("DetalleVentas");

            if (ModelState.IsValid)
            {
                try
                {
                    // Si se subió una nueva imagen
                    if (producto.ImagenFile != null && producto.ImagenFile.Length > 0)
                    {
                        // Eliminar imagen anterior si existe
                        if (!string.IsNullOrEmpty(producto.RutaImagen))
                        {
                            EliminarImagen(producto.RutaImagen);
                        }

                        // Guardar nueva imagen
                        producto.RutaImagen = await GuardarImagen(producto.ImagenFile);
                    }

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
                    // Eliminar imagen si existe
                    if (!string.IsNullOrEmpty(producto.RutaImagen))
                    {
                        EliminarImagen(producto.RutaImagen);
                    }

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

        // MÉTODOS AUXILIARES PARA MANEJAR IMÁGENES

        private async Task<string> GuardarImagen(IFormFile imagen)
        {
            // Crear carpeta si no existe
            string carpetaProductos = Path.Combine(_webHostEnvironment.WebRootPath, "images", "productos");
            if (!Directory.Exists(carpetaProductos))
            {
                Directory.CreateDirectory(carpetaProductos);
            }

            // Generar nombre único para la imagen
            string extension = Path.GetExtension(imagen.FileName);
            string nombreArchivo = $"{Guid.NewGuid()}{extension}";
            string rutaCompleta = Path.Combine(carpetaProductos, nombreArchivo);

            // Guardar archivo
            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                await imagen.CopyToAsync(stream);
            }

            // Retornar la ruta relativa
            return $"/images/productos/{nombreArchivo}";
        }

        private void EliminarImagen(string rutaImagen)
        {
            if (string.IsNullOrEmpty(rutaImagen))
                return;

            string rutaCompleta = Path.Combine(_webHostEnvironment.WebRootPath, rutaImagen.TrimStart('/'));

            if (System.IO.File.Exists(rutaCompleta))
            {
                try
                {
                    System.IO.File.Delete(rutaCompleta);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al eliminar imagen: {ex.Message}");
                }
            }
        }
    }
}