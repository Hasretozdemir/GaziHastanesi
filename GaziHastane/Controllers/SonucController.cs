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
        public IActionResult Panel(int? userId, string? kategori)
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
            ViewBag.KullaniciId = aktifKullanici.Id;

            var seciliKategori = (kategori ?? "laboratuvar").Trim().ToLowerInvariant();

            var hastaSonuclariQuery = _context.TahlilSonuclari
                .AsNoTracking()
                .Where(t => t.HastaId == userId);

            var radyolojiSayisi = hastaSonuclariQuery.Count(t =>
                EF.Functions.ILike(t.TestKategorisi ?? string.Empty, "%radyoloj%"));

            var patolojiSayisi = hastaSonuclariQuery.Count(t =>
                EF.Functions.ILike(t.TestKategorisi ?? string.Empty, "%patoloj%"));

            var laboratuvarSayisi = hastaSonuclariQuery.Count() - radyolojiSayisi - patolojiSayisi;

            var filtreliQuery = hastaSonuclariQuery;

            if (seciliKategori == "radyoloji")
            {
                filtreliQuery = filtreliQuery.Where(t =>
                    EF.Functions.ILike(t.TestKategorisi ?? string.Empty, "%radyoloj%"));
            }
            else if (seciliKategori == "patoloji")
            {
                filtreliQuery = filtreliQuery.Where(t =>
                    EF.Functions.ILike(t.TestKategorisi ?? string.Empty, "%patoloj%"));
            }
            else
            {
                seciliKategori = "laboratuvar";
                filtreliQuery = filtreliQuery.Where(t =>
                    !EF.Functions.ILike(t.TestKategorisi ?? string.Empty, "%radyoloj%") &&
                    !EF.Functions.ILike(t.TestKategorisi ?? string.Empty, "%patoloj%"));
            }

            var sonuclar = filtreliQuery
                .OrderByDescending(t => t.Tarih)
                .ToList();

            ViewBag.SeciliKategori = seciliKategori;
            ViewBag.LaboratuvarSayisi = laboratuvarSayisi;
            ViewBag.RadyolojiSayisi = radyolojiSayisi;
            ViewBag.PatolojiSayisi = patolojiSayisi;

            return View(sonuclar);
        }
    }
}