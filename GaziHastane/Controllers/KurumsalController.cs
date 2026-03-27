using Microsoft.AspNetCore.Mvc;

namespace GaziHastane.Controllers
{
    public class KurumsalController : Controller
    {
        // Hakkýmýzda (Kurumsal Ana Sayfa)
        public IActionResult Index() { return View(); }

        // Baþhekimlik
        public IActionResult Bashekimlik() { return View(); }

        // Baþmüdürlük
        public IActionResult Basmudurluk() { return View(); }

        // Hemþirelik Hizmetleri
        public IActionResult HemsirelikHizmetleri() { return View(); }

        // Bilgi Ýþlem Merkezi
        public IActionResult BilgiIslem() { return View(); }

        // Ýþ Saðlýðý ve Güvenliði
        public IActionResult IsSagligi() { return View(); }

        // Enfeksiyon Kontrol
        public IActionResult Enfeksiyon() { return View(); }

        // Eczacýlýk Hizmetleri
        public IActionResult Eczacilik() { return View(); }

        // Satýn Alma
        public IActionResult SatinAlma() { return View(); }

        // Ýstatistik ve Raporlama
        public IActionResult Istatistik() { return View(); }

        // Arþiv Birimi
        public IActionResult Arsiv() { return View(); }

        // Hasta Ýletiþim Birimi
        public IActionResult HastaIletisim() { return View(); }

        // Ýþ Akýþ Þemalarý
        public IActionResult IsAkis() { return View(); }

        // Organizasyon Þemalarý
        public IActionResult Organizasyon() { return View(); }

        // Ýç Kontrol
        public IActionResult IcKontrol()
        {
            // Explicitly return the view by full path to avoid lookup issues
            return View("~/Views/Kurumsal/IcKontrol.cshtml");
        }

        // Basýn ve Kurumsal Ýletiþim
        public IActionResult BasinIletisim() { return View(); }
    }
}