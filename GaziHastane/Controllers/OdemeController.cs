using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GaziHastane.Data;
using System.Linq;
using System;
using System.Collections.Generic;

namespace GaziHastane.Controllers
{
    public class OdemeController : Controller
    {
        private readonly GaziHastaneContext _context;

        // Veritabaný baðlantýsýný alýyoruz
        public OdemeController(GaziHastaneContext context)
        {
            _context = context;
        }

        // Ödeme Giriþ Ekraný
        [HttpGet]
        public IActionResult Giris()
        {
            return View();
        }

        // Ödeme Login Ýþlemi (POST)
        [HttpPost]
        public IActionResult Login(string tcKimlik, string protokolNo)
        {
            if (string.IsNullOrEmpty(tcKimlik))
            {
                TempData["Error"] = "Lütfen T.C. Kimlik numaranýzý giriniz.";
                return RedirectToAction("Giris");
            }

            // Girilen TC Kimlik numarasýna göre kullanýcýyý bul
            var user = _context.Users.FirstOrDefault(u => u.TCKimlikNo == tcKimlik);

            if (user != null)
            {
                // Kullanýcý bulunduysa ödeme sayfasýna ID'si ve girilen protokol no ile yönlendir
                return RedirectToAction("Icerik", new { userId = user.Id, protokol = protokolNo });
            }

            TempData["Error"] = "Girdiðiniz T.C. Kimlik numarasýna ait hasta kaydý bulunamadý.";
            return RedirectToAction("Giris");
        }

        // Borçlarýn listelendiði ekran
        [HttpGet]
        public IActionResult Icerik(int? userId, string protokol)
        {
            // Giriþ yapýlmadan bu sayfaya gelinirse geri at
            if (userId == null)
            {
                return RedirectToAction("Giris");
            }

            // 1. Kullanýcýyý bul ve adýný arayüze (View) gönder
            var aktifKullanici = _context.Users.Find(userId);
            ViewBag.KullaniciAdSoyad = aktifKullanici?.Ad.ToUpper() + " " + aktifKullanici?.Soyad.ToUpper();
            ViewBag.Protokol = string.IsNullOrEmpty(protokol) ? "Bilinmiyor" : protokol;
            ViewBag.KullaniciId = aktifKullanici?.Id;

            // 2. Kullanýcýnýn ÖDENMEMÝÞ borçlarýný veritabanýndan çek
            var borclar = _context.BorclarOdemeler
                .Where(b => b.HastaId == userId && !b.OdendiMi)
                .ToList();

            // Borç listesini arayüze (View) gönderiyoruz
            return View(borclar);
        }
    }
}