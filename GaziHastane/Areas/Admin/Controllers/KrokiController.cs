using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class KrokiController : Controller
    {
        private readonly GaziHastaneContext _context;

        public KrokiController(GaziHastaneContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bloklar = await _context.KrokiBloklar
                .Include(b => b.Katlar)
                    .ThenInclude(k => k.Bolumler)
                .ToListAsync();
            return View(bloklar);
        }

        // --- BLOK ---
        [HttpGet]
        public async Task<IActionResult> BlokEkle() => View("BlokForm", new KrokiBlok { Renk = "#0ea5e9" });

        [HttpGet]
        public async Task<IActionResult> BlokDuzenle(int id)
        {
            var model = await _context.KrokiBloklar.FindAsync(id);
            if (model == null) return NotFound();
            return View("BlokForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlokKaydet(KrokiBlok model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0) _context.KrokiBloklar.Add(model);
                else _context.KrokiBloklar.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Blok kaydedildi.";
                return RedirectToAction("Index");
            }
            return View("BlokForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlokSil(int id)
        {
            var veri = await _context.KrokiBloklar.FindAsync(id);
            if (veri != null) { _context.KrokiBloklar.Remove(veri); await _context.SaveChangesAsync(); }
            TempData["Success"] = "Blok silindi.";
            return RedirectToAction("Index");
        }

        // --- KAT ---
        [HttpGet]
        public async Task<IActionResult> KatEkle(int blokId) => View("KatForm", new KrokiKat { BlokId = blokId });

        [HttpGet]
        public async Task<IActionResult> KatDuzenle(int id)
        {
            var model = await _context.KrokiKatlar.FindAsync(id);
            if (model == null) return NotFound();
            return View("KatForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KatKaydet(KrokiKat model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0) _context.KrokiKatlar.Add(model);
                else _context.KrokiKatlar.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Kat kaydedildi.";
                return RedirectToAction("Index");
            }
            return View("KatForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KatSil(int id)
        {
            var veri = await _context.KrokiKatlar.FindAsync(id);
            if (veri != null) { _context.KrokiKatlar.Remove(veri); await _context.SaveChangesAsync(); }
            TempData["Success"] = "Kat silindi.";
            return RedirectToAction("Index");
        }

        // --- BÖLÜM ---
        [HttpGet]
        public async Task<IActionResult> BolumEkle(int katId) => View("BolumForm", new KrokiBolum { KatId = katId, Ikon = "fa-door-closed" });

        [HttpGet]
        public async Task<IActionResult> BolumDuzenle(int id)
        {
            var model = await _context.KrokiBolumler.FindAsync(id);
            if (model == null) return NotFound();
            return View("BolumForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BolumKaydet(KrokiBolum model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Ikon)) model.Ikon = "fa-layer-group";
                if (model.Id == 0) _context.KrokiBolumler.Add(model);
                else _context.KrokiBolumler.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Bölüm kaydedildi.";
                return RedirectToAction("Index");
            }
            return View("BolumForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BolumSil(int id)
        {
            var veri = await _context.KrokiBolumler.FindAsync(id);
            if (veri != null) { _context.KrokiBolumler.Remove(veri); await _context.SaveChangesAsync(); }
            TempData["Success"] = "Bölüm silindi.";
            return RedirectToAction("Index");
        }
    }
}