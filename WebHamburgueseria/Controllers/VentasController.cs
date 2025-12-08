using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebHamburgueseria.Models;
using System.Text.Json;

namespace WebHamburgueseria.Controllers
{
    public class VentasController : Controller
    {
        private readonly LabHamburgueseriaContext _context;

        public VentasController(LabHamburgueseriaContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            var labHamburgueseriaContext = _context.Ventas
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.IdUsuarioNavigation);
            return View(await labHamburgueseriaContext.ToListAsync());
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await _context.Ventas
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.IdUsuarioNavigation)
                .Include(v => v.DetalleVentas)
                    .ThenInclude(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ventas == null)
            {
                return NotFound();
            }

            return View(ventas);
        }

        // Reemplazá el método Create (GET) en VentasController.cs con este código:

        // GET: Ventas/Create
        public IActionResult Create()
        {
            // Cargar clientes con JSON para búsqueda
            var clientes = _context.Cliente
                .Where(c => c.Estado == 1)
                .Select(c => new
                {
                    c.Id,
                    c.Nombres,
                    c.Apellidos,
                    c.CedulaIdentidad
                })
                .ToList();
            ViewBag.ClientesJson = JsonSerializer.Serialize(clientes);

            // Cargar usuarios
            ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Usuario1");

            // ACTUALIZADO: Cargar productos con nombre, precio, saldo e IMAGEN
            var productos = _context.Producto
                .Where(p => p.Estado == 1)
                .Select(p => new
                {
                    p.Id,
                    p.Nombre,
                    p.PrecioVenta,
                    p.Saldo,
                    p.RutaImagen  // NUEVO: Incluir la ruta de la imagen
                })
                .ToList();
            ViewBag.ProductosJson = JsonSerializer.Serialize(productos);
            ViewBag.Productos = productos;

            return View();
        }

        // Clase auxiliar para recibir los detalles de la venta desde el JSON
        public class DetalleVentaDto
        {
            public int IdProducto { get; set; }
            public int Cantidad { get; set; }
            public decimal PrecioUnitario { get; set; }
            public decimal Subtotal { get; set; }
        }

        // POST: Ventas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUsuario,IdCliente,NumeroTransaccion,UsuarioRegistro,FechaRegistro,Estado")] Ventas ventas, string DetallesVentaJson)
        {
            try
            {
                var detallesVenta = JsonSerializer.Deserialize<List<DetalleVentaDto>>(DetallesVentaJson);

                if (detallesVenta == null || !detallesVenta.Any())
                {
                    ModelState.AddModelError("", "Debe agregar al menos un producto a la venta");
                    ViewData["IdCliente"] = new SelectList(_context.Cliente, "Id", "Nombres", ventas.IdCliente);
                    ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Usuario1", ventas.IdUsuario);
                    ViewBag.Productos = _context.Producto.Where(p => p.Estado == 1).ToList();
                    return View(ventas);
                }

                // VALIDAR SALDO (STOCK) ANTES DE PROCESAR LA VENTA
                foreach (var detalle in detallesVenta)
                {
                    var producto = await _context.Producto.FindAsync(detalle.IdProducto);
                    if (producto == null)
                    {
                        ModelState.AddModelError("", $"El producto con ID {detalle.IdProducto} no existe");
                        ViewData["IdCliente"] = new SelectList(_context.Cliente, "Id", "Nombres", ventas.IdCliente);
                        ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Usuario1", ventas.IdUsuario);
                        ViewBag.Productos = _context.Producto.Where(p => p.Estado == 1).ToList();
                        return View(ventas);
                    }

                    if (producto.Saldo < detalle.Cantidad)
                    {
                        ModelState.AddModelError("", $"Stock insuficiente para el producto '{producto.Nombre}'. Stock disponible: {producto.Saldo}, Cantidad solicitada: {detalle.Cantidad}");
                        ViewData["IdCliente"] = new SelectList(_context.Cliente, "Id", "Nombres", ventas.IdCliente);
                        ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Usuario1", ventas.IdUsuario);
                        ViewBag.Productos = _context.Producto.Where(p => p.Estado == 1).ToList();
                        return View(ventas);
                    }
                }

                decimal totalVenta = detallesVenta.Sum(d => d.Subtotal);

                if (ventas.NumeroTransaccion == "TXN-AUTO")
                {
                    ventas.NumeroTransaccion = "TXN-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                }

                _context.Add(ventas);
                await _context.SaveChangesAsync();

                // GUARDAR DETALLES Y DESCONTAR SALDO (STOCK)
                foreach (var detalle in detallesVenta)
                {
                    // Crear detalle de venta
                    var detalleVenta = new DetalleVentas
                    {
                        IdVenta = ventas.Id,
                        IdProducto = detalle.IdProducto,
                        Cantidad = detalle.Cantidad,
                        PrecioUnitario = detalle.PrecioUnitario,
                        Total = detalle.Subtotal,
                        UsuarioRegistro = "Admin",
                        FechaRegistro = DateTime.Now,
                        Estado = 1
                    };
                    _context.DetalleVentas.Add(detalleVenta);

                    // DESCONTAR SALDO (STOCK) DEL PRODUCTO
                    var producto = await _context.Producto.FindAsync(detalle.IdProducto);
                    producto.Saldo -= detalle.Cantidad;
                    _context.Update(producto);
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Venta registrada exitosamente y stock actualizado";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al guardar la venta: " + ex.Message);

                ViewData["IdCliente"] = new SelectList(_context.Cliente, "Id", "Nombres", ventas.IdCliente);
                ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Usuario1", ventas.IdUsuario);
                ViewBag.Productos = _context.Producto.Where(p => p.Estado == 1).ToList();
                return View(ventas);
            }
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await _context.Ventas
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);

            if (ventas == null)
            {
                return NotFound();
            }

            // Cargar clientes con nombres completos
            ViewData["IdCliente"] = new SelectList(
                _context.Cliente.Select(c => new
                {
                    c.Id,
                    NombreCompleto = c.Nombres + " - CI: " + c.CedulaIdentidad
                }),
                "Id",
                "NombreCompleto",
                ventas.IdCliente
            );

            // Cargar usuarios
            ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Usuario1", ventas.IdUsuario);

            return View(ventas);
        }

        // POST: Ventas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUsuario,IdCliente,NumeroTransaccion,UsuarioRegistro,FechaRegistro,Estado")] Ventas ventas)
        {
            if (id != ventas.Id)
            {
                return NotFound();
            }

            // Remover validaciones de propiedades de navegación
            ModelState.Remove("IdClienteNavigation");
            ModelState.Remove("IdUsuarioNavigation");
            ModelState.Remove("DetalleVentas");

            if (ModelState.IsValid)
            {
                try
                {
                    // Método 1: Attach y marcar como modificado
                    _context.Attach(ventas);
                    _context.Entry(ventas).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Venta actualizada exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentasExists(ventas.Id))
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
                    ModelState.AddModelError("", $"Error al actualizar: {ex.Message}");
                }
            }

            // Si hay error, recargar los datos
            ViewData["IdCliente"] = new SelectList(
                _context.Cliente.Select(c => new
                {
                    c.Id,
                    NombreCompleto = c.Nombres + " - CI: " + c.CedulaIdentidad
                }),
                "Id",
                "NombreCompleto",
                ventas.IdCliente
            );
            ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Usuario1", ventas.IdUsuario);

            return View(ventas);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await _context.Ventas
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ventas == null)
            {
                return NotFound();
            }

            return View(ventas);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var ventas = await _context.Ventas
                    .Include(v => v.DetalleVentas)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (ventas != null)
                {
                    // DEVOLVER EL SALDO (STOCK) ANTES DE ELIMINAR
                    foreach (var detalle in ventas.DetalleVentas)
                    {
                        var producto = await _context.Producto.FindAsync(detalle.IdProducto);
                        if (producto != null)
                        {
                            producto.Saldo += detalle.Cantidad; // Devolver saldo (stock)
                            _context.Update(producto);
                        }
                    }

                    // Eliminar los detalles de venta
                    _context.DetalleVentas.RemoveRange(ventas.DetalleVentas);

                    // Eliminar la venta
                    _context.Ventas.Remove(ventas);

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Venta eliminada exitosamente y stock devuelto";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar la venta: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        private bool VentasExists(int id)
        {
            return _context.Ventas.Any(e => e.Id == id);
        }
    }
}