using Microsoft.AspNetCore.Mvc;
using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.EntityFrameworkCore;

namespace GaziHastane.Controllers
{
    public class YemekController : Controller
    {
        private readonly GaziHastaneContext _context;

        public YemekController(GaziHastaneContext context)
        {
            _context = context;
        }

        // Günlük yemek listesini getiren aksiyon
        public IActionResult Liste()
        {
            // PostgreSQL UTC uyumluluðu için UtcNow kullanýlmýþtýr
            var bugun = DateTime.UtcNow.Date;

            var gunlukListe = _context.YemekListesi
                                      .Where(x => x.Tarih >= bugun && x.Tarih < bugun.AddDays(1))
                                      .OrderBy(x => x.Ogun)
                                      .ToList();

            return View(gunlukListe);
        }

        // Tüm aylýk listeyi veritabanýndan çeken aksiyon
        public async Task<IActionResult> AylikListe()
        {
            // Veritabanýndaki tüm yemek listesini çekiyoruz
            // Tarihe göre baþtan sona (OrderBy) ve öðün sýrasýna göre diziyoruz
            var liste = await _context.YemekListesi
                                      .OrderBy(x => x.Tarih)
                                      .ThenBy(x => x.Ogun)
                                      .ToListAsync();

            // Çekilen listeyi View'a (AylikListe.cshtml) gönderiyoruz
            return View(liste);
        }
    }
}
