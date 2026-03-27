using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize] // Sadece giriş yapmış adminler görebilsin
    public class KrokiController : Controller
    {
        private readonly GaziHastaneContext _context;

        public KrokiController(GaziHastaneContext context)
        {
            _context = context;
        }

        // Krokileri listelediğimiz ana admin sayfası
        public async Task<IActionResult> Index()
        {
            // Veritabanındaki kroki birimlerini, bağlı olduğu Poliklinik (Bolum) bilgisiyle çekiyoruz
            var birimler = await _context.KrokiBirimleri.Include(k => k.Bolum).ToListAsync();

            // Modal içindeki "Bağlı Poliklinik" Dropdown'ı (Seçim Kutusu) için aktif bölümleri gönderiyoruz
            ViewBag.Bolumler = new SelectList(await _context.Bolumler.Where(b => b.IsActive).ToListAsync(), "Id", "Ad");

            return View(birimler);
        }

        // Yeni oda ekleme veya var olanı güncelleme metodu (Modal içindeki form buraya post edilir)
        [HttpPost]
        public async Task<IActionResult> Kaydet(KrokiBirim model)
        {
            if (model.Id == 0)
            {
                // Yeni Kayıt
                _context.KrokiBirimleri.Add(model);
                TempData["Success"] = "Yeni birim krokiye başarıyla eklendi.";
            }
            else
            {
                // Güncelleme
                _context.KrokiBirimleri.Update(model);
                TempData["Success"] = "Kroki birimi güncellendi.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Odaları silme metodu
        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var veri = await _context.KrokiBirimleri.FindAsync(id);
            if (veri != null)
            {
                _context.KrokiBirimleri.Remove(veri);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Birim haritadan silindi.";
            }
            return RedirectToAction("Index");
        }
    }
}