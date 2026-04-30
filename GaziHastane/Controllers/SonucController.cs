using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GaziHastane.Data;
using System.Linq;
using QRCoder;

namespace GaziHastane.Controllers
{
    public class SonucController : Controller
    {
        private readonly GaziHastaneContext _context;
        private readonly IWebHostEnvironment _env;

        // Veritabaný baðlamýný alýyoruz
        public SonucController(GaziHastaneContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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

        [HttpGet]
        public JsonResult GetSonucFisi(int sonucId, int? userId)
        {
            try
            {
                if (sonucId <= 0)
                    return Json(new { success = false, message = "Geçersiz sonuç id'si." });

                var sonuc = _context.TahlilSonuclari
                    .AsNoTracking()
                    .Include(x => x.Hasta)
                    .Include(x => x.Doktor)
                    .FirstOrDefault(x => x.Id == sonucId);

                if (sonuc == null)
                    return Json(new { success = false, message = "Sonuç kaydý bulunamadý." });

                if (userId.HasValue && sonuc.HastaId != userId.Value)
                    return Json(new { success = false, message = "Bu sonuca eriþim yetkiniz yok." });

                var hastaAd = sonuc.Hasta != null
                    ? $"{sonuc.Hasta.Ad} {sonuc.Hasta.Soyad}"
                    : "Hasta";

                var doktorAd = sonuc.Doktor != null
                    ? $"{(string.IsNullOrWhiteSpace(sonuc.Doktor.Unvan) ? "Dr." : sonuc.Doktor.Unvan)} {sonuc.Doktor.Ad} {sonuc.Doktor.Soyad}"
                    : "Doktor";

                var durum = string.IsNullOrWhiteSpace(sonuc.SonucDegeri) ? "Onay Bekliyor" : "Tamamlandý";

                var raporDosyaUrl = sonuc.RaporDosyaUrl ?? string.Empty;
                string raporDownloadUrl = string.Empty;
                bool raporVar = false;

                if (!string.IsNullOrWhiteSpace(raporDosyaUrl))
                {
                    // Eðer absolute URL ise olduðu gibi kullan
                    if (raporDosyaUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    {
                        raporDownloadUrl = raporDosyaUrl;
                        raporVar = true;
                    }
                    else
                    {
                        // normalize path and check wwwroot
                        var relative = raporDosyaUrl.TrimStart('~').TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
                        if (!string.IsNullOrEmpty(_env?.WebRootPath))
                        {
                            var physical = Path.Combine(_env.WebRootPath, relative);
                            if (System.IO.File.Exists(physical))
                            {
                                raporVar = true;
                                raporDownloadUrl = $"{Request.Scheme}://{Request.Host}/{relative.Replace(Path.DirectorySeparatorChar, '/')}";
                            }
                            else
                            {
                                // fallback: expose relative as URL
                                raporDownloadUrl = $"{Request.Scheme}://{Request.Host}/{relative.Replace(Path.DirectorySeparatorChar, '/')}";
                            }
                        }
                        else
                        {
                            raporDownloadUrl = raporDosyaUrl;
                        }
                    }
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        sonucNo = sonuc.Id,
                        hastaAd,
                        testAdi = sonuc.TestAdi,
                        kategori = sonuc.TestKategorisi ?? "Genel",
                        tarih = sonuc.Tarih.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("tr-TR")),
                        doktorAd,
                        sonucDegeri = string.IsNullOrWhiteSpace(sonuc.SonucDegeri) ? "Sonuç henüz çýkmadý" : sonuc.SonucDegeri,
                        referansAraligi = string.IsNullOrWhiteSpace(sonuc.ReferansAraligi) ? "-" : sonuc.ReferansAraligi,
                        durum,
                        raporDosyaUrl,
                        raporDownloadUrl,
                        raporDosyaVar = raporVar
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Sonuç fiþi hazýrlanýrken bir hata oluþtu.", detail = ex.Message });
            }
        }
    }
}