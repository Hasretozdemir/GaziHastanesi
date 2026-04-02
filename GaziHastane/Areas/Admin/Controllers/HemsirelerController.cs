using Microsoft.AspNetCore.Mvc;

namespace GaziHastane.Areas.Admin.Controllers
{
    public class HemsirelerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
