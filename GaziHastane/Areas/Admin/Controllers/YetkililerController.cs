using System;
using System.Linq;
using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class YetkililerController : Controller
    {
        private readonly GaziHastaneContext _context;
        public YetkililerController(GaziHastaneContext context) { _context = context; }

        public IActionResult Index()
        {
            return View(_context.Yetkililer.OrderByDescending(y => y.KayitTarihi).ToList());
        }

        // EKLEME SAYFASINI AÇAR
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Yetkili yetkili)
        {
            if (ModelState.IsValid)
            {
                yetkili.KayitTarihi = DateTime.Now;
                _context.Yetkililer.Add(yetkili);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(yetkili);
        }

        // DÜZENLEME SAYFASINI AÇAR
        public IActionResult Edit(int id)
        {
            var yetkili = _context.Yetkililer.Find(id);
            if (yetkili == null) return NotFound();
            return View(yetkili);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Yetkili yetkili)
        {
            var mevcut = _context.Yetkililer.Find(yetkili.Id);
            if (mevcut == null) return NotFound();

            mevcut.AdSoyad = yetkili.AdSoyad;
            mevcut.Email = yetkili.Email;
            mevcut.Rol = yetkili.Rol;
            mevcut.IsActive = yetkili.IsActive;

            if (!string.IsNullOrEmpty(yetkili.SifreHash))
                mevcut.SifreHash = yetkili.SifreHash;

            _context.Update(mevcut);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var yetkili = _context.Yetkililer.Find(id);
            if (yetkili != null) { _context.Yetkililer.Remove(yetkili); _context.SaveChanges(); }
            return RedirectToAction(nameof(Index));
        }
    }
}