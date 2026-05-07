using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GaziHastane.Models
{
    [Table("KurumsalIcerikler")]
    public class KurumsalIcerik
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string SayfaKey { get; set; } = null!; // BilgiIslemMerkezi, EnfeksiyonKontrol, vb.

        [Required]
        [StringLength(100)]
        public string Kategori { get; set; } = null!; // KurumsalSekme.SekmeId ile e�le�ecek

        [Required]
        [StringLength(20)]
        public string IcerikTipi { get; set; } = "Form"; // Form, Video, Sayfa

        public string? Baslik { get; set; } 
        public string? AltBaslik { get; set; } 
        public string? Aciklama { get; set; } 
        public string? MedyaYolu { get; set; } 
        public string? VideoUrl { get; set; }

        public int Sira { get; set; } = 99;
        public bool AktifMi { get; set; } = true;
    }
}
