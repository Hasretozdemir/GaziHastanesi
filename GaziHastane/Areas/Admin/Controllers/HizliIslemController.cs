using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HizliIslemController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly GaziHastaneContext _context;

        public HizliIslemController(IWebHostEnvironment env, GaziHastaneContext context)
        {
            _env = env;
            _context = context;
        }

        // ==========================================
        // DİJİTAL İŞLEMLER (ANA SAYFA HIZLI LİNKLER)
        // ==========================================

        public async Task<IActionResult> Index()
        {
            // Veritabanındaki hızlı işlemleri sıra numarasına göre getir
            var liste = await _context.HizliIslemler.OrderBy(x => x.SiraNo).ToListAsync();
            return View(liste);
        }

        [HttpGet]
        public async Task<IActionResult> GetHizliIslem(int id)
        {
            var veri = await _context.HizliIslemler.FindAsync(id);
            if (veri == null) return NotFound();
            return Json(veri);
        }

        [HttpPost]
        public async Task<IActionResult> HizliIslemKaydet(HizliIslem model)
        {
            if (model.Id == 0) // Yeni Ekleme
            {
                _context.HizliIslemler.Add(model);
                TempData["Success"] = "Yeni dijital işlem başarıyla eklendi.";
            }
            else // Güncelleme
            {
                _context.HizliIslemler.Update(model);
                TempData["Success"] = "Dijital işlem başarıyla güncellendi.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> HizliIslemSil(int id)
        {
            var veri = await _context.HizliIslemler.FindAsync(id);
            if (veri != null)
            {
                _context.HizliIslemler.Remove(veri);
                await _context.SaveChangesAsync();
                TempData["Success"] = "İşlem başarıyla silindi.";
            }
            return RedirectToAction("Index");
        }

        // ==========================================
        // --- KALİTE YÖNETİMİ PANELİ ---
        // ==========================================
        public async Task<IActionResult> Kalite()
        {
            var liste = await _context.KaliteBelgeleri.OrderByDescending(x => x.YayinTarihi).ToListAsync();
            return View(liste);
        }

        [HttpGet]
        public async Task<IActionResult> GetKaliteDetay(int id)
        {
            var veri = await _context.KaliteBelgeleri.FindAsync(id);
            if (veri == null) return NotFound();
            return Json(veri);
        }

        [HttpPost]
        public async Task<IActionResult> KaliteKaydet(KaliteBelgesi model, IFormFile? fotoDosya, IFormFile? belgeDosya)
        {
            string uploadPath = Path.Combine(_env.WebRootPath, "uploads", "kalite");
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

            if (fotoDosya != null)
            {
                string fotoName = Guid.NewGuid() + Path.GetExtension(fotoDosya.FileName);
                using (var stream = new FileStream(Path.Combine(uploadPath, fotoName), FileMode.Create))
                {
                    await fotoDosya.CopyToAsync(stream);
                }
                model.FotoUrl = "/uploads/kalite/" + fotoName;
            }

            if (belgeDosya != null)
            {
                string belgeName = Guid.NewGuid() + Path.GetExtension(belgeDosya.FileName);
                using (var stream = new FileStream(Path.Combine(uploadPath, belgeName), FileMode.Create))
                {
                    await belgeDosya.CopyToAsync(stream);
                }
                model.DosyaUrl = "/uploads/kalite/" + belgeName;
            }

            if (string.IsNullOrEmpty(model.DosyaUrl))
            {
                model.DosyaUrl = "#";
            }

            if (model.Id == 0)
            {
                model.YayinTarihi = DateTime.UtcNow;
                _context.KaliteBelgeleri.Add(model);
                TempData["Success"] = "Yeni kalite kartı oluşturuldu.";
            }
            else
            {
                var eskiVeri = await _context.KaliteBelgeleri.AsNoTracking().FirstOrDefaultAsync(x => x.Id == model.Id);

                model.FotoUrl ??= eskiVeri?.FotoUrl;
                if (model.DosyaUrl == "#") model.DosyaUrl = eskiVeri?.DosyaUrl ?? "#";

                model.YayinTarihi = DateTime.UtcNow;
                _context.KaliteBelgeleri.Update(model);
                TempData["Success"] = "İçerik başarıyla güncellendi.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Kalite");
        }

        [HttpPost]
        public async Task<IActionResult> KaliteSil(int id)
        {
            var veri = await _context.KaliteBelgeleri.FindAsync(id);
            if (veri != null)
            {
                _context.KaliteBelgeleri.Remove(veri);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Kalite");
        }

        // ==========================================
        // --- YEMEK LİSTESİ PANELİ ---
        // ==========================================
        public async Task<IActionResult> YemekListesi()
        {
            var liste = await _context.YemekListesi.OrderByDescending(x => x.Tarih).ThenBy(x => x.Ogun).ToListAsync();
            return View(liste);
        }

        [HttpPost]
        public async Task<IActionResult> YemekEkle(YemekListesi model)
        {
            _context.YemekListesi.Add(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Yemek listesi güncellendi.";
            return RedirectToAction("YemekListesi");
        }

        public async Task<IActionResult> YemekSil(int id)
        {
            var veri = await _context.YemekListesi.FindAsync(id);
            if (veri != null)
            {
                _context.YemekListesi.Remove(veri);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Öğün kaydı silindi.";
            }
            return RedirectToAction("YemekListesi");
        }


        public async Task<IActionResult> YemekListesiExcel()
        {
            var liste = await _context.YemekListesi.OrderByDescending(x => x.Tarih).ThenBy(x => x.Ogun).ToListAsync();

            using var workbook = new ClosedXML.Excel.XLWorkbook();
            {
                var worksheet = workbook.Worksheets.Add("Yemek Listesi");
                
                // Başlıklar
                worksheet.Cell(1, 1).Value = "Tarih";
                worksheet.Cell(1, 2).Value = "Gün";
                worksheet.Cell(1, 3).Value = "Öğün";
                worksheet.Cell(1, 4).Value = "Çorba";
                worksheet.Cell(1, 5).Value = "Ana Yemek";
                worksheet.Cell(1, 6).Value = "Yardımcı Yemek";
                worksheet.Cell(1, 7).Value = "Tatlı / Meyve";
                worksheet.Cell(1, 8).Value = "Kalori (Kcal)";

                // Başlık Stili
                var headerRange = worksheet.Range(1, 1, 1, 8);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.AliceBlue;
                headerRange.Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;

                // Veriler
                for (int i = 0; i < liste.Count; i++)
                {
                    var item = liste[i];
                    int row = i + 2;

                    worksheet.Cell(row, 1).Value = item.Tarih.ToString("dd.MM.yyyy");
                    worksheet.Cell(row, 2).Value = item.Tarih.ToString("dddd");
                    worksheet.Cell(row, 3).Value = item.Ogun switch { 1 => "Kahvaltı", 2 => "Öğle", 3 => "Akşam", _ => "-" };
                    worksheet.Cell(row, 4).Value = item.Corba;
                    worksheet.Cell(row, 5).Value = item.AnaYemek;
                    worksheet.Cell(row, 6).Value = item.YardimciYemek;
                    worksheet.Cell(row, 7).Value = item.TatliMeyve;
                    worksheet.Cell(row, 8).Value = item.ToplamKalori;
                }

                worksheet.Columns().AdjustToContents();

                using var stream = new MemoryStream();
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Yemek_Listesi_{DateTime.Now:yyyyMMdd}.xlsx");
                }
            }
        }



        // ==========================================
        // --- ANA SAYFA SLIDER GÖRSELLERİ PANELİ ---
        // ==========================================
        public async Task<IActionResult> AnaSayfaGorsel()
        {
            var liste = await _context.Medyalar
                .Where(x => x.Alan == "AnaSayfa")
                .OrderBy(x => x.SiraNo)
                .ThenByDescending(x => x.YuklenmeTarihi)
                .ToListAsync();

            return View(liste);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnaSayfaGorselYukle(string title, IFormFile imageFile, bool isSlider, string? hedefUrl, int siraNo = 0)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                TempData["Error"] = "Lütfen bir görsel seçin.";
                return RedirectToAction(nameof(AnaSayfaGorsel));
            }

            var uploadPath = Path.Combine(_env.WebRootPath, "uploads", "slider", "anasayfa");
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

            var ext = Path.GetExtension(imageFile.FileName);
            var fileName = Guid.NewGuid() + ext;
            var fullPath = Path.Combine(uploadPath, fileName);

            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            var medya = new Medya
            {
                Baslik = title,
                Alan = "AnaSayfa",
                GorselYolu = "/uploads/slider/anasayfa/" + fileName,
                IsSlider = isSlider,
                HedefUrl = string.IsNullOrWhiteSpace(hedefUrl) ? null : hedefUrl.Trim(),
                SiraNo = siraNo,
                IsActive = true,
                YuklenmeTarihi = DateTime.UtcNow
            };

            _context.Medyalar.Add(medya);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Ana sayfa slider görseli eklendi.";
            return RedirectToAction(nameof(AnaSayfaGorsel));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnaSayfaGorselDuzenle(int id, string title, bool isSlider, string? hedefUrl, int siraNo = 0, bool isActive = true, IFormFile? imageFile = null)
        {
            var medya = await _context.Medyalar.FirstOrDefaultAsync(x => x.Id == id && x.Alan == "AnaSayfa");
            if (medya == null) return NotFound();

            medya.Baslik = title;
            medya.IsSlider = isSlider;
            medya.HedefUrl = string.IsNullOrWhiteSpace(hedefUrl) ? null : hedefUrl.Trim();
            medya.SiraNo = siraNo;
            medya.IsActive = isActive;

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadPath = Path.Combine(_env.WebRootPath, "uploads", "slider", "anasayfa");
                if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                var ext = Path.GetExtension(imageFile.FileName);
                var fileName = Guid.NewGuid() + ext;
                var fullPath = Path.Combine(uploadPath, fileName);

                await using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                medya.GorselYolu = "/uploads/slider/anasayfa/" + fileName;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Ana sayfa slider görseli güncellendi.";
            return RedirectToAction(nameof(AnaSayfaGorsel));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnaSayfaGorselSil(int id)
        {
            var medya = await _context.Medyalar.FirstOrDefaultAsync(x => x.Id == id && x.Alan == "AnaSayfa");
            if (medya != null)
            {
                _context.Medyalar.Remove(medya);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Görsel silindi.";
            return RedirectToAction(nameof(AnaSayfaGorsel));
        }

        [HttpPost]
        public async Task<IActionResult> AnaSayfaSliderDurumGuncelle(int id, bool status)
        {
            var medya = await _context.Medyalar.FirstOrDefaultAsync(x => x.Id == id && x.Alan == "AnaSayfa");
            if (medya == null) return NotFound();

            medya.IsSlider = status;
            await _context.SaveChangesAsync();
            return Ok();
        }


    }
}