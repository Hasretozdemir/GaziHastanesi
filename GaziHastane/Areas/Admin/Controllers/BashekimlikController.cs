using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BashekimlikController : Controller
    {
        private readonly GaziHastaneContext _context;

        public BashekimlikController(GaziHastaneContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var personeller = await _context.BashekimlikPersoneller
                .Where(x => x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            return View(personeller);
        }

        [HttpGet]
        public async Task<IActionResult> PersonelForm(int? id)
        {
            if (id == null)
                return View(new BashekimlikPersonel());

            var personel = await _context.BashekimlikPersoneller.FindAsync(id.Value);
            if (personel == null)
                return NotFound();

            return View(personel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PersonelForm(BashekimlikPersonel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Eðer bu IsBashekim=true olarak kaydediliyorsa, diðer baþhekimleri kontrol et
            if (model.IsBashekim && model.Id == 0)
            {
                var mevcutBashekim = await _context.BashekimlikPersoneller
                    .FirstOrDefaultAsync(x => x.IsBashekim && x.AktifMi);
                
                if (mevcutBashekim != null)
                {
                    mevcutBashekim.IsBashekim = false;
                    _context.BashekimlikPersoneller.Update(mevcutBashekim);
                }
            }

            if (model.Id == 0)
                _context.BashekimlikPersoneller.Add(model);
            else
                _context.BashekimlikPersoneller.Update(model);

            await _context.SaveChangesAsync();
            TempData["Success"] = "Personel baþarýyla kaydedildi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            var personel = await _context.BashekimlikPersoneller.FindAsync(id);
            if (personel != null)
            {
                personel.AktifMi = false;
                _context.BashekimlikPersoneller.Update(personel);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Personel silindi.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
