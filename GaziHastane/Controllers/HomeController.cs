using GaziHastane.Models;
using GaziHastane.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Index() { return View(); }

        public async Task<IActionResult> Bolumler()
        {
            // Veritabanındaki Bolumler tablosundan aktif olanları listeye çevirip çekiyoruz
            var aktifBolumler = await _context.Bolumler
                                              .Where(b => b.IsActive)
                                              .ToListAsync();

            // Çekilen veriyi View'a gönderiyoruz
            return View(aktifBolumler);
        }
        public async Task<IActionResult> Doktorlar()
        {
            // Include ile doktorların bölümlerini de çekiyoruz
            var aktifDoktorlar = await _context.Doktorlar
                                              .Include(d => d.Bolum)
                                              .Where(b => b.IsActive)
                                              .ToListAsync();

            return View(aktifDoktorlar);
        }
        public IActionResult Rehber() { return View(); }
        public IActionResult Iletisim() { return View(); }
        public IActionResult Privacy() { return View(); }

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