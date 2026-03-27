using GaziHastane.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GaziHastane.Controllers
{
    public class EtkinliklerController : Controller
    {
        private readonly GaziHastaneContext _context;

        public EtkinliklerController(GaziHastaneContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var etkinlikler = await _context.Etkinlikler
                                            .Where(e => e.IsActive)
                                            .OrderByDescending(e => e.Tarih)
                                            .ToListAsync();
            return View(etkinlikler);
        }
    }
}