using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GaziHastane.Data;
using GaziHastane.Models;
using System.Collections.Generic;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BorcYonetimController : Controller
    {
        private readonly GaziHastaneContext _context;

        public BorcYonetimController(GaziHastaneContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                // Menü linkini sadece ilk girişte tabloya otomatik eklemek için küçük bir seed logic
                var count = _context.Database.ExecuteSqlRaw("INSERT INTO \"AdminMenuItems\" (\"Section\", \"Url\", \"Label\", \"IconClass\", \"SortOrder\", \"IsSuperAdminOnly\", \"IsActive\") SELECT 'Main', '/Admin/BorcYonetim/Index', 'Borç Yönetimi', 'fa-solid fa-money-bill-wave', 99, false, true WHERE NOT EXISTS (SELECT 1 FROM \"AdminMenuItems\" WHERE \"Url\" = '/Admin/BorcYonetim/Index');");
            }
            catch { /* Ignore */ }

            var borclar = _context.BorclarOdemeler
                .Include(b => b.Hasta)
                .OrderByDescending(b => b.Id)
                .ToList();

            return View(borclar);
        }

        public IActionResult BorcEkle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TcSorgu(string tcKimlik)
        {
            if (string.IsNullOrWhiteSpace(tcKimlik))
            {
                return Json(new { success = false, message = "T.C. Kimlik numarası boş olamaz." });
            }

            var user = _context.Users.FirstOrDefault(u => u.TCKimlikNo == tcKimlik);

            if (user != null)
            {
                return Json(new { success = true, id = user.Id, ad = user.Ad, soyad = user.Soyad, tcKimlik = user.TCKimlikNo });
            }

            return Json(new { success = false, message = "Kayıt bulunamadı." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BorcEkle(int HastaId, string IslemTipi, decimal Tutar)
        {
            if (HastaId <= 0 || string.IsNullOrWhiteSpace(IslemTipi) || Tutar <= 0)
            {
                TempData["Error"] = "Lütfen tüm zorunlu alanları doldurun.";
                return RedirectToAction(nameof(BorcEkle));
            }

            var count = _context.BorclarOdemeler.Count() + 1;
            string protakolNo = $"GZ-{DateTime.Now.Year}-{count.ToString("D4")}";

            var yeniBorc = new BorcOdeme
            {
                HastaId = HastaId,
                IslemTipi = IslemTipi,
                Tutar = Tutar,
                OdendiMi = false,
                SonOdemeTarihi = null,
                EklenmeTarihi = DateTime.Now,
                ProtakolNo = protakolNo
            };

            _context.BorclarOdemeler.Add(yeniBorc);
            _context.SaveChanges();

            TempData["Success"] = $"Borç eklendi. Protokol No: {protakolNo}";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BorcSil(int id)
        {
            var borc = _context.BorclarOdemeler.Find(id);
            if (borc != null)
            {
                _context.BorclarOdemeler.Remove(borc);
                _context.SaveChanges();
                TempData["Success"] = "Borç kaydı silindi.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
