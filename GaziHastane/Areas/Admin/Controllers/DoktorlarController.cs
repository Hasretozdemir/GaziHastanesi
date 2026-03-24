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
            // ViewBag adını 'Bolumler' yaptık.
            // Show all poliklinikler in admin dropdown (do not filter by IsActive)
            ViewBag.Bolumler = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Bolumler.OrderBy(b => b.Ad).ToList(), "Id", "Ad");
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

            // Eğer formda hata varsa dropdown boş gelmesin diye burayı da 'Bolumler' yapıyoruz
            ViewBag.Bolumler = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Bolumler.OrderBy(b => b.Ad).ToList(), "Id", "Ad", doktor.BolumId);
            return View(doktor);
        }

        // --- DÜZENLEME (EDIT) İŞLEMLERİ ---

        // Düzenle Ekranını Açma (GET) - Verilerin dolu gelmesini sağlayan kısım
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var doktor = await _context.Doktorlar.FindAsync(id);
            if (doktor == null) return NotFound();

            // DİKKAT: ViewBag.BolumId yerine ViewBag.Bolumler yapıldı
            ViewBag.Bolumler = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Bolumler.Where(b => b.IsActive), "Id", "Ad", doktor.BolumId);
            return View(doktor);
        }

        // Formdan Gelen Güncel Veriyi Kaydetme (POST) - Değiştir dediğinizde çalışan kısım
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
                catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
                {
                    if (!_context.Doktorlar.Any(e => e.Id == doktor.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index)); // Başarılıysa listeye dön
            }

            // Hata varsa dropdown boş gelmesin
            ViewBag.Bolumler = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Bolumler.Where(b => b.IsActive), "Id", "Ad", doktor.BolumId);
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

