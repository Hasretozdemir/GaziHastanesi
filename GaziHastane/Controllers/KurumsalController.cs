using Microsoft.AspNetCore.Mvc;

namespace GaziHastane.Controllers
{
    public class KurumsalController : Controller
    {
        // Hakkýmýzda (Kurumsal Ana Sayfa)
        public IActionResult Index() { return View(); }

        // Baþhekimlik
        public IActionResult Bashekimlik() { return View(); }

        // Baþmüdürlük
        public IActionResult Basmudurluk() { return View(); }
    }
}
