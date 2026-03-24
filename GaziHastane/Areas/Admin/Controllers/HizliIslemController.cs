using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GaziHastane.Areas.Admin.Controllers
{
    // Bu Controller'ın Admin paneline ait olduğunu belirtiyoruz
    [Area("Admin")]
    [Authorize]
    public class HizliIslemController : Controller
    {

        public IActionResult YemekListesi()
        {
            // Yemek listesi güncelleme işlemleri buraya gelecek
            return View();
        }

        public IActionResult Kalite()
        {
            // Laboratuvar ayarları buraya gelecek
            return View();
        }
    }
}