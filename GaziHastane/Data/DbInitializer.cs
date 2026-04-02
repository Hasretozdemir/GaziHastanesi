using GaziHastane.Models;
using System;
using System.Linq;

namespace GaziHastane.Data
{
    public static class DbInitializer
    {
        public static void Initialize(GaziHastaneContext context)
        {
            // Veritabanýnýn oluþturulduðundan emin ol (Eðer migration yapýlmadýysa bunu oluþturur)
            context.Database.EnsureCreated();

            // 1. YEMEK LÝSTESÝ KONTROLÜ VE EKLEME
            if (!context.YemekListesi.Any())
            {
                var bugun = DateTime.Today;

                var yemekler = new YemekListesi[]
                {
                    // Bugünün Yemekleri
                    new YemekListesi { Tarih = bugun, Ogun = 1, Corba = "Çay", AnaYemek = "Haþlanmýþ Yumurta", YardimciYemek = "Beyaz Peynir", TatliMeyve = "Siyah Zeytin", ToplamKalori = 450 },
                    new YemekListesi { Tarih = bugun, Ogun = 2, Corba = "Mercimek Çorbasý", AnaYemek = "Ýzmir Köfte", YardimciYemek = "Pirinç Pilavý", TatliMeyve = "Kemalpaþa Tatlýsý", ToplamKalori = 850 },
                    new YemekListesi { Tarih = bugun, Ogun = 3, Corba = "Ezogelin Çorbasý", AnaYemek = "Taze Fasulye", YardimciYemek = "Bulgur Pilavý", TatliMeyve = "Mevsim Meyvesi", ToplamKalori = 700 },

                    // Yarýnýn Yemekleri
                    new YemekListesi { Tarih = bugun.AddDays(1), Ogun = 1, Corba = "Süt", AnaYemek = "Omlet", YardimciYemek = "Kaþar Peyniri", TatliMeyve = "Bal-Tereyað", ToplamKalori = 500 },
                    new YemekListesi { Tarih = bugun.AddDays(1), Ogun = 2, Corba = "Tarhana Çorbasý", AnaYemek = "Piliç Topkapý", YardimciYemek = "Meyhane Pilavý", TatliMeyve = "Sütlaç", ToplamKalori = 800 },
                    new YemekListesi { Tarih = bugun.AddDays(1), Ogun = 3, Corba = "Yayla Çorbasý", AnaYemek = "Karnýyarýk", YardimciYemek = "Cacýk", TatliMeyve = "Elma", ToplamKalori = 750 },
                };

                // Verileri veritabaný kontekstine ekle
                context.YemekListesi.AddRange(yemekler);
                context.SaveChanges();
            }

            // 2. BÖLÜM VE DOKTOR KONTROLÜ VE EKLEME
            if (!context.Bolumler.Any())
            {
                // ÖNEMLÝ: Arayüzde görünmesi için 'Kategori' alanlarý (Dahili, Cerrahi, Temel) eklendi!
                var bolumler = new Bolum[] {
                     new Bolum { Ad = "Kardiyoloji", Aciklama = "Kalp saðlýðý birimi", Kategori = "Dahili", IsActive = true },
                     new Bolum { Ad = "Genel Cerrahi", Aciklama = "Cerrahi müdahaleler", Kategori = "Cerrahi", IsActive = true },
                     new Bolum { Ad = "Anatomi", Aciklama = "Ýnsan anatomisi", Kategori = "Temel", IsActive = true },
                     new Bolum { Ad = "Göz Hastalýklarý", Aciklama = "Göz ve görme saðlýðý", Kategori = "Dahili", IsActive = true },
                     new Bolum { Ad = "Acil Týp", Aciklama = "7/24 Acil Servis", Kategori = "Dahili", IsActive = true }
                 };
                context.Bolumler.AddRange(bolumler);

                // Bölümleri önce kaydet ki ID'leri oluþsun (Doktor eklerken lazým olacak)
                context.SaveChanges();

                // Örnek Doktor Ekle
                var kardiyoloji = context.Bolumler.FirstOrDefault(b => b.Ad == "Kardiyoloji");
                if (kardiyoloji != null)
                {
                    context.Doktorlar.Add(new Doktor
                    {
                        Ad = "Hasret",
                        Soyad = "Özdemir",
                        Unvan = "Uzman Dr.",
                        BolumId = kardiyoloji.Id,
                        UzmanlikAlani = "Kalp Yetmezliði",
                        IsActive = true
                    });
                }

                context.SaveChanges();
            }

            // 3. YETKÝLÝLER (ADMIN) - Eðer yoksa bir yönetici ekle
            if (!context.Yetkililer.Any())
            {
                // DÝKKAT: SifreHash þu an düz metin olarak kullanýlýyor. Üretimde hash uygulayýn.
                var admin = new Yetkili
                {
                    AdSoyad = "Admin Kullanýcý",
                    Email = "admin@gazihastanesi.com",
                    SifreHash = "admin123",
                    Rol = "Yönetici",
                    IsActive = true,
                    KayitTarihi = DateTime.UtcNow
                };
                context.Yetkililer.Add(admin);
                context.SaveChanges();
            }

            // 4. KURUMSAL SIDEBAR MENÜ TOHUMLAMA
            if (!context.KurumsalMenuGruplar.Any())
            {
                var grupYonetim = new KurumsalMenuGrup
                {
                    GrupAdi = "Yönetim",
                    Sira = 1,
                    AktifMi = true
                };

                var grupKurumsal = new KurumsalMenuGrup
                {
                    GrupAdi = "Kurumsal",
                    Sira = 2,
                    AktifMi = true
                };

                context.KurumsalMenuGruplar.AddRange(grupYonetim, grupKurumsal);
                context.SaveChanges();

                var menuler = new KurumsalMenu[]
                {
                    new KurumsalMenu { GrupId = grupYonetim.Id, Baslik = "Baþhekimlik", Url = "/Kurumsal/Bashekimlik", IconClass = "fa-user-tie", Sira = 1, AktifMi = true },
                    new KurumsalMenu { GrupId = grupYonetim.Id, Baslik = "Baþmüdürlük", Url = "/Kurumsal/Basmudurluk", IconClass = "fa-users", Sira = 2, AktifMi = true },

                    new KurumsalMenu { GrupId = grupKurumsal.Id, Baslik = "Hakkýmýzda", Url = "/Kurumsal/Index", IconClass = "fa-circle-info", Sira = 1, AktifMi = true },
                    new KurumsalMenu { GrupId = grupKurumsal.Id, Baslik = "Hemþirelik Hizmetleri", Url = "/Kurumsal/HemsirelikHizmetleri", IconClass = "fa-user-nurse", Sira = 2, AktifMi = true },
                    new KurumsalMenu { GrupId = grupKurumsal.Id, Baslik = "Bilgi Ýþlem Merkezi", Url = "/Kurumsal/BilgiIslem", IconClass = "fa-microchip", Sira = 3, AktifMi = true },
                    new KurumsalMenu { GrupId = grupKurumsal.Id, Baslik = "Ýþ Saðlýðý ve Güvenliði", Url = "/Kurumsal/IsSagligi", IconClass = "fa-shield-halved", Sira = 4, AktifMi = true },
                    new KurumsalMenu { GrupId = grupKurumsal.Id, Baslik = "Enfeksiyon Kontrol", Url = "/Kurumsal/Enfeksiyon", IconClass = "fa-virus-slash", Sira = 5, AktifMi = true },
                    new KurumsalMenu { GrupId = grupKurumsal.Id, Baslik = "Eczacýlýk Hizmetleri", Url = "/Kurumsal/Eczacilik", IconClass = "fa-pills", Sira = 6, AktifMi = true },
                    new KurumsalMenu { GrupId = grupKurumsal.Id, Baslik = "Satýn Alma", Url = "/Kurumsal/SatinAlma", IconClass = "fa-cart-shopping", Sira = 7, AktifMi = true },
                    new KurumsalMenu { GrupId = grupKurumsal.Id, Baslik = "Ýstatistik ve Raporlama", Url = "/Kurumsal/Istatistik", IconClass = "fa-chart-line", Sira = 8, AktifMi = true },
                    new KurumsalMenu { GrupId = grupKurumsal.Id, Baslik = "Arþiv Birimi", Url = "/Kurumsal/Arsiv", IconClass = "fa-box-archive", Sira = 9, AktifMi = true },
                    new KurumsalMenu { GrupId = grupKurumsal.Id, Baslik = "Hasta Ýletiþim Birimi", Url = "/Kurumsal/HastaIletisim", IconClass = "fa-comment-medical", Sira = 10, AktifMi = true },
                    new KurumsalMenu { GrupId = grupKurumsal.Id, Baslik = "Ýþ Akýþ Þemalarý", Url = "/Kurumsal/IsAkis", IconClass = "fa-diagram-project", Sira = 11, AktifMi = true },
                    new KurumsalMenu { GrupId = grupKurumsal.Id, Baslik = "Organizasyon Þemalarý", Url = "/Kurumsal/Organizasyon", IconClass = "fa-sitemap", Sira = 12, AktifMi = true },
                    new KurumsalMenu { GrupId = grupKurumsal.Id, Baslik = "Ýç Kontrol", Url = "/Kurumsal/IcKontrol", IconClass = "fa-check-double", Sira = 13, AktifMi = true },
                    new KurumsalMenu { GrupId = grupKurumsal.Id, Baslik = "Basýn ve Kurumsal Ýletiþim", Url = "/Kurumsal/BasinIletisim", IconClass = "fa-bullhorn", Sira = 14, AktifMi = true }
                };

                context.KurumsalMenuler.AddRange(menuler);
                context.SaveChanges();
            }
        }
    }
}