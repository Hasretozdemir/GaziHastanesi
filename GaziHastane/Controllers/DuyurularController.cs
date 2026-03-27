using GaziHastane.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GaziHastane.Controllers
{
    public class DuyurularController : Controller
    {
        private readonly GaziHastaneContext _context;

        public DuyurularController(GaziHastaneContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var duyurular = await _context.Duyurular
                                          .Where(d => d.IsActive)
                                          .OrderByDescending(d => d.YayinTarihi)
                                          .ToListAsync();
            return View(duyurular);
        }
    }
}