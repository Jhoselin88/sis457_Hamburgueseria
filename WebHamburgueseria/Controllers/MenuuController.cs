using Microsoft.AspNetCore.Mvc;

namespace WebHamburgueseria.Controllers
{
    public class MenuuController : Controller
    {
        // GET: Menuu/Index
        public IActionResult Index()
        {
            ViewData["Title"] = "Menu";
            ViewData["BodyClass"] = "sub_page";
            return View();
        }
    }
}