using GaziHastane.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GaziHastane.Controllers
{
    public class HaberlerController : Controller
    {
        private readonly GaziHastaneContext _context;

        public HaberlerController(GaziHastaneContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Tüm aktif haberleri tarihe göre yeniden eskiye sıralayarak getiriyoruz
            var haberler = await _context.Haberler
                                         .Where(h => h.IsActive)
                                         .OrderByDescending(h => h.YayinTarihi)
                                         .ToListAsync();
            return View(haberler);
        }
    }
}