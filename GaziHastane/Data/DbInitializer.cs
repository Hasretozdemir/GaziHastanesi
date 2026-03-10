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

            // Yemek Listesi kontrolü: Eðer veritabanýnda yemek varsa ekleme yapma
            if (context.YemekListesi.Any())
            {
                return;   // Veritabaný zaten dolu
            }

            var bugun = DateTime.Today;

            // Örnek Yemek Verileri (Eskiden kod içinde olanlar buraya taþýnýyor)
            var yemekler = new YemekListesi[]
            {
                // Bugünün Kahvaltýsý
                new YemekListesi 
                { 
                    Tarih = bugun, 
                    Ogun = 1, 
                    Corba = "Çay", 
                    AnaYemek = "Haþlanmýþ Yumurta", 
                    YardimciYemek = "Beyaz Peynir", 
                    TatliMeyve = "Siyah Zeytin", 
                    ToplamKalori = 450 
                },
                // Bugünün Öðle Yemeði
                new YemekListesi 
                { 
                    Tarih = bugun, 
                    Ogun = 2, 
                    Corba = "Mercimek Çorbasý", 
                    AnaYemek = "Ýzmir Köfte", 
                    YardimciYemek = "Pirinç Pilavý", 
                    TatliMeyve = "Kemalpaþa Tatlýsý", 
                    ToplamKalori = 850 
                },
                // Bugünün Akþam Yemeði
                new YemekListesi 
                { 
                    Tarih = bugun, 
                    Ogun = 3, 
                    Corba = "Ezogelin Çorbasý", 
                    AnaYemek = "Taze Fasulye", 
                    YardimciYemek = "Bulgur Pilavý", 
                    TatliMeyve = "Mevsim Meyvesi", 
                    ToplamKalori = 700 
                },

                // Yarýnýn Yemekleri (Örnek)
                new YemekListesi { Tarih = bugun.AddDays(1), Ogun = 1, Corba = "Süt", AnaYemek = "Omlet", YardimciYemek = "Kaþar Peyniri", TatliMeyve = "Bal-Tereyað", ToplamKalori = 500 },
                new YemekListesi { Tarih = bugun.AddDays(1), Ogun = 2, Corba = "Tarhana Çorbasý", AnaYemek = "Piliç Topkapý", YardimciYemek = "Meyhane Pilavý", TatliMeyve = "Sütlaç", ToplamKalori = 800 },
                new YemekListesi { Tarih = bugun.AddDays(1), Ogun = 3, Corba = "Yayla Çorbasý", AnaYemek = "Karnýyarýk", YardimciYemek = "Cacýk", TatliMeyve = "Elma", ToplamKalori = 750 },
            };

            // Verileri veritabaný kontekstine ekle
            foreach (var y in yemekler)
            {
                context.YemekListesi.Add(y);
            }
            
            // Eðer hiç Bölüm yoksa örnek bölümler ekle
            if (!context.Bolumler.Any()) 
            {
                 var bolumler = new Bolum[] {
                     new Bolum { Ad = "Kardiyoloji", Aciklama = "Kalp saðlýðý birimi", IsActive = true },
                     new Bolum { Ad = "Göz Hastalýklarý", Aciklama = "Göz ve görme saðlýðý", IsActive = true },
                     new Bolum { Ad = "Acil Týp", Aciklama = "7/24 Acil Servis", IsActive = true }
                 };
                 context.Bolumler.AddRange(bolumler);
                 
                 // Bölümleri önce kaydet ki ID'leri oluþsun (Doktor eklerken lazým olacak)
                 context.SaveChanges(); 

                 // Örnek Doktor Ekle
                 var kardiyoloji = context.Bolumler.FirstOrDefault(b => b.Ad == "Kardiyoloji");
                 if (kardiyoloji != null)
                 {
                    context.Doktorlar.Add(new Doktor {
                        Ad = "Hasret",
                        Soyad = "Özdemir",
                        Unvan = "Uzman Dr.",
                        BolumId = kardiyoloji.Id,
                        UzmanlikAlani = "Kalp Yetmezliði",
                        IsActive = true
                    });
                 }
            }

            // Tüm deðiþiklikleri veritabanýna kaydet
            context.SaveChanges();
        }
    }
}
