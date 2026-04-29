using ClosedXML.Excel;
using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SistemLogController : Controller
    {
        private readonly GaziHastaneContext _context;

        public SistemLogController(GaziHastaneContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? kullanici, string? islemTipi, string? modul, DateTime? baslangicTarih, DateTime? bitisTarih)
        {
            var query = _context.AdminLoglari.AsQueryable();

            if (!string.IsNullOrWhiteSpace(kullanici))
            {
                query = query.Where(x => x.KullaniciAdi != null && x.KullaniciAdi.Contains(kullanici));
            }

            if (!string.IsNullOrWhiteSpace(islemTipi))
            {
                query = query.Where(x => x.IslemTipi == islemTipi);
            }

            if (!string.IsNullOrWhiteSpace(modul))
            {
                query = query.Where(x => x.Modul == modul);
            }

            if (baslangicTarih.HasValue)
            {
                var start = baslangicTarih.Value.Date;
                query = query.Where(x => x.Tarih >= start);
            }

            if (bitisTarih.HasValue)
            {
                var end = bitisTarih.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.Tarih <= end);
            }

            var loglar = await query
                .OrderByDescending(x => x.Tarih)
                .Take(500)
                .ToListAsync();

            ViewBag.Kullanici = kullanici;
            ViewBag.IslemTipi = islemTipi;
            ViewBag.Modul = modul;
            ViewBag.BaslangicTarih = baslangicTarih?.ToString("yyyy-MM-dd");
            ViewBag.BitisTarih = bitisTarih?.ToString("yyyy-MM-dd");

            ViewBag.IslemTipleri = new List<string>
            {
                "EKLE",
                "SÝL",
                "GÜNCELLE",
                "GÝRÝÞ"
            };

            ViewBag.Moduller = new List<string>
            {
                "Gösterge Paneli",
                "Ana Sayfa Slider",
                "Belgeler",
                "Doktor Planlama",
                "Tahlil Sonuç Giriþi",
                "Tahlil Sonuç Sorgu",
                "Dijital Ýþlemler",
                "Haberler",
                "Etkinlikler",
                "Duyurular",
                "Kurumsal",
                "Yetkili Listesi",
                "Doktorlar",
                "Poliklinikler",
                "Hasta Rehberi",
                "Ýletiþim",
                "Borç Yönetimi",
                "Kalite Yönetimi",
                "Eðitim Komitesi",
                "Yemek Listesi",
                "Kroki Yönetimi",
                "Görseller",
                "Giriþ Paneli"
            };

            return View(loglar);
        }
        public async Task<IActionResult> ExportExcel(string? kullanici, string? islemTipi, string? modul, DateTime? baslangicTarih, DateTime? bitisTarih)
        {
            var query = _context.AdminLoglari.AsQueryable();

            if (!string.IsNullOrWhiteSpace(kullanici))
                query = query.Where(x => x.KullaniciAdi != null && x.KullaniciAdi.Contains(kullanici));

            if (!string.IsNullOrWhiteSpace(islemTipi))
                query = query.Where(x => x.IslemTipi == islemTipi);

            if (!string.IsNullOrWhiteSpace(modul))
                query = query.Where(x => x.Modul == modul);

            if (baslangicTarih.HasValue)
            {
                var start = baslangicTarih.Value.Date;
                query = query.Where(x => x.Tarih >= start);
            }

            if (bitisTarih.HasValue)
            {
                var end = bitisTarih.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.Tarih <= end);
            }

            var logs = await query.OrderByDescending(x => x.Tarih).ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sistem Hareketleri");
                var currentRow = 1;

                // Basliklar ve Stil
                worksheet.Cell(currentRow, 1).Value = "Tarih";
                worksheet.Cell(currentRow, 2).Value = "Kullanici Adi";
                worksheet.Cell(currentRow, 3).Value = "Modul";
                worksheet.Cell(currentRow, 4).Value = "Islem Tipi";
                worksheet.Cell(currentRow, 5).Value = "Aciklama";
                worksheet.Cell(currentRow, 6).Value = "IP Adresi";

                var headerRange = worksheet.Range("A1:F1");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Font.FontColor = XLColor.White;
                headerRange.Style.Fill.BackgroundColor = XLColor.Navy;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Veriler
                foreach (var log in logs)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = log.Tarih.ToString("dd.MM.yyyy HH:mm:ss");
                    worksheet.Cell(currentRow, 2).Value = log.KullaniciAdi ?? "-";
                    worksheet.Cell(currentRow, 3).Value = log.Modul ?? "-";
                    worksheet.Cell(currentRow, 4).Value = log.IslemTipi ?? "-";
                    worksheet.Cell(currentRow, 5).Value = log.Aciklama ?? "-";
                    worksheet.Cell(currentRow, 6).Value = log.IpAdresi ?? "-";
                    
                    if(currentRow % 2 == 0)
                        worksheet.Range($"A{currentRow}:F{currentRow}").Style.Fill.BackgroundColor = XLColor.AliceBlue;
                }

                // Hucre stil ayarlamalari
                worksheet.Columns().AdjustToContents();
                worksheet.Range($"A1:F{currentRow}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A1:F{currentRow}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    string excelName = $"Sistem_Hareketleri_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                }
            }
        }
    }
}


