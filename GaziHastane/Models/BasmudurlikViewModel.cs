using System.Collections.Generic;

namespace GaziHastane.Models
{
    public class BasmudurlikViewModel
    {
        public BasmudurlikPersonel? Basmudur { get; set; }
        public List<BasmudurlikPersonel> Yardimcilar { get; set; } = new List<BasmudurlikPersonel>();
        public string Telefon { get; set; } = string.Empty;
        public string CalismaSaatleri { get; set; } = string.Empty;
    }
}
