using GaziHastane.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GaziHastane.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // ==========================================
        // ZİYARETÇİ (ÖNYÜZ) SAYFALARI
        // ==========================================
        public IActionResult Index() { return View(); }
        public IActionResult Bolumler() { return View(); }
        public IActionResult Doktorlar() { return View(); }
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