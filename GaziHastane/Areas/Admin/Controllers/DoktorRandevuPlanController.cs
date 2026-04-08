using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DoktorRandevuPlanController : Controller
    {
        private readonly GaziHastaneContext _context;

        public DoktorRandevuPlanController(GaziHastaneContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int? doktorId, int? yil, int? ay)
        {
            var doktorlar = _context.Doktorlar
                .Where(x => x.IsActive)
                .OrderBy(x => x.Ad)
                .ThenBy(x => x.Soyad)
                .ToList();

            doktorlar = FiltrelenmisDoktorlar(doktorlar);

            var bolumler = _context.Bolumler.Where(x => x.IsActive).OrderBy(x => x.Ad).ToList();

            if (!doktorlar.Any())
            {
                ViewBag.Doktorlar = doktorlar;
                ViewBag.Bolumler = bolumler;
                return View(new DoktorRandevuPlanViewModel { Yil = DateTime.Today.Year, Ay = DateTime.Today.Month });
            }

            var seciliDoktorId = doktorId ?? doktorlar.First().Id;

            if (!doktorlar.Any(x => x.Id == seciliDoktorId))
            {
                seciliDoktorId = doktorlar.First().Id;
            }

            var seciliYil = yil ?? DateTime.Today.Year;
            var seciliAy = ay ?? DateTime.Today.Month;
            var seciliDoktorBolumId = doktorlar.FirstOrDefault(x => x.Id == seciliDoktorId)?.BolumId;

            var plan = _context.DoktorRandevuPlanlari
                .Include(x => x.Gunler)
                .FirstOrDefault(x => x.DoktorId == seciliDoktorId && x.Yil == seciliYil && x.Ay == seciliAy);

            var vm = new DoktorRandevuPlanViewModel
            {
                DoktorId = seciliDoktorId,
                BolumId = seciliDoktorBolumId,
                Yil = seciliYil,
                Ay = seciliAy,
                SlotSureDakika = plan?.SlotSureDakika ?? 30,
                BaslangicSaati = (plan?.BaslangicSaati ?? new TimeSpan(9, 0, 0)).ToString("hh\\:mm"),
                BitisSaati = (plan?.BitisSaati ?? new TimeSpan(17, 0, 0)).ToString("hh\\:mm"),
                OgleMolaBaslangicSaati = (plan?.OgleMolaBaslangicSaati ?? new TimeSpan(12, 0, 0)).ToString("hh\\:mm"),
                OgleMolaBitisSaati = (plan?.OgleMolaBitisSaati ?? new TimeSpan(13, 0, 0)).ToString("hh\\:mm"),
                VarsayilanGunlukMaxRandevu = plan?.Gunler.FirstOrDefault()?.GunlukMaxRandevu ?? 20
            };

            var ayinIlkGunu = new DateTime(seciliYil, seciliAy, 1);
            var ayinSonGunu = ayinIlkGunu.AddMonths(1).AddDays(-1);

            for (var d = ayinIlkGunu; d <= ayinSonGunu; d = d.AddDays(1))
            {
                var planGun = plan?.Gunler.FirstOrDefault(x => x.Tarih.Date == d.Date);
                vm.Gunler.Add(new DoktorRandevuGunSatirViewModel
                {
                    PlanGunId = planGun?.Id,
                    Tarih = d,
                    IsRandevuAcik = planGun?.IsRandevuAcik ?? false,
                    GunlukMaxRandevu = planGun?.GunlukMaxRandevu ?? vm.VarsayilanGunlukMaxRandevu,
                    BaslangicSaati = planGun?.BaslangicSaati?.ToString("hh\\:mm"),
                    BitisSaati = planGun?.BitisSaati?.ToString("hh\\:mm")
                });
            }

            ViewBag.Doktorlar = doktorlar;
            ViewBag.Bolumler = bolumler;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(DoktorRandevuPlanViewModel model)
        {
            var doktorlar = _context.Doktorlar.Where(x => x.IsActive).OrderBy(x => x.Ad).ThenBy(x => x.Soyad).ToList();
            doktorlar = FiltrelenmisDoktorlar(doktorlar);

            if (!doktorlar.Any())
            {
                TempData["Error"] = "Plan düzenleme yetkinize ait doktor kaydý bulunamadý.";
                return RedirectToAction(nameof(Index));
            }

            if (DoktorRolundeMi())
            {
                model.DoktorId = doktorlar.First().Id;
            }

            if (!doktorlar.Any(x => x.Id == model.DoktorId))
            {
                TempData["Error"] = "Bu doktor planýný düzenleme yetkiniz yok.";
                return RedirectToAction(nameof(Index));
            }

            var bolumler = _context.Bolumler.Where(x => x.IsActive).OrderBy(x => x.Ad).ToList();
            ViewBag.Doktorlar = doktorlar;
            ViewBag.Bolumler = bolumler;

            if (!TimeSpan.TryParse(model.BaslangicSaati, out var baslangic) || !TimeSpan.TryParse(model.BitisSaati, out var bitis))
            {
                TempData["Error"] = "Saat formatý geçersiz.";
                return RedirectToAction(nameof(Index), new { doktorId = model.DoktorId, yil = model.Yil, ay = model.Ay });
            }

            if (!TimeSpan.TryParse(model.OgleMolaBaslangicSaati, out var ogleBaslangic) || !TimeSpan.TryParse(model.OgleMolaBitisSaati, out var ogleBitis))
            {
                TempData["Error"] = "Öðlen mola saat formatý geçersiz.";
                return RedirectToAction(nameof(Index), new { doktorId = model.DoktorId, yil = model.Yil, ay = model.Ay });
            }

            if (bitis <= baslangic)
            {
                TempData["Error"] = "Bitiþ saati baþlangýçtan büyük olmalýdýr.";
                return RedirectToAction(nameof(Index), new { doktorId = model.DoktorId, yil = model.Yil, ay = model.Ay });
            }

            if (ogleBitis <= ogleBaslangic)
            {
                TempData["Error"] = "Öðlen mola bitiþ saati baþlangýçtan büyük olmalýdýr.";
                return RedirectToAction(nameof(Index), new { doktorId = model.DoktorId, yil = model.Yil, ay = model.Ay });
            }

            var plan = _context.DoktorRandevuPlanlari
                .Include(x => x.Gunler)
                .FirstOrDefault(x => x.DoktorId == model.DoktorId && x.Yil == model.Yil && x.Ay == model.Ay);

            if (plan == null)
            {
                var doktorBolumId = _context.Doktorlar.Where(x => x.Id == model.DoktorId).Select(x => x.BolumId).FirstOrDefault();
                plan = new DoktorRandevuPlani
                {
                    DoktorId = model.DoktorId,
                    BolumId = doktorBolumId,
                    Yil = model.Yil,
                    Ay = model.Ay
                };
                _context.DoktorRandevuPlanlari.Add(plan);
            }

            plan.BolumId = _context.Doktorlar.Where(x => x.Id == model.DoktorId).Select(x => x.BolumId).FirstOrDefault();
            plan.SlotSureDakika = model.SlotSureDakika;
            plan.BaslangicSaati = baslangic;
            plan.BitisSaati = bitis;
            plan.OgleMolaBaslangicSaati = ogleBaslangic;
            plan.OgleMolaBitisSaati = ogleBitis;

            var seciliGunler = model.Gunler.Where(x => x.IsRandevuAcik).ToList();
            var seciliTarihler = seciliGunler.Select(x => x.Tarih.Date).ToHashSet();

            var silinecekler = plan.Gunler.Where(x => !seciliTarihler.Contains(x.Tarih.Date)).ToList();
            if (silinecekler.Any())
            {
                _context.DoktorRandevuPlanGunleri.RemoveRange(silinecekler);
            }

            foreach (var gun in seciliGunler)
            {
                var mevcut = plan.Gunler.FirstOrDefault(x => x.Tarih.Date == gun.Tarih.Date);
                var gunBaslangicVar = TimeSpan.TryParse(gun.BaslangicSaati, out var gunBaslangic);
                var gunBitisVar = TimeSpan.TryParse(gun.BitisSaati, out var gunBitis);

                if (mevcut == null)
                {
                    mevcut = new DoktorRandevuPlanGunu
                    {
                        Tarih = gun.Tarih.Date,
                        IsRandevuAcik = true,
                        GunlukMaxRandevu = gun.GunlukMaxRandevu <= 0 ? model.VarsayilanGunlukMaxRandevu : gun.GunlukMaxRandevu,
                        BaslangicSaati = gunBaslangicVar ? gunBaslangic : null,
                        BitisSaati = gunBitisVar ? gunBitis : null
                    };
                    plan.Gunler.Add(mevcut);
                }
                else
                {
                    mevcut.IsRandevuAcik = true;
                    mevcut.GunlukMaxRandevu = gun.GunlukMaxRandevu <= 0 ? model.VarsayilanGunlukMaxRandevu : gun.GunlukMaxRandevu;
                    mevcut.BaslangicSaati = gunBaslangicVar ? gunBaslangic : null;
                    mevcut.BitisSaati = gunBitisVar ? gunBitis : null;
                }
            }

            _context.SaveChanges();
            TempData["Success"] = "Doktor randevu planý güncellendi.";
            return RedirectToAction(nameof(Index), new { doktorId = model.DoktorId, yil = model.Yil, ay = model.Ay });
        }

        private List<Doktor> FiltrelenmisDoktorlar(List<Doktor> doktorlar)
        {
            if (!DoktorRolundeMi())
            {
                return doktorlar;
            }

            var adSoyad = User.FindFirstValue(ClaimTypes.Name)?.Trim();
            if (string.IsNullOrWhiteSpace(adSoyad))
            {
                return new List<Doktor>();
            }

            return doktorlar
                .Where(x => string.Equals($"{x.Ad} {x.Soyad}".Trim(), adSoyad, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private bool DoktorRolundeMi()
        {
            var rol = User.FindFirstValue(ClaimTypes.Role);
            return string.Equals(rol, "Doktor", StringComparison.OrdinalIgnoreCase);
        }
    }
}
