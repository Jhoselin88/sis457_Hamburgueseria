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
    public class ClientesController : Controller
    {
        private readonly LabHamburgueseriaContext _context;

        public ClientesController(LabHamburgueseriaContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cliente.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CedulaIdentidad,Nombres,Apellidos,Estado")] Cliente cliente)
        {
            // Establecer automáticamente el usuario y fecha de registro
            cliente.UsuarioRegistro = User.Identity?.Name ?? "Admin";
            cliente.FechaRegistro = DateTime.Now;
            cliente.Estado = 1; // Asegurar que el estado sea activo

            // Remover validaciones de campos que no vienen del formulario
            ModelState.Remove("UsuarioRegistro");
            ModelState.Remove("FechaRegistro");
            ModelState.Remove("Ventas");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(cliente);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Cliente creado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al guardar el cliente: {ex.Message}");
                }
            }

            return View(cliente);
        }

        // NUEVO: API para crear cliente desde modal (AJAX)
        [HttpPost]
        public async Task<IActionResult> CrearClienteAjax([FromBody] Cliente cliente)
        {
            try
            {
                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(cliente.CedulaIdentidad))
                {
                    return Json(new { success = false, message = "La Cédula de Identidad es obligatoria" });
                }

                if (string.IsNullOrWhiteSpace(cliente.Nombres))
                {
                    return Json(new { success = false, message = "El nombre es obligatorio" });
                }

                if (string.IsNullOrWhiteSpace(cliente.Apellidos))
                {
                    return Json(new { success = false, message = "Los apellidos son obligatorios" });
                }

                // Verificar si ya existe un cliente con esa CI
                var clienteExiste = await _context.Cliente
                    .AnyAsync(c => c.CedulaIdentidad == cliente.CedulaIdentidad && c.Estado == 1);

                if (clienteExiste)
                {
                    return Json(new { success = false, message = "Ya existe un cliente con esa Cédula de Identidad" });
                }

                // Establecer valores por defecto
                cliente.FechaRegistro = DateTime.Now;
                cliente.Estado = 1;
                cliente.UsuarioRegistro = User.Identity?.Name ?? "Admin";

                // Guardar cliente
                _context.Add(cliente);
                await _context.SaveChangesAsync();

                // Retornar el cliente creado
                return Json(new
                {
                    success = true,
                    message = "Cliente creado exitosamente",
                    cliente = new
                    {
                        cliente.Id,
                        cliente.CedulaIdentidad,
                        cliente.Nombres,
                        cliente.Apellidos
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al crear el cliente: " + ex.Message });
            }
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CedulaIdentidad,Nombres,Apellidos,UsuarioRegistro,FechaRegistro,Estado")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Ventas");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Cliente actualizado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
                    ModelState.AddModelError("", $"Error al actualizar el cliente: {ex.Message}");
                }
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var cliente = await _context.Cliente.FindAsync(id);
                if (cliente != null)
                {
                    _context.Cliente.Remove(cliente);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Cliente eliminado exitosamente";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar el cliente. Puede que esté siendo usado en ventas.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Cliente.Any(e => e.Id == id);
        }
    }
}