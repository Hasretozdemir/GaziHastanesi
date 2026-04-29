using System;
using System.Collections.Generic;
using System.Linq;
using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using Microsoft.EntityFrameworkCore;

namespace GaziHastane.Controllers
{
    public class RandevuController : Controller
    {
        private readonly GaziHastaneContext _context;

        // Dependency Injection ile DbContext'i al�yoruz
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

        // Giri� Ekran�
        [HttpGet]
        public IActionResult Giris()
        {
            return View();
        }

        // Login ��lemi (POST)
        [HttpPost]
        public IActionResult Login(string loginType, string IdentityNumber, int Day, string Month, int Year)
        {
            // Formdan gelen T�rk�e ay metnini say�ya (1-12) �evirmek i�in dizi olu�turuyoruz
            string[] aylar = { "Ocak", "�ubat", "Mart", "Nisan", "May�s", "Haziran", "Temmuz", "A�ustos", "Eyl�l", "Ekim", "Kas�m", "Aral�k" };
            int monthNumber = Array.IndexOf(aylar, Month) + 1;

            // Gelen verilerde bo� veya eksik var m� diye kontrol et
            if (string.IsNullOrEmpty(IdentityNumber) || monthNumber == 0 || Day == 0 || Year == 0)
            {
                TempData["Error"] = "L�tfen kimlik bilgilerinizi ve do�um tarihinizi eksiksiz giriniz.";
                return RedirectToAction("Giris");
            }

            // Veritaban�nda kullan�c�y� TCKimlikNo'ya g�re ara
            var user = _context.Users.FirstOrDefault(u => u.TCKimlikNo == IdentityNumber);

            // Kullan�c� varsa ve se�ilen do�um tarihi e�le�iyorsa giri� yap
            if (user != null && user.DogumTarihi.Day == Day && user.DogumTarihi.Month == monthNumber && user.DogumTarihi.Year == Year)
            {
                // Do�rulama BA�ARILI. Kullan�c�n�n ID'sini Secim ekran�na parametre olarak yolluyoruz.
                return RedirectToAction("Secim", new { userId = user.Id });
            }

            // Do�rulama BA�ARISIZ
            TempData["Error"] = "Kimlik numaras� veya do�um tarihi hatal�. L�tfen kontrol edip tekrar deneyin.";
            return RedirectToAction("Giris");
        }

        // Se�im Ekran� ve Randevular�m Sekmesi ��in Veriler
        [HttpGet]
        public IActionResult Secim(int? userId)
        {
            if (userId == null)
            {
                TempData["Error"] = "L�tfen �nce kimlik do�rulamas� yap�n�z.";
                return RedirectToAction("Giris");
            }

            var aktifKullanici = _context.Users.Find(userId);
            if (aktifKullanici == null)
            {
                return RedirectToAction("Giris");
            }

            // Kullan�c� Profili Bilgileri
            ViewBag.KullaniciAdSoyad = aktifKullanici.Ad + " " + aktifKullanici.Soyad;
            ViewBag.KullaniciBasHarfler = aktifKullanici.Ad.Substring(0, 1) + aktifKullanici.Soyad.Substring(0, 1);
            ViewBag.KullaniciId = aktifKullanici.Id;

            // Aktif B�l�mler Listesi
            ViewBag.Bolumler = _context.Bolumler.Where(b => b.IsActive).ToList();

            // KULLANICININ RANDEVU GE�M���N� GET�R (Tablo ��in)
            var tumRandevular = _context.Randevular
                .Include(r => r.Doktor)
                .Include(r => r.Bolum)
                .Where(r => r.HastaId == userId)
                .OrderByDescending(r => r.RandevuTarihi)
                .ToList();

            // Yakla�an Randevular: Tarihi bug�nden b�y�k ve Durumu 1 (Aktif) olanlar
            ViewBag.YaklasanRandevular = tumRandevular
                .Where(r => r.RandevuTarihi > DateTime.Now && r.Durum == 1).ToList();

            // Ge�mi� veya �ptal Edilmi� Randevular: Tarihi bug�nden k���k veya Durumu 1'den farkl� olanlar
            // Gemi veya ptal Edilmi Randevular: Tarihi bugnden kk veya Durumu 1'den farkl olanlar
            ViewBag.GecmisRandevular = tumRandevular
                .Where(r => r.RandevuTarihi <= DateTime.Now || r.Durum != 1).ToList();

            return View();
        }

        // Blm seildiinde o blmn doktorlarn getiren AJAX Endpoint'i
        [HttpGet]
        public JsonResult GetBolumlerByHekimTipi(short? hekimTipi)
        {
            List<int> bolumIdler;

            if (hekimTipi.HasValue && hekimTipi.Value == 2) // Sadece Asistan
            {
                bolumIdler = _context.Doktorlar
                    .Where(d => d.IsActive && d.HekimTipi == 2 && d.BolumId != null)
                    .Select(d => d.BolumId!.Value)
                    .Distinct()
                    .ToList();
            }
            else if (hekimTipi.HasValue && hekimTipi.Value == 1) // Uzman: 1 veya 0 (tanimsiz) olanlar
            {
                bolumIdler = _context.Doktorlar
                    .Where(d => d.IsActive && (d.HekimTipi == 1 || d.HekimTipi == 0) && d.BolumId != null)
                    .Select(d => d.BolumId!.Value)
                    .Distinct()
                    .ToList();
            }
            else // Tumu
            {
                bolumIdler = _context.Doktorlar
                    .Where(d => d.IsActive && d.BolumId != null)
                    .Select(d => d.BolumId!.Value)
                    .Distinct()
                    .ToList();
            }

            var bolumler = _context.Bolumler
                .Where(b => b.IsActive && bolumIdler.Contains(b.Id))
                .OrderBy(b => b.Ad)
                .Select(b => new
                {
                    id = b.Id,
                    ad = b.Ad,
                    blok = b.Blok ?? "",
                    kat = b.Kat ?? ""
                })
                .ToList();

            return Json(bolumler);
        }

        // DEBUG: Doktorlarin HekimTipi degerlerini gormek icin
        [HttpGet]
        public JsonResult GetDoktorHekimTipleri()
        {
            var doktorlar = _context.Doktorlar
                .Select(d => new { d.Id, d.Ad, d.Soyad, d.HekimTipi, d.IsActive, d.BolumId })
                .ToList();
            return Json(doktorlar);
        }

        // Blm seildiinde o blmn doktorlarn getiren AJAX Endpoint'i
        [HttpGet]
        public JsonResult GetDoktorlar(int bolumId, short? hekimTipi)
        {
            var doktorlarQuery = _context.Doktorlar
                .Where(d => d.BolumId == bolumId && d.IsActive);

            if (hekimTipi.HasValue && hekimTipi.Value == 2) // Sadece Asistan
            {
                doktorlarQuery = doktorlarQuery.Where(d => d.HekimTipi == 2);
            }
            else if (hekimTipi.HasValue && hekimTipi.Value == 1) // Uzman: 1 veya 0 (tanimsiz)
            {
                doktorlarQuery = doktorlarQuery.Where(d => d.HekimTipi == 1 || d.HekimTipi == 0);
            }
            // hekimTipi=0 (Tumu) ise filtre yok, tum doktorlar gelir

            var doktorlar = doktorlarQuery
                .Select(d => new
                {
                    id = d.Id,
                    adSoyad = (string.IsNullOrEmpty(d.Unvan) ? "" : d.Unvan + " ") + d.Ad + " " + d.Soyad
                })
                .ToList();

            return Json(doktorlar);
        }

        // Doktor ve Tarih se�ildi�inde planlanan t�m saatleri (m�sait/dolu) getiren Endpoint
        [HttpGet]
        public JsonResult GetUygunSaatler(int doktorId, string tarih)
        {
            try
            {
                DateTime secilenTarih = DateTime.Parse(tarih);

                var slotlar = HesaplaGunSlotDurumlari(doktorId, secilenTarih);

                // Bug�n i�in ge�mi� saatleri de dolu/pasif i�aretle
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
                return Json(new { success = false, message = "Saatler hesaplan�rken sunucu hatas� olu�tu." });
            }
        }

        // RANDEVU KAYDETME ��LEM� (POST)
        [HttpPost]
        public JsonResult RandevuKaydet(int BolumId, int DoktorId, string Tarih, string Saat, int HastaId, short RandevuTipi = 1)
        {
            try
            {
                if (BolumId <= 0 || DoktorId <= 0 || string.IsNullOrEmpty(Tarih) || string.IsNullOrEmpty(Saat) || HastaId <= 0)
                {
                    return Json(new { success = false, message = "L�tfen se�imleri eksiksiz yap�n�z." });
                }

                if (RandevuTipi != 1 && RandevuTipi != 2)
                {
                    return Json(new { success = false, message = "Ge�ersiz randevu tipi se�ildi." });
                }

                DateTime randevuZamani = DateTime.Parse($"{Tarih} {Saat}");

                var musaitSaatler = HesaplaGunSlotDurumlari(DoktorId, randevuZamani.Date)
                    .Where(x => x.Musait)
                    .Select(x => x.Saat)
                    .ToHashSet();

                if (!musaitSaatler.Contains(randevuZamani.ToString("HH:mm")))
                {
                    return Json(new { success = false, message = "Se�ilen saat doktor plan�na uygun de�il veya dolu." });
                }

                // G�VENL�K KONTROL�: Ayn� doktora, ayn� saate ba�ka bir aktif randevu var m�?
                bool saatDoluMu = _context.Randevular.Any(r =>
                    r.DoktorId == DoktorId &&
                    r.RandevuTarihi == randevuZamani &&
                    r.Durum == 1);

                if (saatDoluMu)
                {
                    return Json(new { success = false, message = "�zg�n�z, bu saat dilimi az �nce ba�ka bir hasta taraf�ndan al�nd�. L�tfen ba�ka bir saat se�iniz." });
                }

                var yeniRandevu = new Randevu
                {
                    BolumId = BolumId,
                    DoktorId = DoktorId,
                    RandevuTarihi = randevuZamani,
                    Durum = 1, // 1: Aktif
                    RandevuTipi = RandevuTipi,
                    OlusturulmaTarihi = DateTime.UtcNow,
                    Sikayet = RandevuTipi == 2 ? "Sonu� randevusu" : "Muayene randevusu",
                    HastaId = HastaId
                };

                _context.Randevular.Add(yeniRandevu);
                _context.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = "Randevunuz ba�ar�yla olu�turulmu�tur. Sa�l�kl� g�nler dileriz!",
                    randevuId = yeniRandevu.Id
                });
            }
            catch (Exception ex)
            {
                string hataMesaji = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return Json(new { success = false, message = "Sistemsel Hata: " + hataMesaji });
            }

        }

        [HttpGet]
        public JsonResult GetRandevuFisi(int randevuId, int? hastaId)
        {
            try
            {
                var randevu = _context.Randevular
                    .Include(x => x.Hasta)
                    .Include(x => x.Bolum)
                    .Include(x => x.Doktor)
                    .FirstOrDefault(x => x.Id == randevuId);

                if (randevu == null)
                {
                    return Json(new { success = false, message = "Randevu bulunamad�." });
                }

                if (hastaId.HasValue && randevu.HastaId != hastaId.Value)
                {
                    return Json(new { success = false, message = "Bu randevu fi�ine eri�im yetkiniz yok." });
                }

                var bolumAd = randevu.Bolum?.Ad ?? "Poliklinik";
                var blok = randevu.Bolum?.Blok?.Trim();
                var kat = randevu.Bolum?.Kat?.Trim();
                
                // Konumu veritaban�ndan �ek
                var iletisimBilgisi = _context.IletisimBilgileri.FirstOrDefault(x => x.IsActive);
                
                string konumBilgisi;

                if (!string.IsNullOrWhiteSpace(blok) && !string.IsNullOrWhiteSpace(kat))
                {
                    konumBilgisi = $"{blok} Blok, {kat}. Kat";
                }
                else if (!string.IsNullOrWhiteSpace(blok))
                {
                    konumBilgisi = $"{blok} Blok";
                }
                else if (!string.IsNullOrWhiteSpace(kat))
                {
                    konumBilgisi = $"{kat}. Kat";
                }
                else if (iletisimBilgisi != null && !string.IsNullOrWhiteSpace(iletisimBilgisi.Adres))
                {
                    // Fallback: �leti�im tablosundan adres bilgisini �ek
                    konumBilgisi = iletisimBilgisi.KisaAdres ?? iletisimBilgisi.Adres;
                }
                else
                {
                    konumBilgisi = "E Blok, Zemin Kat"; // Varsay�lan
                }

                var krokiUrl = Url.Action("Kroki", "Home", new { hedef = bolumAd }, Request.Scheme) ?? $"/Home/Kroki?hedef={bolumAd}";

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(krokiUrl, QRCodeGenerator.ECCLevel.Q);
                Base64QRCode qrCode = new Base64QRCode(qrCodeData);
                string qrCodeImageAsBase64 = qrCode.GetGraphic(20);

                var tcKimlikNo = randevu.Hasta?.TCKimlikNo ?? "-";
                var hastaAd = randevu.Hasta != null
                    ? $"{randevu.Hasta.Ad} {randevu.Hasta.Soyad}"
                    : "Hasta";

                var doktorAd = randevu.Doktor != null
                    ? $"{(string.IsNullOrWhiteSpace(randevu.Doktor.Unvan) ? "Dr." : randevu.Doktor.Unvan)} {randevu.Doktor.Ad} {randevu.Doktor.Soyad}"
                    : "Doktor";

                var durumText = randevu.Durum == 1 ? "Onayland�" : (randevu.Durum == 2 ? "�ptal Edildi" : (randevu.Durum == 3 ? "Gelmedi" : "Tamamland�"));

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        randevuNo = randevu.Id,
                        tcKimlikNo,
                        hastaAd,
                        bolumAd,
                        doktorAd,
                        tarihSaat = randevu.RandevuTarihi.ToString("dd MMMM yyyy - HH:mm", new System.Globalization.CultureInfo("tr-TR")),
                        randevuTipi = randevu.RandevuTipi == 2 ? "Sonu�" : "Muayene",
                        durum = durumText,
                        konum = konumBilgisi,
                        qrCode = "data:image/png;base64," + qrCodeImageAsBase64,
                        krokiUrl
                    }
                });
            }
            catch
            {
                return Json(new { success = false, message = "Randevu fi�i haz�rlan�rken bir hata olu�tu." });
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

        // RANDEVU �PTAL ��LEM� (POST)
        [HttpPost]
        public JsonResult RandevuIptal(int id)
        {
            try
            {
                var randevu = _context.Randevular.Find(id);
                if (randevu != null)
                {
                    randevu.Durum = 2; // 2: �ptal Edildi
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Randevu bulunamad�." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "�ptal i�lemi s�ras�nda bir hata olu�tu." });
            }
        }

        public IActionResult RandevuBasarili(int randevuId)
        {
            var randevu = _context.Randevular
                .Include(x => x.Hasta)
                .Include(x => x.Bolum)
                .FirstOrDefault(x => x.Id == randevuId);

            if (randevu == null)
            {
                TempData["Error"] = "Randevu bilgisi bulunamad�.";
                return RedirectToAction("Giris");
            }

            var hastaAd = randevu.Hasta != null
                ? $"{randevu.Hasta.Ad} {randevu.Hasta.Soyad}"
                : "Hasta";
            var bolumAd = randevu.Bolum?.Ad ?? "Poliklinik";
            var tarihSaat = randevu.RandevuTarihi.ToString("dd MMMM yyyy - HH:mm", new System.Globalization.CultureInfo("tr-TR"));
            string krokiUrl = Url.Action("Kroki", "Home", new { hedef = bolumAd }, Request.Scheme) ?? $"/Home/Kroki?hedef={bolumAd}";

            // 3. QR Kodu Olustur
            QRCodeGenerator qrGenerator2 = new QRCodeGenerator();
            QRCodeData qrCodeData2 = qrGenerator2.CreateQrCode(krokiUrl, QRCodeGenerator.ECCLevel.Q);
            Base64QRCode qrCode2 = new Base64QRCode(qrCodeData2);
            string qrCodeImageAsBase64 = qrCode2.GetGraphic(20);

            // 4. View'a verileri g�nder
            ViewBag.QrCode = "data:image/png;base64," + qrCodeImageAsBase64;
            ViewBag.HastaAd = hastaAd;
            ViewBag.BolumAd = bolumAd;
            ViewBag.TarihSaat = tarihSaat;
            ViewBag.RandevuNo = randevu.Id;
            ViewBag.KrokiUrl = krokiUrl;
            ViewBag.HastaId = randevu.HastaId;

            return View();
        }
    }
}
