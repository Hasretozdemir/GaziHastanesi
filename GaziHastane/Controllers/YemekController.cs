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

        public IActionResult Liste() 
        { 
            // Bugünün tarihine göre yemek listesini getir
            // (Demo amaçlı 2026 yılına sabitlenmiş olabilir, ama normalde DateTime.Today kullanılır)
            // Eğer veri yoksa boş liste döner, view tarafında kontrol edilir.
            var bugun = DateTime.Today; 
            
            // Veritabanından bugüne ait kayıtları çekiyoruz (Sabah, Öğle, Akşam)
            var gunlukListe = _context.YemekListesi
                                      .Where(x => x.Tarih.Date == bugun)
                                      .OrderBy(x => x.Ogun)
                                      .ToList();

            return View(gunlukListe); 
        }

        public async Task<IActionResult> AylikListe() 
        {
            // Tüm aylık listeyi getir
            var liste = await _context.YemekListesi
                                      .OrderByDescending(x => x.Tarih)
                                      .ThenBy(x => x.Ogun)
                                      .ToListAsync();
            return View(liste); 
        }
    }
}
//Hastane çalışanlarının ve refakatçilerin aylık yemek menüsünü dijital ortamda takip edebilmesi için eklenmiş.