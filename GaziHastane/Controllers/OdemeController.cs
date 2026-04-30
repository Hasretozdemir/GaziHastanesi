using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GaziHastane.Data;
using GaziHastane.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GaziHastane.Controllers
{
    public class OdemeController : Controller
    {
        private readonly GaziHastaneContext _context;

        public OdemeController(GaziHastaneContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string tcKimlik, string protokolNo)
        {
            if (string.IsNullOrEmpty(tcKimlik))
            {
                TempData["Error"] = "Lutfen T.C. Kimlik numaranizi giriniz.";
                return RedirectToAction("Giris");
            }

            var user = _context.Users.FirstOrDefault(u => u.TCKimlikNo == tcKimlik);

            if (user != null)
            {
                if (!string.IsNullOrEmpty(protokolNo))
                {
                    var protokolExists = _context.BorclarOdemeler
                        .Any(b => b.HastaId == user.Id && b.ProtakolNo == protokolNo && !b.OdendiMi);
                    if (!protokolExists)
                    {
                        TempData["Error"] = "Bu protokol numarasina ait odenmemis borc bulunamadi.";
                        return RedirectToAction("Giris");
                    }
                }
                return RedirectToAction("Icerik", new { userId = user.Id, protokol = protokolNo });
            }

            TempData["Error"] = "Girdiginiz T.C. Kimlik numarasina ait hasta kaydi bulunmamadi.";
            return RedirectToAction("Giris");
        }
        [HttpGet]
        public IActionResult Icerik(int? userId, string protokol)
        {
            if (userId == null)
            {
                return RedirectToAction("Giris");
            }

            var aktifKullanici = _context.Users.Find(userId);
            ViewBag.KullaniciAdSoyad = aktifKullanici?.Ad.ToUpper() + " " + aktifKullanici?.Soyad.ToUpper();
            ViewBag.Protokol = string.IsNullOrEmpty(protokol) ? "Bilinmiyor" : protokol;
            ViewBag.KullaniciId = aktifKullanici?.Id;
            ViewBag.TcKimlik = aktifKullanici?.TCKimlikNo;

            var borclar = _context.BorclarOdemeler
                .Where(b => b.HastaId == userId && !b.OdendiMi &&
                    (string.IsNullOrEmpty(protokol) || b.ProtakolNo == protokol))
                .ToList();

            return View(borclar);
        }

        // OdemeTamamla: Icerik.cshtml'deki JS fetch bu endpoint'i cagiriyor.
        // Secilen borclar odendiMi = true yapilip Makbuz'a yonlendirilir.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OdemeTamamla(int userId, string protokol, List<int> borcIds)
        {
            if (userId == 0 || borcIds == null || !borcIds.Any())
            {
                return Json(new { success = false, message = "Gecersiz odeme istegi." });
            }

            var kullanici = _context.Users.Find(userId);
            if (kullanici == null)
            {
                return Json(new { success = false, message = "Kullanici bulunamadi." });
            }

            var secilenBorclar = _context.BorclarOdemeler
                .Where(b => borcIds.Contains(b.Id) && b.HastaId == userId && !b.OdendiMi)
                .ToList();

            if (!secilenBorclar.Any())
            {
                return Json(new { success = false, message = "Odenecek borc kaydi bulunamadi." });
            }

            foreach (var borc in secilenBorclar)
            {
                borc.OdendiMi = true;
                borc.OdemeTarihi = DateTime.Now;
            }

            _context.SaveChanges();

            var borcIdsStr = string.Join(",", secilenBorclar.Select(b => b.Id));
            var makbuzUrl = Url.Action("Makbuz", "Odeme", new { userId = userId, borcIds = borcIdsStr });

            return Json(new { success = true, message = "Odeme basariyla tamamlandi.", redirectUrl = makbuzUrl });
        }

        [HttpGet]
        public IActionResult Makbuz(int? userId, string borcIds)
        {
            if (userId == null || string.IsNullOrEmpty(borcIds))
            {
                return RedirectToAction("Giris");
            }

            var selectedBorcIds = borcIds.Split(',').Select(int.Parse).ToList();

            var kullanici = _context.Users.Find(userId);
            if (kullanici == null)
            {
                return RedirectToAction("Giris");
            }

            var secilenBorclar = _context.BorclarOdemeler
                .Where(b => selectedBorcIds.Contains(b.Id) && b.HastaId == userId)
                .ToList();

            if (!secilenBorclar.Any())
            {
                TempData["Error"] = "Secilen borc kaydi bulunmadi.";
                return RedirectToAction("Icerik", new { userId });
            }

            var makbuzViewModel = new OdemeMakbuzViewModel
            {
                MakbuzId = userId.Value,
                HastaAdSoyad = $"{kullanici.Ad} {kullanici.Soyad}",
                TcKimlik = kullanici.TCKimlikNo,
                MakbuzNo = $"#GZ-{new Random().Next(100000, 999999)}",
                IslemTarihi = DateTime.Now,
                OdemeYontemi = "Kredi Karti (Tek Cekim)",
                IslemRef = $"TR-{new Random().Next(100000000, 999999999)}",
                KasiyerAdi = "Ayse Yilmaz",
                KasiyerBirim = "Gazi Hastanesi Tahsilat Birimi"
            };

            int siraNo = 1;
            foreach (var borc in secilenBorclar)
            {
                makbuzViewModel.Kalemler.Add(new OdemeIslemKalemi
                {
                    Id = siraNo++,
                    IslemAdi = borc.IslemTipi,
                    Adet = 1,
                    BirimFiyat = borc.Tutar
                });
            }

            ViewBag.KullaniciId = userId;

            return View(makbuzViewModel);
        }
    }
}
