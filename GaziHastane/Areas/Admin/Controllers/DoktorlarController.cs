using Microsoft.AspNetCore.Mvc;
using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DoktorlarController : Controller
    {
        private readonly GaziHastaneContext _context;

        public DoktorlarController(GaziHastaneContext context)
        {
            _context = context;
        }

        // Listeleme (READ)
        public async Task<IActionResult> Index()
        {
            var doktorlar = await _context.Doktorlar
                                          .Include(d => d.Bolum) // İlişkili veriyi de yükle
                                          .ToListAsync();
            return View(doktorlar);
        }

        // Ekleme Sayfası (GET)
        public IActionResult Create()
        {
            ViewData["BolumId"] = new SelectList(_context.Bolumler, "Id", "Ad");
            return View();
        }

        // Ekleme İşlemi (POST) - Veritabanına Kaydetme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ad,Soyad,BolumId,Unvan,UzmanlikAlani")] Doktor doktor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doktor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BolumId"] = new SelectList(_context.Bolumler, "Id", "Ad", doktor.BolumId);
            return View(doktor);
        }
    }
}