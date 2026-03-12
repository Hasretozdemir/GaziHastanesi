using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GaziHastane.Data;
using System.Linq;

namespace GaziHastane.Controllers
{
    public class SonucController : Controller
    {
        private readonly GaziHastaneContext _context;

        // Veritabaný baðlamýný alýyoruz
        public SonucController(GaziHastaneContext context)
        {
            _context = context;
        }

        // Sonuç Sorgulama Giriþ Ekraný
        [HttpGet]
        public IActionResult Giris()
        {
            return View();
        }

        // Sonuç Login Ýþlemi (POST)
        [HttpPost]
        public IActionResult Login(string identityNumber, string password)
        {
            if (string.IsNullOrEmpty(identityNumber) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Lütfen kimlik numaranýzý ve þifrenizi eksiksiz giriniz.";
                return RedirectToAction("Giris");
            }

            // Girilen TC Kimlik numarasý ve Þifreye (SifreHash) göre kullanýcýyý bul
            var user = _context.Users.FirstOrDefault(u => u.TCKimlikNo == identityNumber && u.SifreHash == password);

            if (user != null)
            {
                // Doðrulama baþarýlýysa ID ile Panele yönlendir
                return RedirectToAction("Panel", new { userId = user.Id });
            }

            TempData["Error"] = "Kimlik numarasý veya þifre hatalý.";
            return RedirectToAction("Giris");
        }

        // E-Nabýz stili sonuç listesi ekraný
        [HttpGet]
        public IActionResult Panel(int? userId)
        {
            if (userId == null)
            {
                return RedirectToAction("Giris");
            }

            // 1. Giriþ yapan hastanýn bilgilerini al
            var aktifKullanici = _context.Users.Find(userId);
            if (aktifKullanici == null) return RedirectToAction("Giris");

            ViewBag.KullaniciAdSoyad = aktifKullanici.Ad.ToUpper() + " " + aktifKullanici.Soyad.ToUpper();
            ViewBag.KullaniciBasHarfler = aktifKullanici.Ad.Substring(0, 1) + aktifKullanici.Soyad.Substring(0, 1);
            ViewBag.HastaNo = aktifKullanici.Id.ToString().PadLeft(6, '0'); // Örn: 000012

            // 2. Hastaya ait tahlil sonuçlarýný tarihe göre azalan (en yeni en üstte) þekilde getir
            var sonuclar = _context.TahlilSonuclari
                .Where(t => t.HastaId == userId)
                .OrderByDescending(t => t.Tarih)
                .ToList();

            return View(sonuclar);
        }
    }
}