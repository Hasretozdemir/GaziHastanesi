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

        // --- KALİTE YÖNETİMİ PANELİ ---
        public async Task<IActionResult> Kalite()
        {
            var liste = await _context.KaliteBelgeleri.OrderByDescending(x => x.YayinTarihi).ToListAsync();
            return View(liste);
        }

        // --- VERİ GETİR (Düzenleme İçin AJAX) ---
        [HttpGet]
        public async Task<IActionResult> GetKaliteDetay(int id)
        {
            var veri = await _context.KaliteBelgeleri.FindAsync(id);
            if (veri == null) return NotFound();
            return Json(veri);
        }

        // --- KAYDET / GÜNCELLE ---
        [HttpPost]
        public async Task<IActionResult> KaliteKaydet(KaliteBelgesi model, IFormFile? fotoDosya, IFormFile? belgeDosya)
        {
            // Dosya Kayıt Klasörleri
            string uploadPath = Path.Combine(_env.WebRootPath, "uploads", "kalite");
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

            // Fotoğraf Yükleme
            if (fotoDosya != null)
            {
                string fotoName = Guid.NewGuid() + Path.GetExtension(fotoDosya.FileName);
                using (var stream = new FileStream(Path.Combine(uploadPath, fotoName), FileMode.Create))
                {
                    await fotoDosya.CopyToAsync(stream);
                }
                model.FotoUrl = "/uploads/kalite/" + fotoName;
            }

            // Belge Yükleme
            if (belgeDosya != null)
            {
                string belgeName = Guid.NewGuid() + Path.GetExtension(belgeDosya.FileName);
                using (var stream = new FileStream(Path.Combine(uploadPath, belgeName), FileMode.Create))
                {
                    await belgeDosya.CopyToAsync(stream);
                }
                model.DosyaUrl = "/uploads/kalite/" + belgeName;
            }

            // 🔥 HATA ÇÖZÜMÜ BURADA: Eğer formdan dosya seçilmediyse veritabanı hata vermesin diye varsayılan değer "#" atanır.
            if (string.IsNullOrEmpty(model.DosyaUrl))
            {
                model.DosyaUrl = "#";
            }

            if (model.Id == 0) // Yeni Kayıt
            {
                model.YayinTarihi = DateTime.UtcNow;
                _context.KaliteBelgeleri.Add(model);
                TempData["Success"] = "Yeni kalite kartı oluşturuldu.";
            }
            else // Güncelleme
            {
                var eskiVeri = await _context.KaliteBelgeleri.AsNoTracking().FirstOrDefaultAsync(x => x.Id == model.Id);

                // Eski fotoğrafı koruma
                if (model.FotoUrl == null) model.FotoUrl = eskiVeri?.FotoUrl;

                // Eski belgeyi koruma (Eğer yeni dosya yüklenmediği için "#" atandıysa, veritabanındaki eski dosyayı al)
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
        // 🔥 YENİ EKLENEN METOTLAR (MENÜ LİNKLERİ İÇİN)
        // ==========================================

        // --- YEMEK LİSTESİ PANELİ ---
        public IActionResult YemekListesi()
        {
            // Veritabanı bağlaman gerekirse:
            // var liste = await _context.YemekListeleri.ToListAsync();
            // return View(liste);
            return View();
        }

        // --- GÖRSELLER PANELİ ---
        public IActionResult Gorsel()
        {
            return View();
        }

        // --- BELGELER PANELİ ---
        public IActionResult Belge()
        {
            return View();
        }
    }
}