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

        public YetkililerController(GaziHastaneContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var yetkililer = _context.Yetkililer.OrderByDescending(y => y.KayitTarihi).ToList();
            return View(yetkililer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Yetkili yetkili)
        {
            if (ModelState.IsValid)
            {
                _context.Yetkililer.Add(yetkili);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(yetkili);
        }

        // Edit, Delete işlemleri vb. daha önceki tablolarla aynı mantıkta ilerler...
    }
}