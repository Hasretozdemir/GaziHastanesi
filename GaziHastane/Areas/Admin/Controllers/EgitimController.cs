using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class EgitimController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
