using System.Threading.Tasks;
using GaziHastane.Data; 
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DoktorlarController : Controller
    {
        private readonly GaziHastaneContext _context;

        public DoktorlarController(GaziHastaneContext context)
        {
            _context = context;
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
                _context.Add(doktor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Kayıt başarılıysa listeye dön
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
                    _context.Update(doktor);
                    await _context.SaveChangesAsync();
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
                {
                    if (!_context.Doktorlar.Any(e => e.Id == doktor.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index)); // Başarılıysa listeye dön
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
            var doktor = await _context.Doktorlar.FindAsync(id);
            if (doktor != null)
            {
                _context.Doktorlar.Remove(doktor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
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

