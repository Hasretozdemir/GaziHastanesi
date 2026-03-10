using Microsoft.AspNetCore.Mvc;

namespace GaziHastane.Controllers
{
    public class MailController : Controller
    {
        // Mail Giri� Ekran�
        public IActionResult Giris() { return View(); }

        // Mail Login ��lemi (POST)
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            return RedirectToAction("Giris");
        }
    }
}
//Personelin kurumsal "Gazi Mail" sistemine h�zl�ca giri� yapabilmesi i�in bir k�pr� sayfas�.