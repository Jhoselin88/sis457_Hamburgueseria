using Microsoft.AspNetCore.Mvc;

namespace WebHamburgueseria.Controllers
{
    public class PagesController : Controller
    {
        // GET: Pages/About
        public IActionResult About()
        {
            ViewData["Title"] = "Acerca de";
            ViewData["BodyClass"] = "sub_page";
            return View();
        }

        // GET: Pages/Book
        public IActionResult Book()
        {
            ViewData["Title"] = "Reservas";
            ViewData["BodyClass"] = "sub_page";
            return View();
        }
    }
}