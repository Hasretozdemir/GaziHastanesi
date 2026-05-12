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

        // HakkÃ¯Â¿Â½mÃ¯Â¿Â½zda (Kurumsal Ana Sayfa)
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

        // BaÃ¯Â¿Â½hekimlik
        public IActionResult Bashekimlik()
        {
            // VeritabanÃ¯Â¿Â½ndan aktif personelleri sÃ¯Â¿Â½rasÃ¯Â¿Â½na gÃ¯Â¿Â½re Ã¯Â¿Â½ekiyoruz
            var aktifPersoneller = _context.BashekimlikPersoneller
                                           .Where(x => x.AktifMi)
                                           .OrderBy(x => x.Sira)
                                           .ToList();

            // Verileri ViewModel'e dolduruyoruz
            var viewModel = new BashekimlikViewModel
            {
                // IsBashekim = true olan Ã¯Â¿Â½LK kaydÃ¯Â¿Â½ BaÃ¯Â¿Â½hekim olarak al
                Bashekim = aktifPersoneller.FirstOrDefault(x => x.IsBashekim),

                // IsBashekim = false olanlarÃ¯Â¿Â½ YardÃ¯Â¿Â½mcÃ¯Â¿Â½lar listesine al
                Yardimcilar = aktifPersoneller.Where(x => !x.IsBashekim).ToList(),

                // Ã¯Â¿Â½letiÃ¯Â¿Â½im bilgilerini burada tanÃ¯Â¿Â½mlÃ¯Â¿Â½yoruz
                Telefon = "(0312) 202 40 00",
                CalismaSaatleri = "PztÃ¯Â¿Â½Cuma Ã¯Â¿Â½ 08:30 Ã¯Â¿Â½ 17:00"
            };

            return View(viewModel);
        }

        // BaÃ¯Â¿Â½mÃ¯Â¿Â½dÃ¯Â¿Â½rlÃ¯Â¿Â½k
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
                CalismaSaatleri = "PztÃ¯Â¿Â½Cuma Ã¯Â¿Â½ 08:30 Ã¯Â¿Â½ 17:00"
            };

            return View(viewModel);
        }

        // HemÃ¯Â¿Â½irelik Hizmetleri (DÃ¯Â¿Â½NAMÃ¯Â¿Â½K HALE GETÃ¯Â¿Â½RÃ¯Â¿Â½LDÃ¯Â¿Â½)
        public IActionResult HemsirelikHizmetleri()
        {
            // Aktif olan tÃ¯Â¿Â½m iÃ¯Â¿Â½erikleri sÃ¯Â¿Â½rasÃ¯Â¿Â½na gÃ¯Â¿Â½re tek seferde Ã¯Â¿Â½ekiyoruz
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
                // Ayarlar tablosundan ilk kaydÃ¯Â¿Â½ al, yoksa boÃ¯Â¿Â½ bir nesne gÃ¯Â¿Â½nder (hata vermemesi iÃ¯Â¿Â½in)
                Ayarlar = _context.HemsirelikAyarlar.FirstOrDefault() ?? new HemsirelikAyar(),
                Sekmeler = sekmeler,

                // Tek tabloyu Kategori sÃ¼tununa gÃ¶re View'daki ilgili listelere paylaÅŸtÄ±rÄ±yoruz
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
            ViewData["Title"] = "Bilgi İşlem Merkezi";
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
            ViewData["Title"] = "İş Sağlığı ve Güvenliği";
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
            ViewData["Title"] = "Enfeksiyon Kontrol Komitesi";
            return View("Index", icerikler);
        }

        // Eczacılık Hizmetleri
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
            ViewData["Title"] = "Eczacılık Hizmetleri";
            return View("Index", icerikler);
        }

        // Satın Alma
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
            ViewData["Title"] = "Satın Alma Birimi";
            return View("Index", icerikler);
        }

        // İstatistik ve Raporlama
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
            ViewData["Title"] = "İstatistik ve Raporlama Birimi";
            return View("Index", icerikler);
        }

        // Arşiv Birimi
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
            ViewData["Title"] = "Arşiv Birimi";
            return View("Index", icerikler);
        }

        // Hasta İletişim Birimi
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
            ViewData["Title"] = "Hasta İletişim Birimi";
            return View("Index", icerikler);
        }

        // İş Akış Şemaları
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
            ViewData["Title"] = "İş Akış Şemaları";
            return View("Index", icerikler);
        }

        // Organizasyon Şemaları
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
            ViewData["Title"] = "Organizasyon Şemaları";
            return View("Index", icerikler);
        }

        // İç Kontrol
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
            ViewData["Title"] = "İç Kontrol";
            return View("Index", icerikler);
        }

        // BasÃ„Â±n ve Kurumsal Ã„Â°letiÃ…Å¸im
        public async Task<IActionResult> BasinIletisim()
        {
            var model = await _context.BasinKurumsalIletisimler.FirstOrDefaultAsync();
            if (model == null)
            {
                // VarsayÃ„Â±lan deÃ„Å¸erler
                model = new BasinKurumsalIletisim
                {
                    Baslik = "BasÃ„Â±n ve Kurumsal Ã„Â°letiÃ…Å¸im Birimi",
                    Aciklama = "SaÃ„Å¸lÃ„Â±k AraÃ…Å¸tÃ„Â±rma ve Uygulama Merkezimiz faaliyetleri ÃƒÂ§erÃƒÂ§evesinde; hedef kitlelerle etkili bir iletiÃ…Å¸im kurmak ve sunulan saÃ„Å¸lÃ„Â±k hizmetinin yanÃ„Â± sÃ„Â±ra hastanemizin gerÃƒÂ§ekleÃ…Å¸tirdiÃ„Å¸i yeniliklerden hem personelimizi ve hem de dÃ„Â±Ã…Å¸ paydaÃ…Å¸larÃ„Â± haberdar etmek amacÃ„Â±yla hastanemiz web sayfasÃ„Â±na ve kurum iÃƒÂ§i SMS faaliyetlerine yÃƒÂ¶nelik sÃƒÂ¼reÃƒÂ§lerin takibi ve koordinasyonunun saÃ„Å¸lanmasÃ„Â± adÃ„Â±na BaÃ…Å¸hekimlik makamÃ„Â±nÃ„Â±n 22.07.2025 tarihli Oluru doÃ„Å¸rultusunda \"BasÃ„Â±n ve Kurumsal Ã„Â°letiÃ…Å¸im Birimi\"miz kurulmuÃ…Å¸tur.",
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
