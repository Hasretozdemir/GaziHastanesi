using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GaziHastane.Data;

namespace GaziHastane.Controllers
{
    public class DbFixController : Controller
    {
        private readonly GaziHastaneContext _context;

        public DbFixController(GaziHastaneContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult RunFix()
        {
            _context.Database.ExecuteSqlRaw("ALTER TABLE \"BorclarOdemeler\" ADD COLUMN IF NOT EXISTS \"ProtakolNo\" character varying(30);");
            _context.Database.ExecuteSqlRaw("ALTER TABLE \"BorclarOdemeler\" ADD COLUMN IF NOT EXISTS \"EklenmeTarihi\" timestamp without time zone;");
            
            _context.Database.ExecuteSqlRaw("INSERT INTO \"AdminMenuItems\" (\"Section\", \"Url\", \"Label\", \"IconClass\", \"SortOrder\", \"IsSuperAdminOnly\", \"IsActive\") VALUES ('Main', '/Admin/BorcYonetim/Index', 'Borç Yönetimi', 'fa-solid fa-money-bill-wave', 99, false, true) ON CONFLICT DO NOTHING;");
            
            return Content("OK");
        }
    }
}
