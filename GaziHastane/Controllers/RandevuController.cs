using Microsoft.AspNetCore.Mvc;

namespace GaziHastane.Controllers
{
    public class RandevuController : Controller
    {
        // Giriþ Ekraný
        public IActionResult Giris() { return View(); }

        // Giriþ yapýldýktan sonra açýlan seçim ekraný
        public IActionResult Secim() { return View(); }

        // Login Ýþlemi (POST)
        [HttpPost]
        public IActionResult Login(string idInput)
        {
            return RedirectToAction("Secim");
        }
    }
}
//Hastalarýn T.C. kimlik numaralarýyla sisteme girip uygun poliklinik ve doktordan randevu seçebilmesi için tasarlanmýþ.