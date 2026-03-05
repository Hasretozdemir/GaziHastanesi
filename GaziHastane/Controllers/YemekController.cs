using Microsoft.AspNetCore.Mvc;

namespace GaziHastane.Controllers
{
    public class YemekController : Controller
    {
        public IActionResult Liste() { return View(); }
        public IActionResult AylikListe() { return View(); }
    }
}
