using Microsoft.AspNetCore.Mvc;

namespace GaziHastane.Controllers
{
    public class EBYSController : Controller
    {
        public IActionResult Giris() { return View(); }

        [HttpPost]
        public IActionResult Login()
        {
            return RedirectToAction("Giris");
        }
    }
}
