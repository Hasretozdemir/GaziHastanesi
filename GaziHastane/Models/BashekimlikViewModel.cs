using System.Collections.Generic;

namespace GaziHastane.Models
{
    public class BashekimlikViewModel
    {
        public BashekimlikPersonel? Bashekim { get; set; }
        public List<BashekimlikPersonel> Yardimcilar { get; set; } = new List<BashekimlikPersonel>();

        // İletişim bilgileri (İstersen bunları da veritabanında Ayarlar tablosundan çekebilirsin, şimdilik Controller'dan göndereceğiz)
        public string? Telefon { get; set; }
        public string? CalismaSaatleri { get; set; }
    }
}