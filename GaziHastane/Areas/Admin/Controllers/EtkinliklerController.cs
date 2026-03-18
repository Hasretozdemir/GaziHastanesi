using System.Linq;
using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class EtkinliklerController : Controller
    {
        private readonly GaziHastaneContext _context;

        public EtkinliklerController(GaziHastaneContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var etkinlikler = _context.Etkinlikler.OrderByDescending(e => e.Tarih).ToList();
            return View(etkinlikler);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Etkinlik etkinlik)
        {
            if (ModelState.IsValid)
            {
                _context.Etkinlikler.Add(etkinlik);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(etkinlik);
        }

        public IActionResult Edit(int id)
        {
            var etkinlik = _context.Etkinlikler.Find(id);
            if (etkinlik == null) return NotFound();
            return View(etkinlik);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Etkinlik etkinlik)
        {
            if (ModelState.IsValid)
            {
                _context.Etkinlikler.Update(etkinlik);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(etkinlik);
        }

        public IActionResult Delete(int id)
        {
            var etkinlik = _context.Etkinlikler.Find(id);
            if (etkinlik != null)
            {
                _context.Etkinlikler.Remove(etkinlik);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}