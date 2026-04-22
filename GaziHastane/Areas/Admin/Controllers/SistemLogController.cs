using GaziHastane.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SistemLogController : Controller
    {
        private readonly GaziHastaneContext _context;

        public SistemLogController(GaziHastaneContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var loglar = await _context.AdminLoglari
                .OrderByDescending(x => x.Tarih)
                .Take(500)
                .ToListAsync();

            return View(loglar);
        }
    }
}
