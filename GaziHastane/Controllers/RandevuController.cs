using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GaziHastane.Data;
using GaziHastane.Models;
using System;
using System.Linq;

namespace GaziHastane.Controllers
{
    public class RandevuController : Controller
    {
        private readonly GaziHastaneContext _context;

        // Dependency Injection ile DbContext'i alýyoruz
        public RandevuController(GaziHastaneContext context)
        {
            _context = context;
        }

        // Giriþ Ekraný
        [HttpGet]
        public IActionResult Giris()
        {
            return View();
        }

        // Giriþ yapýldýktan sonra açýlan seçim ekraný (userId parametresi eklendi)
        [HttpGet]
        public IActionResult Secim(int? userId)
        {
            // Eðer userId gelmediyse (direkt linkten girilmeye çalýþýldýysa) giriþ sayfasýna yönlendir
            if (userId == null)
            {
                TempData["Error"] = "Lütfen önce kimlik doðrulamasý yapýnýz.";
                return RedirectToAction("Giris");
            }

            // Veritabanýndan giriþ yapan kullanýcýyý buluyoruz
            var aktifKullanici = _context.Users.Find(userId);
            if (aktifKullanici == null)
            {
                return RedirectToAction("Giris");
            }

            // Kullanýcý bilgilerini Arayüze (HTML'e) taþýyoruz (Murat SARI yerine bu kullanýlacak)
            ViewBag.KullaniciAdSoyad = aktifKullanici.Ad + " " + aktifKullanici.Soyad;

            // Baþ harfleri alýyoruz (Örn: Hasret Özdemir -> HÖ)
            ViewBag.KullaniciBasHarfler = aktifKullanici.Ad.Substring(0, 1) + aktifKullanici.Soyad.Substring(0, 1);

            // Javascript'e göndermek için ID'yi View'a taþýyoruz
            ViewBag.KullaniciId = aktifKullanici.Id;

            // Aktif bölümleri veritabanýndan çekip ViewBag ile View'a gönderiyoruz
            ViewBag.Bolumler = _context.Bolumler.Where(b => b.IsActive).ToList();
            return View();
        }

        // Bölüm seçildiðinde o bölümün doktorlarýný getiren AJAX Endpoint'i
        [HttpGet]
        public JsonResult GetDoktorlar(int bolumId)
        {
            var doktorlar = _context.Doktorlar
                .Where(d => d.BolumId == bolumId && d.IsActive)
                .Select(d => new
                {
                    id = d.Id,
                    // Unvan (Prof. Dr., Doç. Dr. vb.) varsa adýn baþýna ekler
                    adSoyad = (string.IsNullOrEmpty(d.Unvan) ? "" : d.Unvan + " ") + d.Ad + " " + d.Soyad
                })
                .ToList();

            return Json(doktorlar);
        }

        // Login Ýþlemi (POST)
        [HttpPost]
        public IActionResult Login(string loginType, string IdentityNumber, int Day, string Month, int Year)
        {
            // Formdan gelen Türkçe ay metnini sayýya (1-12) çevirmek için dizi oluþturuyoruz
            string[] aylar = { "Ocak", "Þubat", "Mart", "Nisan", "Mayýs", "Haziran", "Temmuz", "Aðustos", "Eylül", "Ekim", "Kasým", "Aralýk" };
            int monthNumber = Array.IndexOf(aylar, Month) + 1;

            // Gelen verilerde boþ veya eksik var mý diye kontrol et
            if (string.IsNullOrEmpty(IdentityNumber) || monthNumber == 0 || Day == 0 || Year == 0)
            {
                TempData["Error"] = "Lütfen kimlik bilgilerinizi ve doðum tarihinizi eksiksiz giriniz.";
                return RedirectToAction("Giris");
            }

            // Veritabanýnda kullanýcýyý TCKimlikNo'ya (veya pasaporta) göre ara
            var user = _context.Users.FirstOrDefault(u => u.TCKimlikNo == IdentityNumber);

            // Kullanýcý varsa ve seçilen doðum tarihi (Gün/Ay/Yýl) veritabanýndakiyle eþleþiyorsa giriþ yap
            if (user != null && user.DogumTarihi.Day == Day && user.DogumTarihi.Month == monthNumber && user.DogumTarihi.Year == Year)
            {
                // Doðrulama BAÞARILI. Kullanýcýnýn ID'sini Secim ekranýna parametre olarak yolluyoruz.
                return RedirectToAction("Secim", new { userId = user.Id });
            }

            // Doðrulama BAÞARISIZ
            TempData["Error"] = "Kimlik numarasý veya doðum tarihi hatalý. Lütfen kontrol edip tekrar deneyin.";
            return RedirectToAction("Giris");
        }

        // RANDEVU KAYDETME ÝÞLEMÝ (POST) - HastaId parametresi eklendi
        [HttpPost]
        public JsonResult RandevuKaydet(int BolumId, int DoktorId, string Tarih, string Saat, int HastaId)
        {
            try
            {
                // 1. Gelen verilerin boþ olup olmadýðýný kontrol et
                if (BolumId <= 0 || DoktorId <= 0 || string.IsNullOrEmpty(Tarih) || string.IsNullOrEmpty(Saat) || HastaId <= 0)
                {
                    return Json(new { success = false, message = "Lütfen seçimleri eksiksiz yapýnýz." });
                }

                // 2. JS'den gelen Tarih (yyyy-MM-dd) ve Saat (HH:mm) verisini birleþtirip C# DateTime formatýna çevir
                DateTime randevuZamani = DateTime.Parse($"{Tarih} {Saat}");

                // 3. Veritabaný modeli için yeni Randevu nesnesi oluþtur
                var yeniRandevu = new Randevu
                {
                    BolumId = BolumId,
                    DoktorId = DoktorId,
                    RandevuTarihi = randevuZamani,
                    Durum = 1, // 1: Bekliyor durumunda
                    OlusturulmaTarihi = DateTime.UtcNow,
                    Sikayet = "Kullanýcý arayüzünden oluþturuldu",
                    HastaId = HastaId // Artýk giriþ yapan kiþinin gerçek ID'si kaydedilecek!
                };

                // 4. Veritabanýna Ekle ve Kaydet
                _context.Randevular.Add(yeniRandevu);
                _context.SaveChanges();

                // Ýþlem baþarýlý mesajýný döndür
                return Json(new { success = true, message = "Randevunuz baþarýyla oluþturulmuþtur. Saðlýklý günler dileriz!" });
            }
            catch (System.Exception ex)
            {
                // Hatanýn detayýný daha net görebilmek için InnerException kontrolü
                string hataMesaji = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return Json(new { success = false, message = "Sistemsel Hata: " + hataMesaji });
            }
        }
    }
}