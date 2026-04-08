using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GaziHastane.Data;
using GaziHastane.Models;
using System;
using System.Linq;
using System.Collections.Generic;

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

        [HttpGet]
        public JsonResult GetDoktorAcilanGunler(int doktorId, int yil, int ay)
        {
            try
            {
                var plan = _context.DoktorRandevuPlanlari
                    .Include(x => x.Gunler)
                    .FirstOrDefault(x => x.DoktorId == doktorId && x.Yil == yil && x.Ay == ay);

                var culture = new System.Globalization.CultureInfo("tr-TR");
                var result = new List<object>();

                var ayBaslangic = new DateTime(yil, ay, 1);
                var ayBitis = ayBaslangic.AddMonths(1).AddDays(-1);

                for (var gun = ayBaslangic; gun <= ayBitis; gun = gun.AddDays(1))
                {
                    var planGunAcik = plan != null && plan.Gunler.Any(x => x.Tarih.Date == gun.Date && x.IsRandevuAcik);
                    var uygunSaatler = planGunAcik ? HesaplaUygunSaatler(doktorId, gun) : new List<string>();

                    if (planGunAcik && gun.Date == DateTime.Today)
                    {
                        var simdi = DateTime.Now.TimeOfDay;
                        uygunSaatler = uygunSaatler.Where(s => TimeSpan.Parse(s) > simdi).ToList();
                    }

                    result.Add(new
                    {
                        tarih = gun.ToString("yyyy-MM-dd"),
                        gun = gun.ToString("ddd", culture),
                        gunNo = gun.Day,
                        ay = gun.ToString("MMM", culture),
                        isOpen = planGunAcik,
                        isFull = planGunAcik && uygunSaatler.Count == 0,
                        slot = uygunSaatler.Count
                    });
                }

                return Json(result);
            }
            catch
            {
                return Json(new List<object>());
            }
        }

        // Giriþ Ekraný
        [HttpGet]
        public IActionResult Giris()
        {
            return View();
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

            // Veritabanýnda kullanýcýyý TCKimlikNo'ya göre ara
            var user = _context.Users.FirstOrDefault(u => u.TCKimlikNo == IdentityNumber);

            // Kullanýcý varsa ve seçilen doðum tarihi eþleþiyorsa giriþ yap
            if (user != null && user.DogumTarihi.Day == Day && user.DogumTarihi.Month == monthNumber && user.DogumTarihi.Year == Year)
            {
                // Doðrulama BAÞARILI. Kullanýcýnýn ID'sini Secim ekranýna parametre olarak yolluyoruz.
                return RedirectToAction("Secim", new { userId = user.Id });
            }

            // Doðrulama BAÞARISIZ
            TempData["Error"] = "Kimlik numarasý veya doðum tarihi hatalý. Lütfen kontrol edip tekrar deneyin.";
            return RedirectToAction("Giris");
        }

        // Seçim Ekraný ve Randevularým Sekmesi Ýçin Veriler
        [HttpGet]
        public IActionResult Secim(int? userId)
        {
            if (userId == null)
            {
                TempData["Error"] = "Lütfen önce kimlik doðrulamasý yapýnýz.";
                return RedirectToAction("Giris");
            }

            var aktifKullanici = _context.Users.Find(userId);
            if (aktifKullanici == null)
            {
                return RedirectToAction("Giris");
            }

            // Kullanýcý Profili Bilgileri
            ViewBag.KullaniciAdSoyad = aktifKullanici.Ad + " " + aktifKullanici.Soyad;
            ViewBag.KullaniciBasHarfler = aktifKullanici.Ad.Substring(0, 1) + aktifKullanici.Soyad.Substring(0, 1);
            ViewBag.KullaniciId = aktifKullanici.Id;

            // Aktif Bölümler Listesi
            ViewBag.Bolumler = _context.Bolumler.Where(b => b.IsActive).ToList();

            // KULLANICININ RANDEVU GEÇMÝÞÝNÝ GETÝR (Tablo Ýçin)
            var tumRandevular = _context.Randevular
                .Include(r => r.Doktor)
                .Include(r => r.Bolum)
                .Where(r => r.HastaId == userId)
                .OrderByDescending(r => r.RandevuTarihi)
                .ToList();

            // Yaklaþan Randevular: Tarihi bugünden büyük ve Durumu 1 (Aktif) olanlar
            ViewBag.YaklasanRandevular = tumRandevular
                .Where(r => r.RandevuTarihi > DateTime.Now && r.Durum == 1).ToList();

            // Geçmiþ veya Ýptal Edilmiþ Randevular: Tarihi bugünden küçük veya Durumu 1'den farklý olanlar
            ViewBag.GecmisRandevular = tumRandevular
                .Where(r => r.RandevuTarihi <= DateTime.Now || r.Durum != 1).ToList();

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
                    adSoyad = (string.IsNullOrEmpty(d.Unvan) ? "" : d.Unvan + " ") + d.Ad + " " + d.Soyad
                })
                .ToList();

            return Json(doktorlar);
        }

        // Doktor ve Tarih seçildiðinde planlanan tüm saatleri (müsait/dolu) getiren Endpoint
        [HttpGet]
        public JsonResult GetUygunSaatler(int doktorId, string tarih)
        {
            try
            {
                DateTime secilenTarih = DateTime.Parse(tarih);

                var slotlar = HesaplaGunSlotDurumlari(doktorId, secilenTarih);

                // Bugün için geçmiþ saatleri de dolu/pasif iþaretle
                if (secilenTarih.Date == DateTime.Today)
                {
                    var simdi = DateTime.Now.TimeOfDay;
                    foreach (var slot in slotlar)
                    {
                        if (TimeSpan.Parse(slot.Saat) <= simdi)
                        {
                            slot.Musait = false;
                        }
                    }
                }

                return Json(slotlar.Select(x => new { saat = x.Saat, musait = x.Musait, dolu = x.Dolu, ogleMolasi = x.OgleMolasi }).ToList());
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Saatler hesaplanýrken sunucu hatasý oluþtu." });
            }
        }

        // RANDEVU KAYDETME ÝÞLEMÝ (POST)
        [HttpPost]
        public JsonResult RandevuKaydet(int BolumId, int DoktorId, string Tarih, string Saat, int HastaId)
        {
            try
            {
                if (BolumId <= 0 || DoktorId <= 0 || string.IsNullOrEmpty(Tarih) || string.IsNullOrEmpty(Saat) || HastaId <= 0)
                {
                    return Json(new { success = false, message = "Lütfen seçimleri eksiksiz yapýnýz." });
                }

                DateTime randevuZamani = DateTime.Parse($"{Tarih} {Saat}");

                var musaitSaatler = HesaplaGunSlotDurumlari(DoktorId, randevuZamani.Date)
                    .Where(x => x.Musait)
                    .Select(x => x.Saat)
                    .ToHashSet();

                if (!musaitSaatler.Contains(randevuZamani.ToString("HH:mm")))
                {
                    return Json(new { success = false, message = "Seçilen saat doktor planýna uygun deðil veya dolu." });
                }

                // GÜVENLÝK KONTROLÜ: Ayný doktora, ayný saate baþka bir aktif randevu var mý?
                bool saatDoluMu = _context.Randevular.Any(r =>
                    r.DoktorId == DoktorId &&
                    r.RandevuTarihi == randevuZamani &&
                    r.Durum == 1);

                if (saatDoluMu)
                {
                    return Json(new { success = false, message = "Üzgünüz, bu saat dilimi az önce baþka bir hasta tarafýndan alýndý. Lütfen baþka bir saat seçiniz." });
                }

                var yeniRandevu = new Randevu
                {
                    BolumId = BolumId,
                    DoktorId = DoktorId,
                    RandevuTarihi = randevuZamani,
                    Durum = 1, // 1: Aktif
                    OlusturulmaTarihi = DateTime.UtcNow,
                    Sikayet = "Kullanýcý arayüzünden oluþturuldu",
                    HastaId = HastaId
                };

                _context.Randevular.Add(yeniRandevu);
                _context.SaveChanges();

                return Json(new { success = true, message = "Randevunuz baþarýyla oluþturulmuþtur. Saðlýklý günler dileriz!" });
            }
            catch (Exception ex)
            {
                string hataMesaji = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return Json(new { success = false, message = "Sistemsel Hata: " + hataMesaji });
            }
        }

        private List<string> HesaplaUygunSaatler(int doktorId, DateTime secilenTarih)
        {
            return HesaplaGunSlotDurumlari(doktorId, secilenTarih)
                .Where(x => x.Musait)
                .Select(x => x.Saat)
                .ToList();
        }

        private List<GunSlotDurumu> HesaplaGunSlotDurumlari(int doktorId, DateTime secilenTarih)
        {
            var plan = _context.DoktorRandevuPlanlari
                .Include(x => x.Gunler)
                .FirstOrDefault(x => x.DoktorId == doktorId && x.Yil == secilenTarih.Year && x.Ay == secilenTarih.Month);

            if (plan == null)
            {
                return new List<GunSlotDurumu>();
            }

            var planGun = plan.Gunler.FirstOrDefault(x => x.Tarih.Date == secilenTarih.Date && x.IsRandevuAcik);
            if (planGun == null)
            {
                return new List<GunSlotDurumu>();
            }

            var gunlukRandevuAdedi = _context.Randevular.Count(r =>
                r.DoktorId == doktorId &&
                r.Durum == 1 &&
                r.RandevuTarihi.Date == secilenTarih.Date);

            var gunlukMax = planGun.GunlukMaxRandevu > 0 ? planGun.GunlukMaxRandevu : 20;

            var slotSure = plan.SlotSureDakika <= 0 ? 30 : plan.SlotSureDakika;
            var baslangic = planGun.BaslangicSaati ?? plan.BaslangicSaati;
            var bitis = planGun.BitisSaati ?? plan.BitisSaati;
            var ogleBaslangic = plan.OgleMolaBaslangicSaati;
            var ogleBitis = plan.OgleMolaBitisSaati;

            var doluSaatler = _context.Randevular
                .Where(r => r.DoktorId == doktorId && r.Durum == 1 && r.RandevuTarihi.Date == secilenTarih.Date)
                .Select(r => r.RandevuTarihi.ToString("HH:mm"))
                .ToHashSet();

            var saatler = new List<GunSlotDurumu>();
            var current = baslangic;
            var uretilenSlot = 0;
            var gunlukDolu = gunlukRandevuAdedi >= gunlukMax;

            while (current.Add(TimeSpan.FromMinutes(slotSure)) <= bitis && uretilenSlot < gunlukMax)
            {
                var saat = current.ToString("hh\\:mm");
                var slotBitis = current.Add(TimeSpan.FromMinutes(slotSure));
                var ogeleCarpisiyor = current < ogleBitis && slotBitis > ogleBaslangic;
                var dolu = doluSaatler.Contains(saat);
                var musait = !gunlukDolu && !dolu && !ogeleCarpisiyor;
                saatler.Add(new GunSlotDurumu
                {
                    Saat = saat,
                    Musait = musait,
                    Dolu = dolu,
                    OgleMolasi = ogeleCarpisiyor
                });

                uretilenSlot++;
                current = current.Add(TimeSpan.FromMinutes(slotSure));
            }

            return saatler;
        }

        private class GunSlotDurumu
        {
            public string Saat { get; set; } = string.Empty;
            public bool Musait { get; set; }
            public bool Dolu { get; set; }
            public bool OgleMolasi { get; set; }
        }

        // RANDEVU ÝPTAL ÝÞLEMÝ (POST)
        [HttpPost]
        public JsonResult RandevuIptal(int id)
        {
            try
            {
                var randevu = _context.Randevular.Find(id);
                if (randevu != null)
                {
                    randevu.Durum = 2; // 2: Ýptal Edildi
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Randevu bulunamadý." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Ýptal iþlemi sýrasýnda bir hata oluþtu." });
            }
        }
    }
}