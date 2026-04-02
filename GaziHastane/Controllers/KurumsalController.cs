using GaziHastane.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GaziHastane.Controllers
{
    public class KurumsalController : Controller
    {
        private readonly Data.GaziHastaneContext _context;

        public KurumsalController(Data.GaziHastaneContext context)
        {
            _context = context;
        }

        // Hakkýmýzda (Kurumsal Ana Sayfa)
        public IActionResult Index() { return View(); }

        // Baþhekimlik
        public IActionResult Bashekimlik()
        {
            // Veritabanýndan aktif personelleri sýrasýna göre çekiyoruz
            var aktifPersoneller = _context.BashekimlikPersoneller
                                           .Where(x => x.AktifMi)
                                           .OrderBy(x => x.Sira)
                                           .ToList();

            // Verileri ViewModel'e dolduruyoruz
            var viewModel = new BashekimlikViewModel
            {
                // IsBashekim = true olan ÝLK kaydý Baþhekim olarak al
                Bashekim = aktifPersoneller.FirstOrDefault(x => x.IsBashekim),

                // IsBashekim = false olanlarý Yardýmcýlar listesine al
                Yardimcilar = aktifPersoneller.Where(x => !x.IsBashekim).ToList(),

                // Ýletiþim bilgilerini burada tanýmlýyoruz
                Telefon = "(0312) 202 40 00",
                CalismaSaatleri = "Pzt–Cuma · 08:30 – 17:00"
            };

            return View(viewModel);
        }

        // Baþmüdürlük
        public IActionResult Basmudurluk()
        {
            var aktifPersoneller = _context.BasmudurlikPersoneller
                                           .Where(x => x.AktifMi)
                                           .OrderBy(x => x.Sira)
                                           .ToList();

            var viewModel = new BasmudurlikViewModel
            {
                Basmudur = aktifPersoneller.FirstOrDefault(x => x.IsBasmudur),
                Yardimcilar = aktifPersoneller.Where(x => !x.IsBasmudur).ToList(),
                Telefon = "(0312) 202 40 00",
                CalismaSaatleri = "Pzt–Cuma · 08:30 – 17:00"
            };

            return View(viewModel);
        }

        // Hemþirelik Hizmetleri (DÝNAMÝK HALE GETÝRÝLDÝ)
        public IActionResult HemsirelikHizmetleri()
        {
            // Aktif olan tüm içerikleri sýrasýna göre tek seferde çekiyoruz
            var tumIcerikler = _context.HemsirelikIcerikler
                                       .Where(x => x.AktifMi)
                                       .OrderBy(x => x.Sira)
                                       .ToList();

            var sekmeler = _context.HemsirelikSekmeler
                .Where(x => x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToList();

            var viewModel = new HemsirelikViewModel
            {
                // Ayarlar tablosundan ilk kaydý al, yoksa boþ bir nesne gönder (hata vermemesi için)
                Ayarlar = _context.HemsirelikAyarlar.FirstOrDefault() ?? new HemsirelikAyar(),
                Sekmeler = sekmeler,

                // Tek tabloyu Kategori sütununa göre View'daki ilgili listelere paylaþtýrýyoruz
                YonetimKadrosu = tumIcerikler.Where(x => x.Kategori == "Yonetim").ToList(),
                Gorevler = tumIcerikler.Where(x => x.Kategori == "Gorev").ToList(),
                Mevzuatlar = tumIcerikler.Where(x => x.Kategori == "Mevzuat").ToList(),
                GaleriFotograflari = tumIcerikler.Where(x => x.Kategori == "Galeri").ToList(),
                Etkinlikler = tumIcerikler.Where(x => x.Kategori == "Etkinlik").ToList(),
                AkisSemalari = tumIcerikler.Where(x => x.Kategori == "Akis").ToList()
            };

            return View(viewModel);
        }

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