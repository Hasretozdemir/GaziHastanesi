using System;
using System.Linq;
using System.Security.Claims;
using GaziHastane.Data;
using GaziHastane.Models;
using GaziHastane.Security;
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
        public IActionResult Create()
        {
            ViewBag.AdminSayfaYetkileri = AdminPanelPermissions.All;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Yetkili yetkili)
        {
            if (ModelState.IsValid)
            {
                yetkili.KayitTarihi = DateTime.Now;
                yetkili.AdminSayfaYetkileri = yetkili.Rol == "Süper Admin"
                    ? AdminPanelPermissions.Serialize(AdminPanelPermissions.AllKeys)
                    : AdminPanelPermissions.Serialize(yetkili.SecilenSayfaYetkileri);

                _context.Yetkililer.Add(yetkili);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AdminSayfaYetkileri = AdminPanelPermissions.All;
            return View(yetkili);
        }

        // DÜZENLEME SAYFASINI AÇAR
        public IActionResult Edit(int id)
        {
            var yetkili = _context.Yetkililer.Find(id);
            if (yetkili == null) return NotFound();

            yetkili.SecilenSayfaYetkileri = AdminPanelPermissions.Parse(yetkili.AdminSayfaYetkileri).ToList();
            ViewBag.AdminSayfaYetkileri = AdminPanelPermissions.All;
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
            mevcut.AdminSayfaYetkileri = yetkili.Rol == "Süper Admin"
                ? AdminPanelPermissions.Serialize(AdminPanelPermissions.AllKeys)
                : AdminPanelPermissions.Serialize(yetkili.SecilenSayfaYetkileri);

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

        public IActionResult Profil()
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? User.Identity?.Name;

            Yetkili? model = null;
            if (!string.IsNullOrWhiteSpace(email))
            {
                model = _context.Yetkililer.FirstOrDefault(x => x.Email == email);
            }

            model ??= _context.Yetkililer.OrderByDescending(x => x.Id).FirstOrDefault();
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
    }
}