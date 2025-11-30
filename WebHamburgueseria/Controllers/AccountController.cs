using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using WebHamburgueseria.Models;
using WebHamburgueseria.Utils;

namespace WebHamburgueseria.Controllers
{
    public class AccountController : Controller
    {
        private readonly LabHamburgueseriaContext _context;

        public AccountController(LabHamburgueseriaContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.usuario = model.usuario?.Trim() ?? string.Empty;
            model.clave = model.clave?.Trim() ?? string.Empty;

            // Encriptar la contraseña
            var claveEncriptada = Util.Encrypt(model.clave);

            System.Diagnostics.Debug.WriteLine("=== DEBUG LOGIN ===");
            System.Diagnostics.Debug.WriteLine($"Usuario: [{model.usuario}]");
            System.Diagnostics.Debug.WriteLine($"Clave: [{model.clave}]");
            System.Diagnostics.Debug.WriteLine($"Hash generado: [{claveEncriptada}]");
            System.Diagnostics.Debug.WriteLine($"Longitud hash: {claveEncriptada.Length}");

            // Buscar usuario en la base de datos
            var usuario = await _context.Usuario
                .Include(u => u.IdEmpleadoNavigation)
                .FirstOrDefaultAsync(u =>
                    u.Usuario1 == model.usuario &&
                    u.Clave == claveEncriptada &&
                    u.Estado == 1);

            System.Diagnostics.Debug.WriteLine($"Usuario encontrado: {(usuario != null ? "SÍ ✓" : "NO ✗")}");
            System.Diagnostics.Debug.WriteLine("==================");

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos");
                return View(model);
            }

            // Crear claims para la sesión
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Usuario1),
                new Claim("FullName", $"{usuario.IdEmpleadoNavigation.Nombres} {usuario.IdEmpleadoNavigation.PrimerApellido}".Trim())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.recordarme,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Redireccionar
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // Modelo de vista para el login
        public class LoginViewModel
        {
            [Required(ErrorMessage = "El usuario es obligatorio")]
            [Display(Name = "Usuario")]
            public string usuario { get; set; } = string.Empty;

            [Required(ErrorMessage = "La contraseña es obligatoria")]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string clave { get; set; } = string.Empty;

            [Display(Name = "Recordarme")]
            public bool recordarme { get; set; }
        }
    }
}