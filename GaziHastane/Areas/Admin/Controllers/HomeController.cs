using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GaziHastane.Data;

[Area("Admin")]
[Authorize]
public class HomeController : Controller
{
    private readonly GaziHastaneContext _context;

    public HomeController(GaziHastaneContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var bugunBaslangic = DateTime.Today;
        var yarinBaslangic = bugunBaslangic.AddDays(1);

        var toplamDoktor = _context.Doktorlar.Count();
        var aktifDoktor = _context.Doktorlar.Count(x => x.IsActive);
        var bugunkuRandevu = _context.Randevular.Count(x => x.RandevuTarihi >= bugunBaslangic && x.RandevuTarihi < yarinBaslangic);
        var aktifBolum = _context.Bolumler.Count(x => x.IsActive);

        var bugunkuRandevuDagilim = _context.Randevular
            .Where(x => x.RandevuTarihi >= bugunBaslangic && x.RandevuTarihi < yarinBaslangic)
            .GroupBy(x => x.Bolum != null ? x.Bolum.Ad : "Diğer")
            .Select(x => new { BolumAdi = x.Key, Adet = x.Count() })
            .OrderByDescending(x => x.Adet)
            .Take(3)
            .ToList()
            .Select(x => new KeyValuePair<string, int>(x.BolumAdi, x.Adet))
            .ToList();

        var kapasiteAyari = _context.PanelAyarlari
            .Where(x => x.AyarKey == "DoktorGunlukRandevuKapasitesi")
            .Select(x => x.AyarValue)
            .FirstOrDefault();

        var doktorBasinaGunlukKapasite = int.TryParse(kapasiteAyari, out var parsedKapasite) && parsedKapasite > 0
            ? parsedKapasite
            : 24;

        var toplamGunlukKapasite = aktifDoktor * doktorBasinaGunlukKapasite;
        var sistemYuku = toplamGunlukKapasite == 0
            ? 0
            : Math.Min(100, (int)Math.Round((double)bugunkuRandevu * 100 / toplamGunlukKapasite));

        var aktifKisiler = _context.Doktorlar
            .Include(x => x.Bolum)
            .Where(x => x.IsActive)
            .OrderBy(x => x.Ad)
            .ThenBy(x => x.Soyad)
            .Take(5)
            .ToList();

        ViewBag.ToplamDoktor = toplamDoktor;
        ViewBag.BugunkuRandevu = bugunkuRandevu;
        ViewBag.BugunkuRandevuDagilim = bugunkuRandevuDagilim;
        ViewBag.AktifBolum = aktifBolum;
        ViewBag.SistemYuku = sistemYuku;
        ViewBag.DoktorBasinaGunlukKapasite = doktorBasinaGunlukKapasite;
        ViewBag.AktifKisiler = aktifKisiler;

        return View();
    }
}