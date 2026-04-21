using System;
using System.Collections.Generic;
using System.Linq;

namespace GaziHastane.Models
{
    public class OdemeMakbuzViewModel
    {
        public int MakbuzId { get; set; }
        public string HastaAdSoyad { get; set; }
        public string TcKimlik { get; set; }
        public string MakbuzNo { get; set; }
        public DateTime IslemTarihi { get; set; }
        public string OdemeYontemi { get; set; }
        public string IslemRef { get; set; }
        public List<OdemeIslemKalemi> Kalemler { get; set; } = new List<OdemeIslemKalemi>();
        
        // Hesaplanan alanlar
        public decimal ToplamTutar => Kalemler.Sum(k => k.Tutar);
        public decimal KdvTutar => ToplamTutar * 0.10m; // Saðlýk KDV'si %10
        public decimal GenelToplam => ToplamTutar + KdvTutar;
        
        // Kasiyer bilgisi
        public string KasiyerAdi { get; set; } = "Ayþe Yýlmaz";
        public string KasiyerBirim { get; set; } = "Gazi Hastanesi Tahsilat Birimi";
    }

    public class OdemeIslemKalemi
    {
        public int Id { get; set; }
        public string IslemAdi { get; set; }
        public int Adet { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal Tutar => Adet * BirimFiyat;
    }
}
