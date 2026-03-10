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
//Elektronik Belge Y�netim Sistemi. Hastane i�indeki resmi evrak trafi�inin ve yaz��malar�n dijital olarak y�r�t�lece�i mod�l.