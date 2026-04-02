using GaziHastane.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GaziHastane.Controllers
{
    public class BasmudurlikController : Controller
    {
        private readonly Data.GaziHastaneContext _context;

        public BasmudurlikController(Data.GaziHastaneContext context)
        {
            _context = context;
        }

        // Baþmüdürlük Sayfasý
        public IActionResult Index()
        {
            // Veritabanýndan aktif personelleri sýrasýna göre çekiyoruz
            var aktifPersoneller = _context.BasmudurlikPersoneller
                                           .Where(x => x.AktifMi)
                                           .OrderBy(x => x.Sira)
                                           .ToList();

            // Verileri ViewModel'e dolduruyoruz
            var viewModel = new BasmudurlikViewModel
            {
                // IsBasmudur = true olan ÝLK kaydý Baþmüdür olarak al
                Basmudur = aktifPersoneller.FirstOrDefault(x => x.IsBasmudur),

                // IsBasmudur = false olanlarý Yardýmcýlar listesine al
                Yardimcilar = aktifPersoneller.Where(x => !x.IsBasmudur).ToList(),

                // Ýletiþim bilgilerini burada tanýmlýyoruz
                Telefon = "(0312) 202 40 00",
                CalismaSaatleri = "Pzt–Cuma · 08:30 – 17:00"
            };

            return View("~/Views/Kurumsal/Basmudurluk.cshtml", viewModel);
        }
    }
}
