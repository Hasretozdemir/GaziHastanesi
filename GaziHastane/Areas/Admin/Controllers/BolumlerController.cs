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
    public class BolumlerController : Controller
    {
        private readonly GaziHastaneContext _context;

        public BolumlerController(GaziHastaneContext context)
        {
            _context = context;
        }

        // 1. LİSTELEME
        public async Task<IActionResult> Index()
        {
            var bolumler = await _context.Set<Bolum>().ToListAsync();
            return View(bolumler);
        }

        // 2. EKLEME (GET & POST)
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Bolum bolum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bolum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bolum);
        }

        // 3. DÜZENLEME (GET & POST)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var bolum = await _context.Set<Bolum>().FindAsync(id);
            if (bolum == null) return NotFound();
            return View(bolum);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Bolum bolum)
        {
            if (id != bolum.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bolum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Set<Bolum>().Any(e => e.Id == bolum.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bolum);
        }

        // 4. SİLME (GET & POST)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var bolum = await _context.Set<Bolum>().FirstOrDefaultAsync(m => m.Id == id);
            if (bolum == null) return NotFound();
            return View(bolum);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bolum = await _context.Set<Bolum>().FindAsync(id);
            if (bolum != null)
            {
                _context.Set<Bolum>().Remove(bolum);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}