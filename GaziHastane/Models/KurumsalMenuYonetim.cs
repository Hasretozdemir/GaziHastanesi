using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GaziHastane.Models
{
    [Table("KurumsalMenuGruplar")]
    public class KurumsalMenuGrup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string GrupAdi { get; set; } = string.Empty;

        public int Sira { get; set; }

        public bool AktifMi { get; set; } = true;

        public virtual ICollection<KurumsalMenu> Menuler { get; set; } = new List<KurumsalMenu>();
    }

    [Table("KurumsalMenuler")]
    public class KurumsalMenu
    {
        [Key]
        public int Id { get; set; }

        public int GrupId { get; set; }

        [ForeignKey(nameof(GrupId))]
        public virtual KurumsalMenuGrup? Grup { get; set; }

        [Required]
        [StringLength(100)]
        public string Baslik { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Url { get; set; } = string.Empty;

        [StringLength(50)]
        public string? IconClass { get; set; }

        public int Sira { get; set; }

        public bool AktifMi { get; set; } = true;
    }
}
