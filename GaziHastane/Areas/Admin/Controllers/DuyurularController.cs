using System.Linq;
using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DuyurularController : Controller
    {
        private readonly GaziHastaneContext _context;

        public DuyurularController(GaziHastaneContext context)
        {
            _context = context;
        }

        // LİSTELEME
        public IActionResult Index()
        {
            var duyurular = _context.Duyurular.OrderByDescending(d => d.YayinTarihi).ToList();
            return View(duyurular);
        }

        // EKLEME (Sayfa)
        public IActionResult Create()
        {
            return View();
        }

        // EKLEME (Post İşlemi)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Duyuru duyuru)
        {
            if (ModelState.IsValid)
            {
                _context.Duyurular.Add(duyuru);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(duyuru);
        }

        // GÜNCELLEME (Sayfa)
        public IActionResult Edit(int id)
        {
            var duyuru = _context.Duyurular.Find(id);
            if (duyuru == null) return NotFound();
            return View(duyuru);
        }

        // GÜNCELLEME (Post İşlemi)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Duyuru duyuru)
        {
            if (ModelState.IsValid)
            {
                _context.Duyurular.Update(duyuru);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(duyuru);
        }

        // SİLME
        public IActionResult Delete(int id)
        {
            var duyuru = _context.Duyurular.Find(id);
            if (duyuru != null)
            {
                _context.Duyurular.Remove(duyuru);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}