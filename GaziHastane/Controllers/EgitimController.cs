using Microsoft.AspNetCore.Mvc;
using GaziHastane.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace GaziHastane.Controllers
{
    public class EgitimController : Controller
    {
        private readonly GaziHastaneContext _context;

        // Dependency Injection ile veritabaný baðlamýný alýyoruz
        public EgitimController(GaziHastaneContext context)
        {
            _context = context;
        }

        // 1. ANA PORTAL SAYFASI (Kartlarýn listelendiði ana menü)
        // Tarayýcýda /Egitim/Index adresine gidildiðinde bu çalýþýr.
        public async Task<IActionResult> Index()
        {
            // Veritabanýndaki Egitim kartlarýný listele
            // (Not: EgitimIcerikleri tablosunun adýný kendi DbContext modeline göre düzenle)
            var egitimKartlari = await _context.EgitimIcerikleri
                                               .OrderBy(x => x.Id)
                                               .ToListAsync();

            return View(egitimKartlari);
        }

        // 2. KULLANICI TARAFI AJAX VERÝ GETÝRME (Modal Ýçin)
        // Eðer kartýn Tipi "Panel" ise ön yüzdeki JavaScript bu metoda istek atýp veriyi çeker
        [HttpGet]
        public async Task<IActionResult> GetEgitimDetay(int id)
        {
            var detay = await _context.EgitimIcerikleri.FindAsync(id);

            if (detay == null)
                return NotFound();

            return Json(detay);
        }
    }
}