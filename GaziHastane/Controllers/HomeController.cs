using GaziHastane.Models;
using GaziHastane.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // ILogger için gereken kütüphane eklendi
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GaziHastane.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GaziHastaneContext _context;

        // Hem Logger hem de Context aynı anda projeye dahil ediliyor
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
            return View();
        }

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
                                               .Where(d => d.IsActive)
                                               .ToListAsync();

            return View(aktifDoktorlar);
        }

        // 1. Metodu "async Task<IActionResult>" olarak güncelleyin
        public async Task<IActionResult> Rehber()
        {
            // 2. Veritabanındaki 'HastaRehberleri' tablosundan aktif kayıtları liste olarak çekin
            var rehberVerileri = await _context.HastaRehberleri
                                               .Where(x => x.IsActive)
                                               .OrderBy(x => x.SiraNo)
                                               .ToListAsync();

            // 3. Çekilen bu listeyi View'a (sayfaya) gönderin
            return View(rehberVerileri);
        }
        public async Task<IActionResult> Iletisim()
        {
            // Veritabanından aktif iletişim verileri ViewBag ile sayfaya gönderiliyor
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