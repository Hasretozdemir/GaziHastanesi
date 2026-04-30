using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

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
                TempData["Error"] = "Plan d�zenleme yetkinize ait doktor kayd� bulunamad�.";
                return RedirectToAction(nameof(Index));
            }

            if (DoktorRolundeMi())
            {
                model.DoktorId = doktorlar.First().Id;
            }

            if (!doktorlar.Any(x => x.Id == model.DoktorId))
            {
                TempData["Error"] = "Bu doktor plan�n� d�zenleme yetkiniz yok.";
                return RedirectToAction(nameof(Index));
            }

            var bolumler = _context.Bolumler.Where(x => x.IsActive).OrderBy(x => x.Ad).ToList();
            ViewBag.Doktorlar = doktorlar;
            ViewBag.Bolumler = bolumler;

            if (!TimeSpan.TryParse(model.BaslangicSaati, out var baslangic) || !TimeSpan.TryParse(model.BitisSaati, out var bitis))
            {
                TempData["Error"] = "Saat format� ge�ersiz.";
                return RedirectToAction(nameof(Index), new { doktorId = model.DoktorId, yil = model.Yil, ay = model.Ay });
            }

            if (!TimeSpan.TryParse(model.OgleMolaBaslangicSaati, out var ogleBaslangic) || !TimeSpan.TryParse(model.OgleMolaBitisSaati, out var ogleBitis))
            {
                TempData["Error"] = "��len mola saat format� ge�ersiz.";
                return RedirectToAction(nameof(Index), new { doktorId = model.DoktorId, yil = model.Yil, ay = model.Ay });
            }

            if (bitis <= baslangic)
            {
                TempData["Error"] = "Biti� saati ba�lang��tan b�y�k olmal�d�r.";
                return RedirectToAction(nameof(Index), new { doktorId = model.DoktorId, yil = model.Yil, ay = model.Ay });
            }

            if (ogleBitis <= ogleBaslangic)
            {
                TempData["Error"] = "��len mola biti� saati ba�lang��tan b�y�k olmal�d�r.";
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
            TempData["Success"] = "Doktor randevu plan� g�ncellendi.";
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

        [HttpGet]
        public async Task<IActionResult> ExportPDF(int doktorId, DateTime? tarih)
        {
            var doktorlar = _context.Doktorlar.Where(x => x.IsActive).OrderBy(x => x.Ad).ThenBy(x => x.Soyad).ToList();
            doktorlar = FiltrelenmisDoktorlar(doktorlar);

            if (!doktorlar.Any(x => x.Id == doktorId))
            {
                return Unauthorized();
            }

            var doktor = await _context.Doktorlar
                .Include(x => x.Bolum)
                .FirstOrDefaultAsync(x => x.Id == doktorId);

            if (doktor == null)
                return NotFound();

            var hedefTarih = tarih ?? DateTime.Today;

            var randevular = await _context.Randevular
                .Include(x => x.Hasta)
                .Include(x => x.Doktor)
                .Include(x => x.Bolum)
                .Where(x => x.DoktorId == doktorId 
                    && x.RandevuTarihi.Date == hedefTarih.Date
                    && x.Durum != 2) // 2 = İptal
                .OrderBy(x => x.RandevuTarihi)
                .ToListAsync();

            using (var stream = new MemoryStream())
            {
                var document = new Document(PageSize.A4, 20, 20, 40, 20);
                var writer = PdfWriter.GetInstance(document, stream);
                document.Open();

                // Başlık - Türkçe karakterler için sistem fontunu kullan ve Unicode (Identity-H) ile embed et
                Font titleFont, headerFont, normalFont, smallFont;
                try
                {
                    var fontsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
                    var arial = Path.Combine(fontsFolder, "arial.ttf");
                    BaseFont bf;
                    if (System.IO.File.Exists(arial))
                    {
                        bf = BaseFont.CreateFont(arial, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    }
                    else
                    {
                        // Fallback to Helvetica (may have limited Turkish support)
                        bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.IDENTITY_H, false);
                    }

                    titleFont = new Font(bf, 16, Font.BOLD);
                    headerFont = new Font(bf, 12, Font.BOLD);
                    normalFont = new Font(bf, 10, Font.NORMAL);
                    smallFont = new Font(bf, 9, Font.NORMAL);
                }
                catch
                {
                    // Eğer özel font yüklenemezse, eski FontFactory kullanımına dön
                    titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                    headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                    normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                    smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                }

                var title = new Paragraph("POLİKLİNİK LİSTESİ", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                document.Add(new Paragraph(" "));

                // Doktor ve Tarih Bilgileri
                var infoTable = new PdfPTable(2);
                infoTable.WidthPercentage = 100;
                infoTable.SetWidths(new float[] { 50, 50 });

                var cell1 = new PdfPCell(new Phrase($"Doktor: {doktor.Ad} {doktor.Soyad}", normalFont));
                cell1.Border = Rectangle.NO_BORDER;
                infoTable.AddCell(cell1);

                var cell2 = new PdfPCell(new Phrase($"Tarih: {hedefTarih:dd.MM.yyyy}", normalFont));
                cell2.Border = Rectangle.NO_BORDER;
                infoTable.AddCell(cell2);

                var cell3 = new PdfPCell(new Phrase($"Bölüm: {doktor.Bolum?.Ad ?? "-"}", normalFont));
                cell3.Border = Rectangle.NO_BORDER;
                infoTable.AddCell(cell3);

                var cell4 = new PdfPCell(new Phrase($"Toplam Randevu: {randevular.Count}", normalFont));
                cell4.Border = Rectangle.NO_BORDER;
                infoTable.AddCell(cell4);

                document.Add(infoTable);
                document.Add(new Paragraph(" "));

                // Randevu Tablosu
                if (randevular.Any())
                {
                    var table = new PdfPTable(4);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 15, 30, 30, 25 });

                    // Header
                    var headers = new[] { "Saat", "Hasta Adı", "Şikayet", "Durum" };
                    foreach (var header in headers)
                    {
                        var headerCell = new PdfPCell(new Phrase(header, headerFont));
                        headerCell.BackgroundColor = new BaseColor(200, 200, 200);
                        headerCell.Padding = 5;
                        table.AddCell(headerCell);
                    }

                    // Veriler
                    foreach (var randevu in randevular)
                    {
                        var saat = randevu.RandevuTarihi.ToString("HH:mm");
                        var hastaAdi = randevu.Hasta != null ? $"{randevu.Hasta.Ad} {randevu.Hasta.Soyad}" : "-";
                        var sikayet = randevu.Sikayet ?? "-";
                        var durum = randevu.Durum == 0 ? "Bekleniyor" : randevu.Durum == 1 ? "Tamamlandı" : "İptal";

                        table.AddCell(new PdfPCell(new Phrase(saat, normalFont)) { Padding = 5 });
                        table.AddCell(new PdfPCell(new Phrase(hastaAdi, normalFont)) { Padding = 5 });
                        table.AddCell(new PdfPCell(new Phrase(sikayet, smallFont)) { Padding = 5 });
                        table.AddCell(new PdfPCell(new Phrase(durum, normalFont)) { Padding = 5 });
                    }

                    document.Add(table);
                }
                else
                {
                    var noData = new Paragraph("Bu tarihte randevu bulunmamaktadır.", normalFont);
                    noData.Alignment = Element.ALIGN_CENTER;
                    document.Add(noData);
                }

                document.Add(new Paragraph(" "));

                // Alt Bilgi
                var footer = new Paragraph($"Oluşturulma Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm:ss}", smallFont);
                footer.Alignment = Element.ALIGN_RIGHT;
                document.Add(footer);

                document.Close();

                var content = stream.ToArray();
                string pdfName = $"Poliklinik_Listesi_{doktor.Ad}_{doktor.Soyad}_{hedefTarih:yyyyMMdd}.pdf";
                return File(content, "application/pdf", pdfName);
            }
        }
    }
}
