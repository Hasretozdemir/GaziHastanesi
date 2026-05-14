using System.Threading.Tasks;
using GaziHastane.Data; 
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DoktorlarController : Controller
    {
        private readonly GaziHastaneContext _context;
        private readonly ILogger<DoktorlarController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        public DoktorlarController(GaziHastaneContext context, ILogger<DoktorlarController> logger, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        // DOKTORLARI LİSTELEME
        public async Task<IActionResult> Index()
        {
            // Doktorları çekerken bağlı oldukları Bolum'leri de (.Include ile) çekiyoruz ki tabloda adını yazabilelim.
            var doktorlar = await _context.Doktorlar
                .Include(d => d.Bolum)
                .ToListAsync();

            return View(doktorlar);
        }

        // Ekleme Ekranını Açma (GET)
        public IActionResult Create()
        {
            PopulateBolumlerViewBag();
            return View();
        }

        // Formdan Gelen Veriyi Kaydetme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doktor doktor, string? kategoriSecim)
        {
            ValidateBolumKategori(doktor.BolumId, kategoriSecim);

            if (ModelState.IsValid)
            {
                // Dosya Yükleme İşlemi
                if (doktor.GorselDosya != null && doktor.GorselDosya.Length > 0)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(doktor.GorselDosya.FileName);
                    string path = Path.Combine(wwwRootPath, "uploads", "doktorlar");
                    
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                    using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        await doktor.GorselDosya.CopyToAsync(fileStream);
                    }
                    doktor.FotografUrl = "/uploads/doktorlar/" + fileName;
                }

                _context.Add(doktor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateBolumlerViewBag(kategoriSecim);
            return View(doktor);
        }

        // --- DÜZENLEME (EDIT) İŞLEMLERİ ---

        // Düzenle Ekranını Açma (GET) - Verilerin dolu gelmesini sağlayan kısım
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var doktor = await _context.Doktorlar.FindAsync(id);
            if (doktor == null) return NotFound();

            var seciliKategori = doktor.BolumId.HasValue
                ? _context.Bolumler.Where(b => b.Id == doktor.BolumId.Value).Select(b => b.Kategori).FirstOrDefault()
                : null;

            PopulateBolumlerViewBag(seciliKategori);
            return View(doktor);
        }

        // Formdan Gelen Güncel Veriyi Kaydetme (POST) - Değiştir dediğinizde çalışan kısım
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Doktor doktor, string? kategoriSecim)
        {
            if (id != doktor.Id) return NotFound();

            ValidateBolumKategori(doktor.BolumId, kategoriSecim);

            if (ModelState.IsValid)
            {
                try
                {
                    // Dosya Yükleme İşlemi (Yeni dosya seçilmişse)
                    if (doktor.GorselDosya != null && doktor.GorselDosya.Length > 0)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(doktor.GorselDosya.FileName);
                        string path = Path.Combine(wwwRootPath, "uploads", "doktorlar");

                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                        // Eski dosyayı silme (İsteğe bağlı, temizlik için)
                        if (!string.IsNullOrEmpty(doktor.FotografUrl))
                        {
                            var oldPath = Path.Combine(wwwRootPath, doktor.FotografUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                        }

                        using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                        {
                            await doktor.GorselDosya.CopyToAsync(fileStream);
                        }
                        doktor.FotografUrl = "/uploads/doktorlar/" + fileName;
                    }

                    _context.Update(doktor);
                    await _context.SaveChangesAsync();
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
                {
                    if (!_context.Doktorlar.Any(e => e.Id == doktor.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            PopulateBolumlerViewBag(kategoriSecim);
            return View(doktor);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var doktor = await _context.Doktorlar
                .Include(d => d.Bolum) // Bölüm adını ekranda göstermek için dahil ediyoruz
                .FirstOrDefaultAsync(m => m.Id == id);

            if (doktor == null) return NotFound();

            return View(doktor);
        }

        // Silme İşlemini Onaylama (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var doktor = await _context.Doktorlar.FindAsync(id);
                if (doktor != null)
                {
                    var kullaniciAdi = User.Identity?.Name ?? "Bilinmiyor";
                    var ipAdresi = HttpContext.Connection.RemoteIpAddress?.ToString();

                    _context.Doktorlar.Remove(doktor);

                    await _context.SaveChangesAsync();

                    _logger.LogInformation("{Kullanici} kullanıcısı {Id} nolu doktoru sildi. IP: {Ip}", kullaniciAdi, id, ipAdresi);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Doktor silinirken hata oluştu. ID: {Id}", id);
                TempData["Error"] = "Doktor kaydı silinirken bir hata oluştu.";
                return RedirectToAction(nameof(Index));
            }
        }

        private void PopulateBolumlerViewBag(string? seciliKategori = null)
        {
            var bolumler = _context.Bolumler
                .OrderBy(b => b.Ad)
                .Select(b => new { b.Id, b.Ad, b.Kategori })
                .ToList();

            ViewBag.Bolumler = bolumler;
            ViewBag.KategoriSecim = seciliKategori;
        }

        private void ValidateBolumKategori(int? bolumId, string? kategoriSecim)
        {
            if (!bolumId.HasValue)
            {
                ModelState.AddModelError("BolumId", "Lütfen bir kategoriye ait poliklinik seçiniz.");
                return;
            }

            if (string.IsNullOrWhiteSpace(kategoriSecim))
            {
                ModelState.AddModelError("", "Lütfen kategori seçiniz (Dahili, Cerrahi, Temel).");
                return;
            }

            var bolumKategori = _context.Bolumler
                .Where(b => b.Id == bolumId.Value)
                .Select(b => b.Kategori)
                .FirstOrDefault();

            if (!string.Equals(bolumKategori, kategoriSecim, System.StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("BolumId", "Seçilen poliklinik, seçtiğiniz kategori ile uyuşmuyor.");
            }
        }
    }
}

