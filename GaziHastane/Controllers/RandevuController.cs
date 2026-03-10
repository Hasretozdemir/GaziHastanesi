using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GaziHastane.Controllers
{
    public class RandevuController : Controller
    {
        public IActionResult Giris()
        {
            var randevular = _context.Randevular.ToList();
            return View(randevular);
        }

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