using Microsoft.AspNetCore.Mvc;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BolumlerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}