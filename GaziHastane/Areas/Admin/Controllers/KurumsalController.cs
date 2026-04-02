using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
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

        public KurumsalController(GaziHastaneContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
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

        [HttpGet]
        public IActionResult ArsivBirimi() => View();

        [HttpGet]
        public IActionResult BasinVeKurumsalIletisim() => View();

        [HttpGet]
        public IActionResult BilgiIslemMerkezi() => View();

        [HttpGet]
        public IActionResult EczacilikHizmetleri() => View();

        [HttpGet]
        public IActionResult EnfeksiyonKontrol() => View();

        [HttpGet]
        public IActionResult HastaIletisimBirimi() => View();

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
            TempData["Success"] = "Ayarlar baþarýyla kaydedildi.";
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

            if (model.Id == 0)
            {
                _context.HemsirelikIcerikler.Add(model);
            }
            else
            {
                _context.HemsirelikIcerikler.Update(model);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Ýçerik baþarýyla kaydedildi.";
            return RedirectToAction(nameof(HemsirelikHizmetleri));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HemsirelikIcerikSil(int id)
        {
            var icerik = await _context.HemsirelikIcerikler.FindAsync(id);
            if (icerik != null)
            {
                icerik.AktifMi = false;
                _context.HemsirelikIcerikler.Update(icerik);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Ýçerik silindi.";
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

            if (model.Id == 0)
            {
                _context.HemsirelikSekmeler.Add(model);
            }
            else
            {
                _context.HemsirelikSekmeler.Update(model);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Sekme baþarýyla kaydedildi.";
            return Redirect($"{Url.Action(nameof(HemsirelikHizmetleri))}#tab-sekmeler");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HemsirelikSekmeSil(int id)
        {
            var sekme = await _context.HemsirelikSekmeler.FindAsync(id);
            if (sekme != null)
            {
                sekme.AktifMi = false;
                _context.HemsirelikSekmeler.Update(sekme);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Sekme silindi.";
            }

            return Redirect($"{Url.Action(nameof(HemsirelikHizmetleri))}#tab-sekmeler");
        }

        [HttpGet]
        public IActionResult IcKontrol() => View();

        [HttpGet]
        public IActionResult IsAkisSemalari() => View();

        [HttpGet]
        public IActionResult IsSagligiVeGuvenligi() => View();

        [HttpGet]
        public IActionResult IstatistikVeRaporlama() => View();

        [HttpGet]
        public IActionResult OrganizasyonSemalari() => View();

        [HttpGet]
        public IActionResult SatinAlma() => View();

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

            // URL boþ býrakýlýrsa otomatik dinamik kurumsal sayfa URL'si üret
            if (string.IsNullOrWhiteSpace(model.Url))
            {
                var slugFromTitle = Slugify(model.Baslik);
                model.Url = $"/Kurumsal/Sayfa/{slugFromTitle}";
            }

            if (model.Id == 0) _context.KurumsalMenuler.Add(model);
            else _context.KurumsalMenuler.Update(model);

            await _context.SaveChangesAsync();

            // Menü kaydý sonrasý ilgili sayfa kaydý yoksa otomatik oluþtur
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
                    Icerik = "<p>Bu sayfa içeriðini buradan düzenleyebilirsiniz.</p>",
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
            TempData["Success"] = "Sayfa içeriði güncellendi.";
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
                Icerik = "<p>Bu sayfa içeriðini buradan düzenleyebilirsiniz.</p>",
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
                .Replace('ç', 'c')
                .Replace('ð', 'g')
                .Replace('ý', 'i')
                .Replace('ö', 'o')
                .Replace('þ', 's')
                .Replace('ü', 'u');

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
    }
}
