using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GaziHastane.Data;
using GaziHastane.Models;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AyarlarController : Controller
    {
        private readonly GaziHastaneContext _context;

        public AyarlarController(GaziHastaneContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var kapasite = _context.PanelAyarlari
                .Where(x => x.AyarKey == "DoktorGunlukRandevuKapasitesi")
                .Select(x => x.AyarValue)
                .FirstOrDefault() ?? "24";

            ViewBag.DoktorGunlukRandevuKapasitesi = kapasite;
            return View();
        }

        [Authorize(Roles = "Süper Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int doktorGunlukRandevuKapasitesi)
        {
            if (doktorGunlukRandevuKapasitesi < 1)
            {
                TempData["Error"] = "Kapasite en az 1 olmalýdýr.";
                return RedirectToAction(nameof(Index));
            }

            var ayar = _context.PanelAyarlari.FirstOrDefault(x => x.AyarKey == "DoktorGunlukRandevuKapasitesi");
            if (ayar == null)
            {
                _context.PanelAyarlari.Add(new PanelAyar
                {
                    AyarKey = "DoktorGunlukRandevuKapasitesi",
                    AyarValue = doktorGunlukRandevuKapasitesi.ToString()
                });
            }
            else
            {
                ayar.AyarValue = doktorGunlukRandevuKapasitesi.ToString();
            }

            _context.SaveChanges();
            TempData["Success"] = "Doktor günlük randevu kapasitesi güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Süper Admin")]
        [HttpGet]
        public IActionResult Menu()
        {
            var model = _context.AdminMenuItems
                .OrderBy(x => x.Section)
                .ThenBy(x => x.SortOrder)
                .ToList();

            return View(model);
        }

        [Authorize(Roles = "Süper Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Menu(List<AdminMenuItem> model)
        {
            var ids = model.Select(x => x.Id).ToHashSet();
            var mevcutlar = _context.AdminMenuItems.Where(x => ids.Contains(x.Id)).ToList();

            foreach (var kayit in mevcutlar)
            {
                var gelen = model.First(x => x.Id == kayit.Id);
                kayit.Label = string.IsNullOrWhiteSpace(gelen.Label) ? kayit.Label : gelen.Label.Trim();
                kayit.Url = string.IsNullOrWhiteSpace(gelen.Url) ? kayit.Url : gelen.Url.Trim();
                kayit.IconClass = gelen.IconClass?.Trim();
                kayit.Controller = gelen.Controller?.Trim();
                kayit.Action = gelen.Action?.Trim();
                kayit.ActiveIconClass = gelen.ActiveIconClass?.Trim();
                kayit.HoverIconClass = gelen.HoverIconClass?.Trim();
                kayit.SortOrder = gelen.SortOrder;
                kayit.IsActive = gelen.IsActive;
                kayit.IsSuperAdminOnly = gelen.IsSuperAdminOnly;
            }

            _context.SaveChanges();
            TempData["Success"] = "Menü ayarlarý güncellendi.";
            return RedirectToAction(nameof(Menu));
        }
    }
}
