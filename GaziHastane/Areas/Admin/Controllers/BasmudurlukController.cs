using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BasmudurlukController : Controller
    {
        private readonly GaziHastaneContext _context;

        public BasmudurlukController(GaziHastaneContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var personeller = await _context.BasmudurlikPersoneller
                .Where(x => x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            return View(personeller);
        }

        [HttpGet]
        public async Task<IActionResult> PersonelForm(int? id)
        {
            if (id == null)
                return View(new BasmudurlikPersonel());

            var personel = await _context.BasmudurlikPersoneller.FindAsync(id.Value);
            if (personel == null)
                return NotFound();

            return View(personel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PersonelForm(BasmudurlikPersonel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Eðer bu IsBasmudur=true olarak kaydediliyorsa, diðer baþmüdürleri kontrol et
            if (model.IsBasmudur && model.Id == 0)
            {
                var mevcutBasmudur = await _context.BasmudurlikPersoneller
                    .FirstOrDefaultAsync(x => x.IsBasmudur && x.AktifMi);
                
                if (mevcutBasmudur != null)
                {
                    mevcutBasmudur.IsBasmudur = false;
                    _context.BasmudurlikPersoneller.Update(mevcutBasmudur);
                }
            }

            if (model.Id == 0)
                _context.BasmudurlikPersoneller.Add(model);
            else
                _context.BasmudurlikPersoneller.Update(model);

            await _context.SaveChangesAsync();
            TempData["Success"] = "Personel baþarýyla kaydedildi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            var personel = await _context.BasmudurlikPersoneller.FindAsync(id);
            if (personel != null)
            {
                personel.AktifMi = false;
                _context.BasmudurlikPersoneller.Update(personel);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Personel silindi.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

