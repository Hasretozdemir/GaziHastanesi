using System.ComponentModel.DataAnnotations;

namespace GaziHastane.Models
{
    public class AdminLog
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string? KullaniciAdi { get; set; }

        [MaxLength(50)]
        public string IslemTipi { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Modul { get; set; } = string.Empty;

        public string Aciklama { get; set; } = string.Empty;

        public DateTime Tarih { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string? IpAdresi { get; set; }
    }
}
