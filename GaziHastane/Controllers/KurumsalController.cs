using GaziHastane.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GaziHastane.Controllers
{
    public class KurumsalController : Controller
    {
        private readonly Data.GaziHastaneContext _context;

        public KurumsalController(Data.GaziHastaneContext context)
        {
            _context = context;
        }

        // Hakkï¿½mï¿½zda (Kurumsal Ana Sayfa)
        public async Task<IActionResult> Index()
        {
            var icerikler = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "hakkimizda" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            var sekmeler = await _context.KurumsalSekmeler
                .Where(x => x.SayfaKey == "hakkimizda" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            ViewBag.Sekmeler = sekmeler;
            return View("Index", icerikler);
        }

        // Baï¿½hekimlik
        public IActionResult Bashekimlik()
        {
            // Veritabanï¿½ndan aktif personelleri sï¿½rasï¿½na gï¿½re ï¿½ekiyoruz
            var aktifPersoneller = _context.BashekimlikPersoneller
                                           .Where(x => x.AktifMi)
                                           .OrderBy(x => x.Sira)
                                           .ToList();

            // Verileri ViewModel'e dolduruyoruz
            var viewModel = new BashekimlikViewModel
            {
                // IsBashekim = true olan ï¿½LK kaydï¿½ Baï¿½hekim olarak al
                Bashekim = aktifPersoneller.FirstOrDefault(x => x.IsBashekim),

                // IsBashekim = false olanlarï¿½ Yardï¿½mcï¿½lar listesine al
                Yardimcilar = aktifPersoneller.Where(x => !x.IsBashekim).ToList(),

                // ï¿½letiï¿½im bilgilerini burada tanï¿½mlï¿½yoruz
                Telefon = "(0312) 202 40 00",
                CalismaSaatleri = "Pztï¿½Cuma ï¿½ 08:30 ï¿½ 17:00"
            };

            return View(viewModel);
        }

        // Baï¿½mï¿½dï¿½rlï¿½k
        public IActionResult Basmudurluk()
        {
            var aktifPersoneller = _context.BasmudurlikPersoneller
                                           .Where(x => x.AktifMi)
                                           .OrderBy(x => x.Sira)
                                           .ToList();

            var viewModel = new BasmudurlikViewModel
            {
                Basmudur = aktifPersoneller.FirstOrDefault(x => x.IsBasmudur),
                Yardimcilar = aktifPersoneller.Where(x => !x.IsBasmudur).ToList(),
                Telefon = "(0312) 202 40 00",
                CalismaSaatleri = "Pztï¿½Cuma ï¿½ 08:30 ï¿½ 17:00"
            };

            return View(viewModel);
        }

        // Hemï¿½irelik Hizmetleri (Dï¿½NAMï¿½K HALE GETï¿½Rï¿½LDï¿½)
        public IActionResult HemsirelikHizmetleri()
        {
            // Aktif olan tï¿½m iï¿½erikleri sï¿½rasï¿½na gï¿½re tek seferde ï¿½ekiyoruz
            var tumIcerikler = _context.HemsirelikIcerikler
                                       .Where(x => x.AktifMi)
                                       .OrderBy(x => x.Sira)
                                       .ToList();

            var sekmeler = _context.HemsirelikSekmeler
                .Where(x => x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToList();

            var viewModel = new HemsirelikViewModel
            {
                // Ayarlar tablosundan ilk kaydï¿½ al, yoksa boï¿½ bir nesne gï¿½nder (hata vermemesi iï¿½in)
                Ayarlar = _context.HemsirelikAyarlar.FirstOrDefault() ?? new HemsirelikAyar(),
                Sekmeler = sekmeler,

                // Tek tabloyu Kategori sï¿½tununa gï¿½re View'daki ilgili listelere paylaï¿½tï¿½rï¿½yoruz
                YonetimKadrosu = tumIcerikler.Where(x => x.Kategori == "Yonetim").ToList(),
                Gorevler = tumIcerikler.Where(x => x.Kategori == "Gorev").ToList(),
                Mevzuatlar = tumIcerikler.Where(x => x.Kategori == "Mevzuat").ToList(),
                GaleriFotograflari = tumIcerikler.Where(x => x.Kategori == "Galeri").ToList(),
                Etkinlikler = tumIcerikler.Where(x => x.Kategori == "Etkinlik").ToList(),
                AkisSemalari = tumIcerikler.Where(x => x.Kategori == "Akis").ToList(),
                TumIcerikler = tumIcerikler
            };

            return View(viewModel);
        }

        public async Task<IActionResult> BilgiIslem()
        {
            var icerikler = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "BilgiIslemMerkezi" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            var sekmeler = await _context.KurumsalSekmeler
                .Where(x => x.SayfaKey == "BilgiIslemMerkezi" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            ViewBag.Sekmeler = sekmeler;
            return View("Index", icerikler);
        }

        public async Task<IActionResult> IsSagligi()
        {
            var icerikler = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "IsSagligiVeGuvenligi" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            var sekmeler = await _context.KurumsalSekmeler
                .Where(x => x.SayfaKey == "IsSagligiVeGuvenligi" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            ViewBag.Sekmeler = sekmeler;
            return View("Index", icerikler);
        }

        // Enfeksiyon Kontrol
        public async Task<IActionResult> Enfeksiyon()
        {
            var icerikler = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "EnfeksiyonKontrol" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            var sekmeler = await _context.KurumsalSekmeler
                .Where(x => x.SayfaKey == "EnfeksiyonKontrol" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            ViewBag.Sekmeler = sekmeler;
            return View("Index", icerikler);
        }

        // EczacÄ±lÄ±k Hizmetleri
        public async Task<IActionResult> Eczacilik()
        {
            var icerikler = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "EczacilikHizmetleri" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            var sekmeler = await _context.KurumsalSekmeler
                .Where(x => x.SayfaKey == "EczacilikHizmetleri" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            ViewBag.Sekmeler = sekmeler;
            return View("Index", icerikler);
        }

        // SatÄ±n Alma
        public async Task<IActionResult> SatinAlma()
        {
            var icerikler = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "SatinAlma" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            var sekmeler = await _context.KurumsalSekmeler
                .Where(x => x.SayfaKey == "SatinAlma" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            ViewBag.Sekmeler = sekmeler;
            return View("Index", icerikler);
        }

        // Ä°statistik ve Raporlama
        public async Task<IActionResult> Istatistik()
        {
            var icerikler = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "IstatistikVeRaporlama" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            var sekmeler = await _context.KurumsalSekmeler
                .Where(x => x.SayfaKey == "IstatistikVeRaporlama" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            ViewBag.Sekmeler = sekmeler;
            return View("Index", icerikler);
        }

        // ArÅŸiv Birimi
        public async Task<IActionResult> Arsiv()
        {
            var icerikler = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "ArsivBirimi" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            var sekmeler = await _context.KurumsalSekmeler
                .Where(x => x.SayfaKey == "ArsivBirimi" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            ViewBag.Sekmeler = sekmeler;
            return View("Index", icerikler);
        }

        // Hasta Ä°letiÅŸim Birimi
        public async Task<IActionResult> HastaIletisim()
        {
            var icerikler = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "HastaIletisimBirimi" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            var sekmeler = await _context.KurumsalSekmeler
                .Where(x => x.SayfaKey == "HastaIletisimBirimi" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            ViewBag.Sekmeler = sekmeler;
            return View("Index", icerikler);
        }

        // Ä°ÅŸ AkÄ±ÅŸ ÅemalarÄ±
        public async Task<IActionResult> IsAkis()
        {
            var icerikler = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "IsAkisSemalari" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            var sekmeler = await _context.KurumsalSekmeler
                .Where(x => x.SayfaKey == "IsAkisSemalari" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            ViewBag.Sekmeler = sekmeler;
            return View("Index", icerikler);
        }

        // Organizasyon emalar
        public async Task<IActionResult> Organizasyon()
        {
            var icerikler = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "OrganizasyonSemalari" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            var sekmeler = await _context.KurumsalSekmeler
                .Where(x => x.SayfaKey == "OrganizasyonSemalari" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            ViewBag.Sekmeler = sekmeler;
            return View("Index", icerikler);
        }

        // Ä°Ã§ Kontrol
        public async Task<IActionResult> IcKontrol()
        {
            var icerikler = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "IcKontrol" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            var sekmeler = await _context.KurumsalSekmeler
                .Where(x => x.SayfaKey == "IcKontrol" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            ViewBag.Sekmeler = sekmeler;
            return View("Index", icerikler);
        }

        // BasÄ±n ve Kurumsal Ä°letiÅŸim
        public async Task<IActionResult> BasinIletisim()
        {
            var model = await _context.BasinKurumsalIletisimler.FirstOrDefaultAsync();
            if (model == null)
            {
                // VarsayÄ±lan deÄŸerler
                model = new BasinKurumsalIletisim
                {
                    Baslik = "BasÄ±n ve Kurumsal Ä°letiÅŸim Birimi",
                    Aciklama = "SaÄŸlÄ±k AraÅŸtÄ±rma ve Uygulama Merkezimiz faaliyetleri Ã§erÃ§evesinde; hedef kitlelerle etkili bir iletiÅŸim kurmak ve sunulan saÄŸlÄ±k hizmetinin yanÄ± sÄ±ra hastanemizin gerÃ§ekleÅŸtirdiÄŸi yeniliklerden hem personelimizi ve hem de dÄ±ÅŸ paydaÅŸlarÄ± haberdar etmek amacÄ±yla hastanemiz web sayfasÄ±na ve kurum iÃ§i SMS faaliyetlerine yÃ¶nelik sÃ¼reÃ§lerin takibi ve koordinasyonunun saÄŸlanmasÄ± adÄ±na BaÅŸhekimlik makamÄ±nÄ±n 22.07.2025 tarihli Oluru doÄŸrultusunda \"BasÄ±n ve Kurumsal Ä°letiÅŸim Birimi\"miz kurulmuÅŸtur.",
                    Telefon = "0312 202 44 39",
                    Email = "gazihastanesibasin@gazi.edu.tr",
                    Lokasyon = "E Blok 11. Kat"
                };
            }
            return View(model);
        }
        public async Task<IActionResult> Hakkimizda()
        {
            return await Index();
        }
    }
}
