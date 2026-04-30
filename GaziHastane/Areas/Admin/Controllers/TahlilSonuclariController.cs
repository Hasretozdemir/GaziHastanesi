using System.Linq;
using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TahlilSonuclariController : Controller
    {
        private readonly GaziHastaneContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TahlilSonuclariController(GaziHastaneContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        private object DoktorListesi()
        {
            return _context.Doktorlar
                .Where(x => x.IsActive)
                .OrderBy(x => x.Ad)
                .ThenBy(x => x.Soyad)
                .Select(x => new { x.Id, AdSoyad = (x.Unvan ?? "Dr.") + " " + x.Ad + " " + x.Soyad })
                .ToList();
        }

        [HttpGet]
        public IActionResult Giris()
        {
            ViewBag.Doktorlar = DoktorListesi();

            return View(new TahlilSonucGirisViewModel { TestKategorisi = "Laboratuvar" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Giris(TahlilSonucGirisViewModel model)
        {
            ViewBag.Doktorlar = DoktorListesi();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var temizTc = (model.TCKimlikNo ?? string.Empty).Trim();
            var hasta = _context.Users.FirstOrDefault(x => x.TCKimlikNo == temizTc);
            if (hasta == null)
            {
                ModelState.AddModelError(nameof(model.TCKimlikNo), "Bu T.C. Kimlik No ile kayýtlý hasta bulunamadý.");
                return View(model);
            }

            // Dosya yükleme ve PDF alanlarý gereksiz olduðundan kaldýrýldý
            string? raporYolu = null;

            var yeniKayit = new TahlilSonuc
            {
                HastaId = hasta.Id,
                DoktorId = model.DoktorId,
                Tarih = model.Tarih,
                TestKategorisi = string.IsNullOrWhiteSpace(model.TestKategorisi) ? "Laboratuvar" : model.TestKategorisi.Trim(),
                TestAdi = model.TestAdi.Trim(),
                SonucDegeri = string.IsNullOrWhiteSpace(model.SonucDegeri) ? null : model.SonucDegeri.Trim(),
                ReferansAraligi = string.IsNullOrWhiteSpace(model.ReferansAraligi) ? null : model.ReferansAraligi.Trim(),
                RaporDosyaUrl = raporYolu
            };

            _context.TahlilSonuclari.Add(yeniKayit);
            _context.SaveChanges();

            TempData["Success"] = $"{hasta.Ad} {hasta.Soyad} için tahlil sonucu kaydedildi.";
            return RedirectToAction(nameof(Giris));
        }

        [HttpGet]
        public IActionResult HastaAra(string tcKimlikNo)
        {
            var temizTc = (tcKimlikNo ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(temizTc) || temizTc.Length != 11)
            {
                return Json(new { success = false, message = "T.C. Kimlik No 11 haneli olmalýdýr." });
            }

            var hasta = _context.Users
                .Where(x => x.TCKimlikNo == temizTc)
                .Select(x => new { x.Id, x.Ad, x.Soyad, x.TCKimlikNo })
                .FirstOrDefault();

            if (hasta == null)
            {
                return Json(new { success = false, message = "Hasta bulunamadý." });
            }

            return Json(new
            {
                success = true,
                hastaId = hasta.Id,
                adSoyad = hasta.Ad + " " + hasta.Soyad,
                tcKimlikNo = hasta.TCKimlikNo
            });
        }

        [HttpGet]
        public IActionResult Sorgula(string? tcKimlikNo)
        {
            var model = new TahlilSonucSorguViewModel
            {
                TCKimlikNo = tcKimlikNo
            };

            if (!string.IsNullOrWhiteSpace(tcKimlikNo))
            {
                var temizTc = tcKimlikNo.Trim();
                var hasta = _context.Users.FirstOrDefault(x => x.TCKimlikNo == temizTc);

                if (hasta != null)
                {
                    model.Hasta = hasta;
                    model.Sonuclar = _context.TahlilSonuclari
                        .Include(x => x.Doktor)
                        .Where(x => x.HastaId == hasta.Id)
                        .OrderByDescending(x => x.Tarih)
                        .ToList();
                }
                else
                {
                    TempData["Error"] = "Bu T.C. Kimlik No ile hasta bulunamadý.";
                }
            }

            return View(model);
        }
    }
}
