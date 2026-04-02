using System.ComponentModel.DataAnnotations;

namespace GaziHastane.Models
{
    public class HemsirelikAyar
    {
        [Key]
        public int Id { get; set; }
        public string Misyon { get; set; }
        public string Vizyon { get; set; }
        public string OrganizasyonSemaUrl { get; set; }

        // İçimizden Biri (Ayın Hemşiresi)
        public string AyinHemsiresiAd { get; set; }
        public string AyinHemsiresiBirim { get; set; }
        public string AyinHemsiresiSoz { get; set; }
        public string AyinHemsiresiFotoUrl { get; set; }

        // İletişim
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Adres { get; set; }
    }

       
}