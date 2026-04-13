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
        private readonly IWebHostEnvironment _env;
        private readonly GaziHastaneContext _context;

        public EtkinliklerController(IWebHostEnvironment env, GaziHastaneContext context)
        {
            _env = env;
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
                if (etkinlik.GorselDosya != null && etkinlik.GorselDosya.Length > 0)
                {
                    etkinlik.GorselUrl = EtkinlikGorseliYukle(etkinlik.GorselDosya);
                }

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
                var mevcut = _context.Etkinlikler.Find(etkinlik.Id);
                if (mevcut == null) return NotFound();

                mevcut.Baslik = etkinlik.Baslik;
                mevcut.EtkinlikTipi = etkinlik.EtkinlikTipi;
                mevcut.Tarih = etkinlik.Tarih;
                mevcut.SaatAraligi = etkinlik.SaatAraligi;
                mevcut.Konum = etkinlik.Konum;
                mevcut.Aciklama = etkinlik.Aciklama;
                mevcut.ModalIcerik = etkinlik.ModalIcerik;
                mevcut.IsActive = etkinlik.IsActive;

                if (etkinlik.GorselDosya != null && etkinlik.GorselDosya.Length > 0)
                {
                    mevcut.GorselUrl = EtkinlikGorseliYukle(etkinlik.GorselDosya);
                }

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

        private string EtkinlikGorseliYukle(IFormFile gorselDosya)
        {
            var uploadPath = Path.Combine(_env.WebRootPath, "uploads", "etkinlikler");
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

            var ext = Path.GetExtension(gorselDosya.FileName);
            var fileName = Guid.NewGuid() + ext;
            var fullPath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                gorselDosya.CopyTo(stream);
            }

            return "/uploads/etkinlikler/" + fileName;
        }
    }
}