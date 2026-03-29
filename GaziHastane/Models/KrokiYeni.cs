using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GaziHastane.Models
{
    public class KrokiBlok
    {
        [Key]
        public int Id { get; set; }
        public string BlokAdi { get; set; }
        public string Renk { get; set; }
        public ICollection<KrokiKat> Katlar { get; set; }
    }

    public class KrokiKat
    {
        [Key]
        public int Id { get; set; }
        public string KatAdi { get; set; }
        public int BlokId { get; set; }
        public KrokiBlok Blok { get; set; }
        public ICollection<KrokiBolum> Bolumler { get; set; }
    }

    public class KrokiBolum
    {
        [Key]
        public int Id { get; set; }
        public string BirimAdi { get; set; }
        public int KatId { get; set; }
        public KrokiKat Kat { get; set; }
        public string Ikon { get; set; }
        public bool IsEmpty { get; set; }
    }
}
