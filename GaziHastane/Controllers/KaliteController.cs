using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GaziHastane.Controllers
{
    public class KaliteController : Controller
    {
        private readonly GaziHastaneContext _context;

        public KaliteController(GaziHastaneContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Kategorilere göre gruplayýp gönderebiliriz veya direkt liste basabiliriz
            var belgeler = await _context.KaliteBelgeleri.OrderBy(x => x.Kategori).ToListAsync();
            return View(belgeler);
        }
    }
}