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
                "SIL",
                "GUNCELLE",
                "GORÜS"
            };

            ViewBag.Moduller = new List<string>
            {
                "Gösterge Paneli",
                "Ana Sayfa Slider",
                "Belgeler",
                "Doktor Planlama",
                "Tahlil Sonucu Girisi",
                "Tahlil Sonucu Sorgu",
                "Dijital Islemler",
                "Haberler",
                "Etkinlikler",
                "Duyurular",
                "Kurumsal",
                "Yetkili Listesi",
                "Doktorlar",
                "Poliklinikler",
                "Hasta Rehberi",
                "Iletisim",
                "Borc Yonetimi",
                "Kalite Yonetimi",
                "Egitim Komitesi",
                "Yemek Listesi",
                "Kroki Yonetimi",
                "Gorseller",
                "Giris Paneli"
            };

            return View(loglar);
        }

        public async Task<IActionResult> ExportExcel(string? kullanici, string? islemTipi, string? modul, DateTime? baslangicTarih, DateTime? bitisTarih)
        {
            try
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

                    // Workbook metadata
                    workbook.Properties.Title = "Sistem Hareketleri Raporu";
                    workbook.Properties.Author = "Gazi Hastane";

                    // Rapor başlığı
                    worksheet.Cell(1, 1).Value = "GAZİ HASTANESİ - SİSTEM HAREKETLERİ RAPORU";
                    worksheet.Range(1, 1, 1, 6).Merge();
                    var reportTitleRange = worksheet.Range(1, 1, 1, 6);
                    reportTitleRange.Style.Font.Bold = true;
                    reportTitleRange.Style.Font.FontSize = 14;
                    reportTitleRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#2F5496");
                    reportTitleRange.Style.Font.FontColor = XLColor.White;
                    reportTitleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    reportTitleRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Row(1).Height = 22;

                    // Filtre Bilgileri
                    var filters = new List<string>();
                    filters.Add("FİLTRE BİLGİLERİ");
                    filters.Add($"Kullanıcı: {(string.IsNullOrWhiteSpace(kullanici) ? "Tümü" : kullanici)}");
                    filters.Add($"İşlem Tipi: {(string.IsNullOrWhiteSpace(islemTipi) ? "Tümü" : islemTipi)}");
                    filters.Add($"Modül: {(string.IsNullOrWhiteSpace(modul) ? "Tümü" : modul)}");
                    filters.Add($"Tarih Aralığı: {(baslangicTarih.HasValue ? baslangicTarih.Value.ToString("dd.MM.yyyy") : "Başlangıç yok")} - {(bitisTarih.HasValue ? bitisTarih.Value.ToString("dd.MM.yyyy") : "Bitiş yok")}");
                    filters.Add($"Rapor Oluşturulma: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                    filters.Add($"Toplam Kayıt: {logs.Count}");

                    var row = 3; // leave one blank row after title
                    foreach (var f in filters)
                    {
                        worksheet.Cell(row, 1).Value = f;
                        worksheet.Range(row, 1, row, 6).Merge();

                        if (row == 3)
                        {
                            var t = worksheet.Range(row, 1, row, 6);
                            t.Style.Font.Bold = true;
                            t.Style.Font.FontSize = 11;
                            t.Style.Fill.BackgroundColor = XLColor.DarkBlue;
                            t.Style.Font.FontColor = XLColor.White;
                            t.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            t.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        }
                        else
                        {
                            var fRange = worksheet.Range(row, 1, row, 6);
                            fRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                            fRange.Style.Font.FontSize = 10;
                            fRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            fRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        }

                        row++;
                    }

                    var lastFilterRow = row - 1;
                    worksheet.Range(3, 1, lastFilterRow, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    // Boş satır
                    row++;

                    // Header row
                    var headerRow = row;
                    worksheet.Cell(headerRow, 1).Value = "Tarih";
                    worksheet.Cell(headerRow, 2).Value = "Kullanıcı Adı";
                    worksheet.Cell(headerRow, 3).Value = "Modül";
                    worksheet.Cell(headerRow, 4).Value = "İşlem Tipi";
                    worksheet.Cell(headerRow, 5).Value = "Açıklama";
                    worksheet.Cell(headerRow, 6).Value = "IP Adresi";

                    var headerRange = worksheet.Range(headerRow, 1, headerRow, 6);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Font.FontColor = XLColor.White;
                    headerRange.Style.Fill.BackgroundColor = XLColor.Navy;
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    headerRange.Style.Font.FontSize = 11;

                    // Freeze top rows (filters + header)
                    worksheet.SheetView.FreezeRows(headerRow);

                    // Veriler
                    foreach (var log in logs)
                    {
                        row++;
                        worksheet.Cell(row, 1).Value = log.Tarih.ToString("dd.MM.yyyy HH:mm:ss");
                        worksheet.Cell(row, 2).Value = log.KullaniciAdi ?? "-";
                        worksheet.Cell(row, 3).Value = log.Modul ?? "-";
                        worksheet.Cell(row, 4).Value = log.IslemTipi ?? "-";
                        worksheet.Cell(row, 5).Value = log.Aciklama ?? "-";
                        worksheet.Cell(row, 6).Value = log.IpAdresi ?? "-";

                        if ((row - headerRow) % 2 == 0)
                            worksheet.Range(row, 1, row, 6).Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    }

                    // Sütun genişlikleri ve hücre ayarları
                    worksheet.Column(1).Width = 20; // Tarih
                    worksheet.Column(2).Width = 28; // Kullanıcı Adı
                    worksheet.Column(3).Width = 22; // Modül
                    worksheet.Column(4).Width = 15; // İşlem Tipi
                    worksheet.Column(5).Width = 60; // Açıklama
                    worksheet.Column(6).Width = 18; // IP
                    worksheet.Column(5).Style.Alignment.WrapText = true;
                    worksheet.Range(1, 1, row, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

                    worksheet.Range(1, 1, row, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Range(1, 1, row, 6).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        string excelName = $"Sistem_Hareketleri_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                    }
                }
            }
            catch (Exception ex)
            {
                // Basit hata geri bildirimi; istenirse burada veritabanına log kaydı eklenebilir.
                return StatusCode(500, "Excel raporu oluşturulurken bir hata oluştu: " + ex.Message);
            }
        }
    }
}
