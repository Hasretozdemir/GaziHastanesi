using GaziHastane.Models;
using GaziHastane.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GaziHastane.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GaziHastaneContext _context;

        public HomeController(ILogger<HomeController> logger, GaziHastaneContext context)
        {
            _logger = logger;
            _context = context;
        }

        // ==========================================
        // ZİYARETÇİ (ÖNYÜZ) SAYFALARI
        // ==========================================

        public IActionResult Index()
        {
            var viewModel = new HomeViewModel
            {
                // IsActive olan son 2 haberi getir
                Haberler = _context.Haberler
                                    .Where(h => h.IsActive)
                                    .OrderByDescending(h => h.YayinTarihi)
                                    .Take(2)
                                    .ToList(),

                // IsActive olan yaklaşan son 2 etkinliği getir
                Etkinlikler = _context.Etkinlikler
                                      .Where(e => e.IsActive && e.Tarih >= System.DateTime.Today)
                                      .OrderBy(e => e.Tarih)
                                      .Take(2)
                                      .ToList(),

                // DUYURULARI VERİTABANINDAN ÇEKİYORUZ (Eksik olan kısım eklendi)
                // Not: Eğer hata alırsanız veritabanında IsActive sütunu olup olmadığını kontrol edin.
                Duyurular = _context.Duyurular
                                    .Where(d => d.IsActive)
                                    .OrderByDescending(d => d.YayinTarihi)
                                    .ToList()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Bolumler()
        {
            var aktifBolumler = await _context.Bolumler
                                              .Where(b => b.IsActive)
                                              .ToListAsync();
            return View(aktifBolumler);
        }

        public async Task<IActionResult> Doktorlar()
        {
            var aktifDoktorlar = await _context.Doktorlar
                                               .Include(d => d.Bolum)
                                               .Where(d => d.IsActive)
                                               .ToListAsync();
            return View(aktifDoktorlar);
        }

        public async Task<IActionResult> Rehber()
        {
            var rehberVerileri = await _context.HastaRehberleri
                                               .Where(x => x.IsActive)
                                               .OrderBy(x => x.SiraNo)
                                               .ToListAsync();
            return View(rehberVerileri);
        }

        public async Task<IActionResult> Iletisim()
        {
            ViewBag.Lokasyonlar = await _context.IletisimBilgileri.Where(x => x.IsActive).ToListAsync();
            ViewBag.UlasimAraclari = await _context.UlasimRehberleri.Where(x => x.IsActive).ToListAsync();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // ==========================================
        // SİSTEM METODLARI (HATA YAKALAMA)
        // ==========================================
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}