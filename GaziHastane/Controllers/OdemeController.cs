using Microsoft.AspNetCore.Mvc;

namespace GaziHastane.Controllers
{
    public class OdemeController : Controller
    {
        // Ödeme Giriþ Ekraný (Borç Sorgulama)
        public IActionResult Giris() { return View(); }

        // Ödeme Login Ýþlemi (POST)
        [HttpPost]
        public IActionResult Login()
        {
            return RedirectToAction("Icerik");
        }

        // Borçlarýn listelendiði ve Kart bilgilerinin girildiði ekran
        public IActionResult Icerik() { return View(); }
    }
}
