using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class EgitimController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly GaziHastaneContext _context;

        // Dependency Injection ile gerekli servisleri alýyoruz
        public EgitimController(IWebHostEnvironment env, GaziHastaneContext context)
        {
            _env = env;
            _context = context;
        }

        // 1. ANA SAYFA (Admin Panelindeki Liste)
        public async Task<IActionResult> Index()
        {
            var liste = await _context.EgitimIcerikleri
                                      .OrderByDescending(x => x.Id)
                                      .ToListAsync();
            return View(liste);
        }

        // 2. VERÝ GETÝR (AJAX - Modal Ýçini Doldurmak Ýçin)
        [HttpGet]
        public async Task<IActionResult> GetEgitimDetay(int id)
        {
            var veri = await _context.EgitimIcerikleri.FindAsync(id);
            if (veri == null) return NotFound();

            return Json(veri);
        }

        // 3. KAYDET VEYA GÜNCELLE
        [HttpPost]
        public async Task<IActionResult> EgitimKaydet(EgitimKarti model, IFormFile? fotoDosya, IFormFile? belgeDosya)
        {
            // Yükleme klasörünü oluþtur
            string uploadPath = Path.Combine(_env.WebRootPath, "uploads", "egitim");
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

            // A. Fotoðraf Yüklendiyse
            if (fotoDosya != null)
            {
                string fotoName = Guid.NewGuid() + Path.GetExtension(fotoDosya.FileName);
                using (var stream = new FileStream(Path.Combine(uploadPath, fotoName), FileMode.Create))
                {
                    await fotoDosya.CopyToAsync(stream);
                }
                model.FotoUrl = "/uploads/egitim/" + fotoName;
            }

            // B. Belge Yüklendiyse
            if (belgeDosya != null)
            {
                string belgeName = Guid.NewGuid() + Path.GetExtension(belgeDosya.FileName);
                using (var stream = new FileStream(Path.Combine(uploadPath, belgeName), FileMode.Create))
                {
                    await belgeDosya.CopyToAsync(stream);
                }
                model.DosyaUrl = "/uploads/egitim/" + belgeName;
            }

            // PostgreSQL null hatasýný önlemek için boþ belgeye varsayýlan deðer atama
            if (string.IsNullOrEmpty(model.DosyaUrl))
            {
                model.DosyaUrl = "#";
            }

            // Veritabaný Ýþlemleri
            if (model.Id == 0)
            {
                // YENÝ KAYIT
                _context.EgitimIcerikleri.Add(model);
                TempData["Success"] = "Yeni eðitim kartý baþarýyla oluþturuldu.";
            }
            else
            {
                // GÜNCELLEME
                var eskiVeri = await _context.EgitimIcerikleri.AsNoTracking().FirstOrDefaultAsync(x => x.Id == model.Id);

                // Eðer yeni dosya yüklenmediyse eski dosyalarý koru
                if (model.FotoUrl == null) model.FotoUrl = eskiVeri?.FotoUrl;
                if (model.DosyaUrl == "#") model.DosyaUrl = eskiVeri?.DosyaUrl ?? "#";

                _context.EgitimIcerikleri.Update(model);
                TempData["Success"] = "Eðitim içeriði güncellendi.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // 4. SÝLME ÝÞLEMÝ
        [HttpPost]
        public async Task<IActionResult> EgitimSil(int id)
        {
            var veri = await _context.EgitimIcerikleri.FindAsync(id);
            if (veri != null)
            {
                _context.EgitimIcerikleri.Remove(veri);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Kart baþarýyla silindi.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}