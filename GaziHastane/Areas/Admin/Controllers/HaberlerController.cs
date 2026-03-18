using System.Linq;
using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]// Bu çok önemli, admin klasöründe olduğunu belirtir
    public class HaberlerController : Controller
    {
        private readonly GaziHastaneContext _context;

        public HaberlerController(GaziHastaneContext context)
        {
            _context = context;
        }

        // LİSTELEME
        public IActionResult Index()
        {
            var haberler = _context.Haberler.OrderByDescending(h => h.YayinTarihi).ToList();
            return View(haberler);
        }

        // EKLEME (Sayfayı Açma)
        public IActionResult Create()
        {
            return View();
        }

        // EKLEME (Formu Gönderme)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Haber haber)
        {
            if (ModelState.IsValid)
            {
                _context.Haberler.Add(haber);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(haber);
        }

        // GÜNCELLEME (Sayfayı Açma)
        public IActionResult Edit(int id)
        {
            var haber = _context.Haberler.Find(id);
            if (haber == null) return NotFound();
            return View(haber);
        }

        // GÜNCELLEME (Formu Gönderme)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Haber haber)
        {
            if (ModelState.IsValid)
            {
                _context.Haberler.Update(haber);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(haber);
        }

        // SİLME
        public IActionResult Delete(int id)
        {
            var haber = _context.Haberler.Find(id);
            if (haber != null)
            {
                _context.Haberler.Remove(haber);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}