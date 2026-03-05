using Microsoft.AspNetCore.Mvc;

namespace GaziHastane.Controllers
{
    public class SonucController : Controller
    {
        // Sonuç Sorgulama Giriþ Ekraný
        public IActionResult Giris() { return View(); }

        // Sonuç Login Ýþlemi (POST)
        [HttpPost]
        public IActionResult Login()
        {
            return RedirectToAction("Panel");
        }

        // E-Nabýz stili sonuç listesi ekraný
        public IActionResult Panel() { return View(); }
    }
}
