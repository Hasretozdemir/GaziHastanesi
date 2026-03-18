using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GaziHastane.Areas.Admin.Controllers
{
    // Bu Controller'ın Admin paneline ait olduğunu belirtiyoruz
    [Area("Admin")]
    [Authorize]
    public class HizliIslemController : Controller
    {
        // 1. Gazi Mail Sayfası
        public IActionResult GaziMail()
        {
            return View(); // Views/HizliIslem/GaziMail.cshtml dosyasını açar
        }

        // 2. EBYS Sayfası
        public IActionResult Ebys()
        {
            return View(); // Views/HizliIslem/Ebys.cshtml dosyasını açar
        }

        // --- İleride içlerini dolduracağın diğer Hızlı İşlem sayfaları ---

        public IActionResult Randevu()
        {
            // Randevu paneli ayarları buraya gelecek
            return View();
        }

        public IActionResult Laboratuvar()
        {
            // Laboratuvar ayarları buraya gelecek
            return View();
        }

        public IActionResult Odeme()
        {
            // Ödeme sistemi ayarları buraya gelecek
            return View();
        }

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