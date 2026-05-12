using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GaziHastane.Models
{
    // IcerikTipi enum'u başka bir dosyada yoksa burada kalabilir.
    // Eğer varsa, projendeki mevcut olanı kullanmalısın.
    public enum BilgiIslemIcerikTipi
    {
        Metin = 1,
        Video = 2,
        Belge = 3
    }

    public class BilgiIslemMerkeziSekme
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Sekme Başlığı")]
        public string Baslik { get; set; }

        public int Sira { get; set; }
        public bool IsActive { get; set; } = true;

        // İlişki
        public List<BilgiIslemMerkeziIcerik> Icerikler { get; set; }
    }

    public class BilgiIslemMerkeziIcerik
    {
        [Key]
        public int Id { get; set; }

        public int SekmeId { get; set; }
        public BilgiIslemMerkeziSekme Sekme { get; set; }

        [Required]
        public BilgiIslemIcerikTipi Tipi { get; set; }

        public string? Baslik { get; set; }

        public string? MetinIcerik { get; set; }
        public string? DosyaYolu { get; set; }
        public string? VideoUrl { get; set; }

        public int Sira { get; set; }
        public bool IsActive { get; set; } = true;
    }
}