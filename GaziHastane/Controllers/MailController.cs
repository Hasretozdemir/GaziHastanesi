using Microsoft.AspNetCore.Mvc;

namespace GaziHastane.Controllers
{
    public class MailController : Controller
    {
        // Mail Giriþ Ekraný
        public IActionResult Giris() { return View(); }

        // Mail Login Ýþlemi (POST)
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            return RedirectToAction("Giris");
        }
    }
}
//Personelin kurumsal "Gazi Mail" sistemine hýzlýca giriþ yapabilmesi için bir köprü sayfasý.