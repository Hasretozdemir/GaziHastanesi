using System.ComponentModel.DataAnnotations;

namespace GaziHastane.Models
{
    public class HemsirelikSekme
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Baslik { get; set; }

        [Required]
        public string SekmeId { get; set; }

        public string IconClass { get; set; }

        public int Sira { get; set; }

        public bool AktifMi { get; set; } = true;
    }
}
