using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GaziHastane.Models
{
    [Table("KurumsalSayfalar")]
    public class KurumsalSayfa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Baslik { get; set; } = string.Empty;

        public string? Icerik { get; set; }

        [StringLength(255)]
        public string? FotografUrl { get; set; }

        [StringLength(150)]
        public string? Unvan { get; set; }

        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(50)]
        public string? Telefon { get; set; }

        public bool AktifMi { get; set; } = true;

        public DateTime GuncellemeZamani { get; set; } = DateTime.UtcNow;
    }
}
