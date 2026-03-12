using Microsoft.AspNetCore.Mvc;
using GaziHastane.Data;
using System.Linq;

namespace GaziHastane.Controllers
{
    public class EgitimController : Controller
    {
        private readonly GaziHastaneContext _context;

        // Dependency Injection ile veritabaný baðlamýný alýyoruz
        public EgitimController(GaziHastaneContext context)
        {
            _context = context;
        }

        // 1. ANA PORTAL SAYFASI (Kartlarýn listelendiði ana menü)
        // Tarayýcýda /Egitim/Index adresine gidildiðinde bu çalýþýr.
        public IActionResult Index()
        {
            return View();
        }

        // 2. KOMÝTE ÜYELERÝ SAYFASI (Dar yan panelde açýlýr)
        public IActionResult Komite()
        {
            // Veritabanýndaki EgitimKomitesi tablosundan üyeleri çekip View'a gönderiyoruz
            var uyeler = _context.EgitimKomitesi.ToList();
            return View(uyeler);
        }

        // 3. HAKKIMIZDA SAYFASI (Geniþ yan panelde açýlýr)
        public IActionResult Hakkimizda()
        {
            return View();
        }

        // 4. DUYURULAR VE ETKÝNLÝKLER SAYFASI (Geniþ yan panelde açýlýr)
        public IActionResult Duyurular()
        {
            return View();
        }

        // 5. FOTOÐRAF GALERÝSÝ SAYFASI (Geniþ yan panelde açýlýr)
        public IActionResult Galeri()
        {
            return View();
        }
    }
}