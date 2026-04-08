using System.Linq;
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
    public class IletisimController : Controller
    {
        private readonly GaziHastaneContext _context;

        public IletisimController(GaziHastaneContext context)
        {
            _context = context;
        }

        // 1. LİSTELEME
        public async Task<IActionResult> Index()
        {
            var iletisimBilgileri = await _context.Set<Iletisim>().ToListAsync();
            ViewBag.UlasimRehberleri = await _context.Set<UlasimRehberi>().OrderBy(x => x.UlasimTipi).ToListAsync();
            return View(iletisimBilgileri);
        }

        // 2. EKLEME
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Iletisim iletisim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(iletisim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(iletisim);
        }

        // 3. DÜZENLEME
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var iletisim = await _context.Set<Iletisim>().FindAsync(id);
            if (iletisim == null) return NotFound();
            return View(iletisim);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Iletisim iletisim)
        {
            if (id != iletisim.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(iletisim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Set<Iletisim>().Any(e => e.Id == iletisim.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(iletisim);
        }

        // 4. SİLME
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var iletisim = await _context.Set<Iletisim>().FirstOrDefaultAsync(m => m.Id == id);
            if (iletisim == null) return NotFound();
            return View(iletisim);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var iletisim = await _context.Set<Iletisim>().FindAsync(id);
            if (iletisim != null)
            {
                _context.Set<Iletisim>().Remove(iletisim);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult UlasimCreate()
        {
            return View(new UlasimRehberi { TemaRengi = "amber", IsActive = true, Ikon = "fa-solid fa-bus" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UlasimCreate(UlasimRehberi model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> UlasimEdit(int? id)
        {
            if (id == null) return NotFound();
            var kayit = await _context.Set<UlasimRehberi>().FindAsync(id);
            if (kayit == null) return NotFound();
            return View(kayit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UlasimEdit(int id, UlasimRehberi model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> UlasimDelete(int? id)
        {
            if (id == null) return NotFound();
            var kayit = await _context.Set<UlasimRehberi>().FirstOrDefaultAsync(x => x.Id == id);
            if (kayit == null) return NotFound();
            return View(kayit);
        }

        [HttpPost, ActionName("UlasimDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UlasimDeleteConfirmed(int id)
        {
            var kayit = await _context.Set<UlasimRehberi>().FindAsync(id);
            if (kayit != null)
            {
                _context.Set<UlasimRehberi>().Remove(kayit);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}