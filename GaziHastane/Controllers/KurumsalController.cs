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

        // Hakk�m�zda (Kurumsal Ana Sayfa)
        public IActionResult Index() { return View(); }

        // Ba�hekimlik
        public IActionResult Bashekimlik()
        {
            // Veritaban�ndan aktif personelleri s�ras�na g�re �ekiyoruz
            var aktifPersoneller = _context.BashekimlikPersoneller
                                           .Where(x => x.AktifMi)
                                           .OrderBy(x => x.Sira)
                                           .ToList();

            // Verileri ViewModel'e dolduruyoruz
            var viewModel = new BashekimlikViewModel
            {
                // IsBashekim = true olan �LK kayd� Ba�hekim olarak al
                Bashekim = aktifPersoneller.FirstOrDefault(x => x.IsBashekim),

                // IsBashekim = false olanlar� Yard�mc�lar listesine al
                Yardimcilar = aktifPersoneller.Where(x => !x.IsBashekim).ToList(),

                // �leti�im bilgilerini burada tan�ml�yoruz
                Telefon = "(0312) 202 40 00",
                CalismaSaatleri = "Pzt�Cuma � 08:30 � 17:00"
            };

            return View(viewModel);
        }

        // Ba�m�d�rl�k
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
                CalismaSaatleri = "Pzt�Cuma � 08:30 � 17:00"
            };

            return View(viewModel);
        }

        // Hem�irelik Hizmetleri (D�NAM�K HALE GET�R�LD�)
        public IActionResult HemsirelikHizmetleri()
        {
            // Aktif olan t�m i�erikleri s�ras�na g�re tek seferde �ekiyoruz
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
                // Ayarlar tablosundan ilk kayd� al, yoksa bo� bir nesne g�nder (hata vermemesi i�in)
                Ayarlar = _context.HemsirelikAyarlar.FirstOrDefault() ?? new HemsirelikAyar(),
                Sekmeler = sekmeler,

                // Tek tabloyu Kategori s�tununa g�re View'daki ilgili listelere payla�t�r�yoruz
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

        // Bilgi ��lem Merkezi
        public IActionResult BilgiIslem() { return View(); }

        // �� Sa�l��� ve G�venli�i
        public IActionResult IsSagligi() { return View(); }

        // Enfeksiyon Kontrol
        public IActionResult Enfeksiyon() { return View(); }

        // Eczac�l�k Hizmetleri
        public IActionResult Eczacilik() { return View(); }

        // Sat�n Alma
        public IActionResult SatinAlma() { return View(); }

        // �statistik ve Raporlama
        public IActionResult Istatistik() { return View(); }

        // Ar�iv Birimi
        public IActionResult Arsiv()
        {
            // Veritaban�ndan aktif sekmeleri s�ras�na g�re �ekiyoruz
            var sekmeler = _context.ArsivSekmeler
                                   .Where(x => x.IsActive)
                                   .OrderBy(x => x.SiraNo)
                                   .ToList();

            return View(sekmeler);
        }

        // Hasta �leti�im Birimi
        public IActionResult HastaIletisim() { return View(); }

        // �� Ak�� �emalar�
        public IActionResult IsAkis() { return View(); }

        // Organizasyon �emalar�
        public async Task<IActionResult> Organizasyon()
        {
            var sekmeler = await _context.KurumsalIcerikler
                .Where(x => x.SayfaKey == "OrganizasyonSemalari" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            return View(sekmeler);
        }

        // �� Kontrol
        public async Task<IActionResult> IcKontrol()
        {
            var sekmeler = await _context.KurumsalSekmeler
                .Where(x => x.SayfaKey == "IcKontrol" && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();
            return View(sekmeler);
        }

        // Bas�n ve Kurumsal �leti�im
        public async Task<IActionResult> BasinIletisim()
        {
            var model = await _context.BasinKurumsalIletisimler.FirstOrDefaultAsync();
            if (model == null)
            {
                // Varsayılan değerler
                model = new BasinKurumsalIletisim
                {
                    Baslik = "Basın ve Kurumsal İletişim Birimi",
                    Aciklama = "Sağlık Araştırma ve Uygulama Merkezimiz faaliyetleri çerçevesinde; hedef kitlelerle etkili bir iletişim kurmak ve sunulan sağlık hizmetinin yanı sıra hastanemizin gerçekleştirdiği yeniliklerden hem personelimizi ve hem de dış paydaşları haberdar etmek amacıyla hastanemiz web sayfasına ve kurum içi SMS faaliyetlerine yönelik süreçlerin takibi ve koordinasyonunun sağlanması adına Başhekimlik makamının 22.07.2025 tarihli Oluru doğrultusunda \"Basın ve Kurumsal İletişim Birimi\"miz kurulmuştur.",
                    Telefon = "0312 202 44 39",
                    Email = "gazihastanesibasin@gazi.edu.tr",
                    Lokasyon = "E Blok 11. Kat"
                };
            }
            return View(model);
        }
    }
}
