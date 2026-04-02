using System.ComponentModel.DataAnnotations;

namespace GaziHastane.Models
{
    public class HemsirelikIcerik
    {
        [Key]
        public int Id { get; set; }

        // "Yonetim", "Gorev", "Mevzuat", "Galeri", "Etkinlik", "Akis" değerlerinden birini alacak
        [Required]
        public string Kategori { get; set; }

        public string Baslik { get; set; } // Personel Adı, Görev Adı veya Etkinlik Başlığı
        public string AltBaslik { get; set; } // Unvan, Tarih vb. için
        public string Aciklama { get; set; } // Görev detayı veya Etkinlik metni
        public string MedyaYolu { get; set; } // Fotoğraf veya PDF URL'si

        public int Sira { get; set; }
        public bool AktifMi { get; set; } = true;
    }
}