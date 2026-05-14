using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class KurumsalController : Controller
    {
        private readonly GaziHastaneContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public KurumsalController(GaziHastaneContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            // Temizlik: Gereksiz/Örnek alt sekmeleri veritabanından temizle
            var silinecekler = new string[] { "Tarihçemiz", "Misyon & Vizyon", "Stratejik Plan & Raporlar", "Yönetmelik", "Amaç ve Hedeflerimiz", "Sağlık Rehberleri", "Hastanemiz Uyum Rehberi", "Eğitim Komitesi" };
            var ornekSekmeler = await _context.KurumsalSekmeler
                .Where(x => silinecekler.Contains(x.Baslik))
                .ToListAsync();

            if (ornekSekmeler.Any())
            {
                _context.KurumsalSekmeler.RemoveRange(ornekSekmeler);
                await _context.SaveChangesAsync();
            }

            // Eer Hakkimizda mens yoksa veritabanna ekle (Ynetim kolayl iin)
            bool hasHakkimizda = await _context.AdminMenuItems.AnyAsync(x => x.Controller == "Kurumsal" && x.Action == "Hakkimizda");
            if (!hasHakkimizda)
            {
                _context.AdminMenuItems.Add(new AdminMenuItem
                {
                    Section = "KurumsalSub",
                    Label = "Hakkımızda Sayfası",
                    Url = "/Admin/Kurumsal/Hakkimizda",
                    Controller = "Kurumsal",
                    Action = "Hakkimizda",
                    SortOrder = 0,
                    IsActive = true,
                    PermissionKey = "kurumsal"
                });
                await _context.SaveChangesAsync();
            }

            ViewBag.Gruplar = await _context.KurumsalMenuGruplar
                .OrderBy(x => x.Sira)
                .ToListAsync();

            var menuler = await _context.KurumsalMenuler
                .Include(x => x.Grup)
                .OrderBy(x => x.Grup!.Sira)
                .ThenBy(x => x.Sira)
                .ToListAsync();

            return View(menuler);
        }

        [HttpGet]
        public async Task<IActionResult> GrupForm(int? id)
        {
            if (id == null) return View(new KurumsalMenuGrup());

            var model = await _context.KurumsalMenuGruplar.FindAsync(id.Value);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GrupForm(KurumsalMenuGrup model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.Id == 0) _context.KurumsalMenuGruplar.Add(model);
            else _context.KurumsalMenuGruplar.Update(model);

            await _context.SaveChangesAsync();
            TempData["Success"] = "Menü grubu kaydedildi.";
            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // ARŞİV BİRİMİ YÖNETİMİ
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> ArsivBirimi()
        {
            var model = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "ArsivBirimi")
                .OrderBy(x => x.Sira)
                .ToListAsync();
            ViewBag.SayfaKey = "ArsivBirimi";
            ViewBag.SayfaBaslik = "Arşiv Birimi Yönetimi";
            return View("OrganizasyonSemalari", model);
        }

        [HttpGet]
        public IActionResult ArsivSekmeEkle()
        {
            return View("ArsivSekmeForm", new ArsivSekme { SabitTasarimMi = false, IsActive = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArsivSekmeEkle(ArsivSekme model)
        {
            if (ModelState.IsValid)
            {
                _context.ArsivSekmeler.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Yeni arşiv sekmesi başarıyla eklendi.";
                return RedirectToAction(nameof(ArsivBirimi));
            }
            return View("ArsivSekmeForm", model);
        }

        [HttpGet]
        public async Task<IActionResult> ArsivSekmeDuzenle(int id)
        {
            var sekme = await _context.ArsivSekmeler.FindAsync(id);
            if (sekme == null) return NotFound();
            return View("ArsivSekmeForm", sekme);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArsivSekmeDuzenle(ArsivSekme model)
        {
            if (ModelState.IsValid)
            {
                _context.ArsivSekmeler.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Arşiv sekmesi başarıyla güncellendi.";
                return RedirectToAction(nameof(ArsivBirimi));
            }
            return View("ArsivSekmeForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArsivSekmeSil(int id)
        {
            var sekme = await _context.ArsivSekmeler.FindAsync(id);
            if (sekme != null)
            {
                _context.ArsivSekmeler.Remove(sekme);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Arşiv sekmesi başarıyla silindi.";
            }
            return RedirectToAction(nameof(ArsivBirimi));
        }

        // ==========================================
        // DİĞER SABİT SAYFALAR
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> BasinVeKurumsalIletisim()
        {
            var model = await _context.BasinKurumsalIletisimler.FirstOrDefaultAsync();
            if (model == null)
            {
                model = new BasinKurumsalIletisim 
                { 
                    Baslik = "Basın ve Kurumsal İletişim Birimi",
                    Aciklama = "",
                    Telefon = "",
                    Email = "",
                    Lokasyon = ""
                };
            }
            return View("BasinKurumsalIletisimForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BasinVeKurumsalIletisimKaydet(BasinKurumsalIletisim model)
        {
            if (!ModelState.IsValid)
            {
                return View("BasinKurumsalIletisimForm", model);
            }

            model.SonGuncelleme = DateTime.UtcNow;

            if (model.Id == 0)
            {
                _context.BasinKurumsalIletisimler.Add(model);
            }
            else
            {
                _context.BasinKurumsalIletisimler.Update(model);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Basın ve Kurumsal İletişim bilgileri başarıyla kaydedildi.";
            return RedirectToAction(nameof(BasinVeKurumsalIletisim));
        }

        [HttpGet]
        public async Task<IActionResult> BilgiIslemMerkezi()
        {
            var model = await _context.KurumsalSekmeler
                .Where(x => x.SayfaKey == "BilgiIslemMerkezi")
                .OrderBy(x => x.Sira)
                .ToListAsync();
            ViewBag.SayfaKey = "BilgiIslemMerkezi";
            ViewBag.SayfaBaslik = "Bilgi İşlem Merkezi Yönetimi";
            return View("GenericSekmeYonetim", model);
        }

                [HttpGet]
        public async Task<IActionResult> EczacilikHizmetleri()
        {
            var model = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "EczacilikHizmetleri")
                .OrderBy(x => x.Sira)
                .ToListAsync();
            ViewBag.SayfaKey = "EczacilikHizmetleri";
            ViewBag.SayfaBaslik = "Eczacılık Hizmetleri Yönetimi";
            return View("OrganizasyonSemalari", model);
        }

                [HttpGet]
        public async Task<IActionResult> EnfeksiyonKontrol()
        {
            var icerikler = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "EnfeksiyonKontrol")
                .OrderBy(x => x.Sira)
                .ToListAsync();

            ViewBag.Sekmeler = await _context.KurumsalSekmeler
                .Where(x => x.SayfaKey == "EnfeksiyonKontrol")
                .OrderBy(x => x.Sira)
                .ToListAsync();

            ViewBag.SayfaKey = "EnfeksiyonKontrol";
            ViewBag.SayfaBaslik = "Enfeksiyon Kontrol Yönetimi";
            return View("EnfeksiyonKontrol", icerikler);
        }

                [HttpGet]
        public async Task<IActionResult> HastaIletisimBirimi()
        {
            var model = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "HastaIletisimBirimi")
                .OrderBy(x => x.Sira)
                .ToListAsync();
            ViewBag.SayfaKey = "HastaIletisimBirimi";
            ViewBag.SayfaBaslik = "Hasta İletişim Birimi Yönetimi";
            return View("OrganizasyonSemalari", model);
        }

        // ==========================================
        // HEMŞİRELİK HİZMETLERİ YÖNETİMİ
        // ==========================================

        [HttpGet]
        public IActionResult HemsirelikHizmetleri()
        {
            var icerikler = _context.HemsirelikIcerikler
                .OrderBy(x => x.Sira)
                .ToList();

            ViewBag.Ayarlar = _context.HemsirelikAyarlar.FirstOrDefault() ?? new HemsirelikAyar();
            ViewBag.Sekmeler = _context.HemsirelikSekmeler
                .OrderBy(x => x.Sira)
                .ToList();

            return View(icerikler);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HemsirelikAyarlarKaydet(HemsirelikAyar model)
        {
            if (model.Id == 0)
            {
                _context.HemsirelikAyarlar.Add(model);
            }
            else
            {
                _context.HemsirelikAyarlar.Update(model);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Ayarlar başarıyla kaydedildi.";
            return RedirectToAction(nameof(HemsirelikHizmetleri));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HemsirelikIcerikKaydet(HemsirelikIcerik model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Ayarlar = _context.HemsirelikAyarlar.FirstOrDefault() ?? new HemsirelikAyar();
                ViewBag.Sekmeler = _context.HemsirelikSekmeler.OrderBy(x => x.Sira).ToList();
                var icerikler = _context.HemsirelikIcerikler.OrderBy(x => x.Sira).ToList();
                return View("HemsirelikHizmetleri", icerikler);
            }

            model.Baslik ??= "";
            model.AltBaslik ??= "";
            model.Aciklama ??= "";
            model.MedyaYolu ??= "";

            if (model.Id == 0)
            {
                _context.HemsirelikIcerikler.Add(model);
            }
            else
            {
                _context.HemsirelikIcerikler.Update(model);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "İçerik başarıyla kaydedildi.";
            return RedirectToAction(nameof(HemsirelikHizmetleri));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HemsirelikIcerikSil(int id)
        {
            var icerik = await _context.HemsirelikIcerikler.FindAsync(id);
            if (icerik != null)
            {
                _context.HemsirelikIcerikler.Remove(icerik);
                await _context.SaveChangesAsync();
                TempData["Success"] = "İçerik başarıyla silindi.";
            }

            return RedirectToAction(nameof(HemsirelikHizmetleri));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HemsirelikSekmeKaydet(HemsirelikSekme model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Ayarlar = _context.HemsirelikAyarlar.FirstOrDefault() ?? new HemsirelikAyar();
                ViewBag.Sekmeler = _context.HemsirelikSekmeler.OrderBy(x => x.Sira).ToList();
                var icerikler = _context.HemsirelikIcerikler.OrderBy(x => x.Sira).ToList();
                return View("HemsirelikHizmetleri", icerikler);
            }

            model.IconClass ??= "";
            
            if (model.Id == 0)
            {
                _context.HemsirelikSekmeler.Add(model);
            }
            else
            {
                _context.HemsirelikSekmeler.Update(model);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Sekme başarıyla kaydedildi.";
            return Redirect($"{Url.Action(nameof(HemsirelikHizmetleri))}#tab-sekmeler");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HemsirelikSekmeSil(int id)
        {
            var sekme = await _context.HemsirelikSekmeler.FindAsync(id);
            if (sekme != null)
            {
                _context.HemsirelikSekmeler.Remove(sekme);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Sekme ve ilgili tüm içerikler için kategori bağlantısı silindi.";
            }

            return Redirect($"{Url.Action(nameof(HemsirelikHizmetleri))}#tab-sekmeler");
        }

        // ==========================================
        // DİĞER SABİT SAYFALAR (Devamı)
        // ==========================================

                [HttpGet]
        public async Task<IActionResult> IcKontrol()
        {
            var model = await _context.KurumsalSekmeler
                .Where(x => x.SayfaKey == "IcKontrol")
                .OrderBy(x => x.Sira)
                .ToListAsync();
            ViewBag.SayfaKey = "IcKontrol";
            ViewBag.SayfaBaslik = "İç Kontrol Yönetimi";
            return View("GenericSekmeYonetim", model);
        }

                [HttpGet]
        public async Task<IActionResult> IsAkisSemalari()
        {
            var model = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "IsAkisSemalari")
                .OrderBy(x => x.Sira)
                .ToListAsync();
            ViewBag.SayfaKey = "IsAkisSemalari";
            ViewBag.SayfaBaslik = "İş Akış Şemaları Yönetimi";
            return View("OrganizasyonSemalari", model);
        }

                [HttpGet]
        public async Task<IActionResult> IsSagligiVeGuvenligi()
        {
            var model = await _context.KurumsalSekmeler
                .Where(x => x.SayfaKey == "IsSagligiVeGuvenligi")
                .OrderBy(x => x.Sira)
                .ToListAsync();
            ViewBag.SayfaKey = "IsSagligiVeGuvenligi";
            ViewBag.SayfaBaslik = "İş Sağlığı ve Güvenliği Yönetimi";
            return View("GenericSekmeYonetim", model);
        }

                [HttpGet]
        public async Task<IActionResult> IstatistikVeRaporlama()
        {
            var model = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "IstatistikVeRaporlama")
                .OrderBy(x => x.Sira)
                .ToListAsync();
            ViewBag.SayfaKey = "IstatistikVeRaporlama";
            ViewBag.SayfaBaslik = "İstatistik ve Raporlama Yönetimi";
            return View("OrganizasyonSemalari", model);
        }

        [HttpGet]
        public async Task<IActionResult> OrganizasyonSemalari()
        {
            var model = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "OrganizasyonSemalari")
                .OrderBy(x => x.Sira)
                .ToListAsync();

            ViewBag.SayfaKey = "OrganizasyonSemalari";
            ViewBag.SayfaBaslik = "Organizasyon Şemaları Yönetimi";
            return View(model);
        }

                [HttpGet]
        public async Task<IActionResult> SatinAlma()
        {
            var model = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "SatinAlma")
                .OrderBy(x => x.Sira)
                .ToListAsync();
            ViewBag.SayfaKey = "SatinAlma";
            ViewBag.SayfaBaslik = "Satın Alma Yönetimi";
            return View("OrganizasyonSemalari", model);
        }

        // ==========================================
        // DİNAMİK MENÜ YÖNETİMİ
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GrupSil(int id)
        {
            var model = await _context.KurumsalMenuGruplar.FindAsync(id);
            if (model != null)
            {
                _context.KurumsalMenuGruplar.Remove(model);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> MenuForm(int? id)
        {
            ViewBag.Gruplar = await _context.KurumsalMenuGruplar
                .OrderBy(x => x.Sira)
                .ToListAsync();

            if (id == null) return View(new KurumsalMenu());

            var model = await _context.KurumsalMenuler.FindAsync(id.Value);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MenuForm(KurumsalMenu model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Gruplar = await _context.KurumsalMenuGruplar.OrderBy(x => x.Sira).ToListAsync();
                return View(model);
            }

            // URL boş bırakılırsa otomatik dinamik kurumsal sayfa URL'si üret
            if (string.IsNullOrWhiteSpace(model.Url))
            {
                var slugFromTitle = Slugify(model.Baslik);
                model.Url = $"/Kurumsal/Sayfa/{slugFromTitle}";
            }

            if (model.Id == 0) _context.KurumsalMenuler.Add(model);
            else _context.KurumsalMenuler.Update(model);

            await _context.SaveChangesAsync();

            // Menü kaydı sonrası ilgili sayfa kaydı yoksa otomatik oluştur
            await EnsureKurumsalSayfaForMenuAsync(model);

            TempData["Success"] = "Menü kaydedildi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SayfaDuzenle(int menuId)
        {
            var menu = await _context.KurumsalMenuler.FirstOrDefaultAsync(x => x.Id == menuId);
            if (menu == null) return NotFound();

            var slug = GetSlugFromMenu(menu);
            var sayfa = await _context.KurumsalSayfalar.FirstOrDefaultAsync(x => x.Slug == slug);

            if (sayfa == null)
            {
                sayfa = new KurumsalSayfa
                {
                    Slug = slug,
                    Baslik = menu.Baslik,
                    Icerik = "<p>Bu sayfa içeriğini buradan düzenleyebilirsiniz.</p>",
                    AktifMi = true,
                    GuncellemeZamani = DateTime.UtcNow
                };

                _context.KurumsalSayfalar.Add(sayfa);
                await _context.SaveChangesAsync();
            }

            ViewBag.MenuBaslik = menu.Baslik;
            ViewBag.MenuId = menu.Id;
            return View(sayfa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SayfaDuzenle(KurumsalSayfa model, int menuId)
        {
            var sayfa = await _context.KurumsalSayfalar.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (sayfa == null) return NotFound();

            sayfa.Baslik = model.Baslik;
            sayfa.Icerik = model.Icerik;
            sayfa.FotografUrl = model.FotografUrl;
            sayfa.Unvan = model.Unvan;
            sayfa.Email = model.Email;
            sayfa.Telefon = model.Telefon;
            sayfa.AktifMi = model.AktifMi;
            sayfa.GuncellemeZamani = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            TempData["Success"] = "Sayfa içeriği güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MenuSil(int id)
        {
            var model = await _context.KurumsalMenuler.FindAsync(id);
            if (model != null)
            {
                _context.KurumsalMenuler.Remove(model);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task EnsureKurumsalSayfaForMenuAsync(KurumsalMenu menu)
        {
            var slug = GetSlugFromMenu(menu);
            var sayfaVarMi = await _context.KurumsalSayfalar.AnyAsync(x => x.Slug == slug);
            if (sayfaVarMi) return;

            var sayfa = new KurumsalSayfa
            {
                Slug = slug,
                Baslik = menu.Baslik,
                Icerik = "<p>Bu sayfa içeriğini buradan düzenleyebilirsiniz.</p>",
                AktifMi = true,
                GuncellemeZamani = DateTime.UtcNow
            };

            _context.KurumsalSayfalar.Add(sayfa);
            await _context.SaveChangesAsync();
        }

        private static string GetSlugFromMenu(KurumsalMenu menu)
        {
            if (!string.IsNullOrWhiteSpace(menu.Url))
            {
                var clean = menu.Url.Trim().Trim('/');
                var parts = clean.Split('/', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length >= 3 && parts[0].Equals("Kurumsal", StringComparison.OrdinalIgnoreCase) && parts[1].Equals("Sayfa", StringComparison.OrdinalIgnoreCase))
                    return Slugify(parts[2]);

                if (parts.Length >= 2 && parts[0].Equals("Kurumsal", StringComparison.OrdinalIgnoreCase))
                    return Slugify(parts[1]);
            }

            return Slugify(menu.Baslik);
        }

        private static string Slugify(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "sayfa";

            text = text.Trim().ToLowerInvariant()
                .Replace("c", "c")
                .Replace("g", "g")
                .Replace("i", "i")
                .Replace("o", "o")
                .Replace("s", "s")
                .Replace("u", "u")
                .Replace("C", "c")
                .Replace("G", "g")
                .Replace("I", "i")
                .Replace("O", "o")
                .Replace("S", "s")
                .Replace("U", "u");

            var sb = new StringBuilder();
            var prevDash = false;

            foreach (var ch in text)
            {
                if (char.IsLetterOrDigit(ch))
                {
                    sb.Append(ch);
                    prevDash = false;
                }
                else if (!prevDash)
                {
                    sb.Append('-');
                    prevDash = true;
                }
            }

            return sb.ToString().Trim('-');
        }

        // ==========================================
        // GENEL KURUMSAL SEKME YÖNETİMİ
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Hakkimizda()
        {
            // Örnek verileri temizle
            var silinecekler = new string[] { "Tarihçemiz", "Misyon & Vizyon", "Stratejik Plan & Raporlar", "Yönetmelik", "Amaç ve Hedeflerimiz", "Sağlık Rehberleri", "Hastanemiz Uyum Rehberi", "Eğitim Komitesi" };
            var ornekSekmeler = await _context.KurumsalSekmeler
                .Where(x => silinecekler.Contains(x.Baslik))
                .ToListAsync();

            if (ornekSekmeler.Any())
            {
                _context.KurumsalSekmeler.RemoveRange(ornekSekmeler);
                await _context.SaveChangesAsync();
            }

            var model = await _context.KurumsalSekmeler
                .Where(x => x.SayfaKey == "hakkimizda" || x.SayfaKey == "Hakkimizda" || x.SayfaKey == "Index")
                .OrderBy(x => x.Sira)
                .ToListAsync();
            
            ViewBag.SayfaKey = "hakkimizda";
            ViewBag.SayfaBaslik = "Hakkımızda Sayfası Yönetimi";
            return View("GenericSekmeYonetim", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KurumsalSekmeKaydet(KurumsalSekme model)
        {
            model.Baslik ??= "";
            model.SekmeId ??= "";
            model.Icerik ??= "";
            model.IconClass ??= "";
            model.SayfaKey ??= "";

            if (model.Id == 0) _context.KurumsalSekmeler.Add(model);
            else _context.KurumsalSekmeler.Update(model);
            
            await _context.SaveChangesAsync();
            TempData["Success"] = "Değişiklikler kaydedildi.";
            return RedirectToAction(model.SayfaKey);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KurumsalSekmeSil(int id)
        {
            var item = await _context.KurumsalSekmeler.FindAsync(id);
            if (item != null)
            {
                var sayfaKey = item.SayfaKey;
                _context.KurumsalSekmeler.Remove(item);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Kayıt silindi.";
                return RedirectToAction(sayfaKey);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KurumsalIcerikKaydet(KurumsalIcerik model, IFormFile? FormDosyasi, string? ExistingMedya)
        {
            model.SayfaKey ??= "";
            model.Kategori ??= "";
            model.IcerikTipi = string.IsNullOrWhiteSpace(model.IcerikTipi) ? "Form" : model.IcerikTipi.Trim();
            model.Baslik ??= "";
            model.AltBaslik ??= "";
            model.Aciklama ??= "";
            model.MedyaYolu ??= ExistingMedya ?? "";
            model.VideoUrl = string.IsNullOrWhiteSpace(model.VideoUrl) ? "" : model.VideoUrl.Trim();

            if (model.IcerikTipi.Equals("Video", StringComparison.OrdinalIgnoreCase) && string.IsNullOrWhiteSpace(model.VideoUrl))
            {
                TempData["Error"] = "Video içerikleri için video URL zorunludur.";
                return RedirectToAction(model.SayfaKey);
            }

            if (!model.IcerikTipi.Equals("Video", StringComparison.OrdinalIgnoreCase))
            {
                model.VideoUrl = "";
            }

            // Dosya yüklenmişse işle
            if (FormDosyasi != null && FormDosyasi.Length > 0)
            {
                try
                {
                    // Yükleme klasörünü oluştur
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "forms");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Dosya adını güvenli ve kısa hale getir (GUID ekle)
                    string fileExtension = Path.GetExtension(FormDosyasi.FileName);
                    string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Dosyayı kaydet
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await FormDosyasi.CopyToAsync(fileStream);
                    }

                    // Veritabanında tutulacak yolu ayarla (web root'tan itibaren)
                    model.MedyaYolu = $"/uploads/forms/{uniqueFileName}";
                }
                catch
                {
                    TempData["Error"] = "Dosya yüklenirken hata oluştu.";
                    return RedirectToAction(model.SayfaKey);
                }
            }

            if (model.Id == 0) _context.KurumsalIcerikler.Add(model);
            else _context.KurumsalIcerikler.Update(model);
            
            await _context.SaveChangesAsync();
            TempData["Success"] = "Değişiklikler kaydedildi.";
            return RedirectToAction(model.SayfaKey);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KurumsalIcerikSil(int id)
        {
            var item = await _context.KurumsalIcerikler.FindAsync(id);
            if (item != null)
            {
                var sayfaKey = item.SayfaKey;
                
                // Dosya varsa sil
                if (!string.IsNullOrWhiteSpace(item.MedyaYolu) && item.MedyaYolu.StartsWith("/uploads/forms/"))
                {
                    try
                    {
                        string filePath = Path.Combine(_hostEnvironment.WebRootPath, item.MedyaYolu.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    catch
                    {
                        // Dosya silinmezse de kayıt silmeye devam et
                    }
                }
                
                _context.KurumsalIcerikler.Remove(item);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Kayıt silindi.";
                return RedirectToAction(sayfaKey);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetIcerikler(string sayfaKey, string kategori)
        {
            var icerikler = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == sayfaKey && x.Kategori == kategori)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            return PartialView("_IcerikListesi", icerikler);
        }
    }
}
