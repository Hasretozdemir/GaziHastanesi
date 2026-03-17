using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GaziHastane.Data;
using GaziHastane.Models;
using System.Linq;
using System.Threading.Tasks;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HastaRehberiController : Controller
    {
        private readonly GaziHastaneContext _context;

        public HastaRehberiController(GaziHastaneContext context)
        {
            _context = context;
        }

        // 1. LİSTELEME
        public async Task<IActionResult> Index()
        {
            // Rehber başlıklarını Sıra Numarasına (SiraNo) göre dizerek getiriyoruz
            var rehberler = await _context.Set<HastaRehberi>().OrderBy(x => x.SiraNo).ToListAsync();
            return View(rehberler);
        }

        // 2. EKLEME (GET & POST)
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HastaRehberi hastaRehberi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hastaRehberi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hastaRehberi);
        }

        // 3. DÜZENLEME (GET & POST)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var hastaRehberi = await _context.Set<HastaRehberi>().FindAsync(id);
            if (hastaRehberi == null) return NotFound();
            return View(hastaRehberi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HastaRehberi hastaRehberi)
        {
            if (id != hastaRehberi.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hastaRehberi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Set<HastaRehberi>().Any(e => e.Id == hastaRehberi.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hastaRehberi);
        }

        // 4. SİLME (GET & POST)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var hastaRehberi = await _context.Set<HastaRehberi>().FirstOrDefaultAsync(m => m.Id == id);
            if (hastaRehberi == null) return NotFound();
            return View(hastaRehberi);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hastaRehberi = await _context.Set<HastaRehberi>().FindAsync(id);
            if (hastaRehberi != null)
            {
                _context.Set<HastaRehberi>().Remove(hastaRehberi);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}