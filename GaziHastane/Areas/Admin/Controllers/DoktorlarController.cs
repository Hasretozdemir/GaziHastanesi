using System.Threading.Tasks;
using GaziHastane.Data; 
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            // Bölümleri veritabanından çekip dropdown'da göstermek için ViewBag'e atıyoruz
            // Burada "Bolumler" senin DB setinin adıdır, eğer hata verirse altını çizen yere göre düzeltiriz.
            ViewBag.BolumId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Set<Bolum>().Where(b => b.IsActive), "Id", "Ad");
            return View();
        }

        // Formdan Gelen Veriyi Kaydetme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doktor doktor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doktor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Kayıt başarılıysa listeye dön
            }

            // Eğer formda bir hata varsa (isim boş geçildiyse vb.) dropdown boş gelmesin diye tekrar dolduruyoruz
            ViewBag.BolumId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Set<Bolum>().Where(b => b.IsActive), "Id", "Ad", doktor.BolumId);
            return View(doktor);
        }

        // --- DÜZENLEME (EDIT) İŞLEMLERİ ---

        // Düzenle Ekranını Açma (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var doktor = await _context.Doktorlar.FindAsync(id);
            if (doktor == null) return NotFound();

            // Bölümleri dropdown için çekiyoruz, mevcut bölümü seçili yapıyoruz
            ViewBag.BolumId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Set<Bolum>().Where(b => b.IsActive), "Id", "Ad", doktor.BolumId);
            return View(doktor);
        }

        // Formdan Gelen Güncel Veriyi Kaydetme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Doktor doktor)
        {
            if (id != doktor.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doktor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Doktorlar.Any(e => e.Id == doktor.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            // Hata varsa dropdown boş gelmesin
            ViewBag.BolumId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Set<Bolum>().Where(b => b.IsActive), "Id", "Ad", doktor.BolumId);
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
    }
}

