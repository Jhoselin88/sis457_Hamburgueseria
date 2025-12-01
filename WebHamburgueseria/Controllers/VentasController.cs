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
            var labHamburgueseriaContext = _context.Ventas.Include(v => v.IdClienteNavigation).Include(v => v.IdUsuarioNavigation);
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ventas == null)
            {
                return NotFound();
            }

            return View(ventas);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            // Cargar clientes con sus nombres completos
            ViewData["IdCliente"] = new SelectList(
                _context.Cliente.Select(c => new
                {
                    c.Id,
                    NombreCompleto = c.Nombres + " - CI: " + c.CedulaIdentidad
                }),
                "Id",
                "NombreCompleto"
            );

            // Cargar usuarios
            ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Usuario1");

            // Cargar productos con nombre y precio - ASEGURARSE DE QUE PrecioVenta SEA DECIMAL
            var productos = _context.Producto
                .Where(p => p.Estado == 1) // Solo productos activos
                .Select(p => new
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    PrecioVenta = p.PrecioVenta
                })
                .ToList();

            ViewBag.Productos = productos;

            // Para depuración - puedes eliminar esto después
            foreach (var prod in productos)
            {
                Console.WriteLine($"Producto: {prod.Nombre}, Precio: {prod.PrecioVenta}");
            }

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
                // Deserializar los detalles de la venta
                var detallesVenta = JsonSerializer.Deserialize<List<DetalleVentaDto>>(DetallesVentaJson);

                if (detallesVenta == null || !detallesVenta.Any())
                {
                    ModelState.AddModelError("", "Debe agregar al menos un producto a la venta");
                    // Recargar datos para la vista
                    ViewData["IdCliente"] = new SelectList(_context.Cliente, "Id", "Nombres", ventas.IdCliente);
                    ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Usuario1", ventas.IdUsuario);
                    ViewBag.Productos = _context.Producto.Where(p => p.Estado == 1).ToList();
                    return View(ventas);
                }

                // Calcular el total de la venta
                decimal totalVenta = detallesVenta.Sum(d => d.Subtotal);

                // Generar número de transacción si es TXN-AUTO
                if (ventas.NumeroTransaccion == "TXN-AUTO")
                {
                    ventas.NumeroTransaccion = "TXN-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                }

                // Guardar la venta
                _context.Add(ventas);
                await _context.SaveChangesAsync();

                // Guardar los detalles de la venta
                foreach (var detalle in detallesVenta)
                {
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
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Venta registrada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al guardar la venta: " + ex.Message);

                // Recargar datos para la vista
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

            var ventas = await _context.Ventas.FindAsync(id);
            if (ventas == null)
            {
                return NotFound();
            }
            ViewData["IdCliente"] = new SelectList(_context.Cliente, "Id", "Id", ventas.IdCliente);
            ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Id", ventas.IdUsuario);
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ventas);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCliente"] = new SelectList(_context.Cliente, "Id", "Id", ventas.IdCliente);
            ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Id", ventas.IdUsuario);
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
            var ventas = await _context.Ventas.FindAsync(id);
            if (ventas != null)
            {
                _context.Ventas.Remove(ventas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentasExists(int id)
        {
            return _context.Ventas.Any(e => e.Id == id);
        }
    }
}