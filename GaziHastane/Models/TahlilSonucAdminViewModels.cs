using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GaziHastane.Models
{
    public class TahlilSonucGirisViewModel
    {
        [Required(ErrorMessage = "T.C. Kimlik No zorunludur.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "T.C. Kimlik No 11 haneli olmalýdýr.")]
        public string TCKimlikNo { get; set; } = string.Empty;

        [StringLength(100)]
        public string? TestKategorisi { get; set; }

        [Required(ErrorMessage = "Test adý zorunludur.")]
        [StringLength(150)]
        public string TestAdi { get; set; } = string.Empty;

        [StringLength(50)]
        public string? SonucDegeri { get; set; }

        [StringLength(50)]
        public string? ReferansAraligi { get; set; }

        [StringLength(255)]
        public string? RaporDosyaUrl { get; set; }

        [Display(Name = "Rapor PDF Dosyasý")]
        public IFormFile? RaporDosya { get; set; }

        [Display(Name = "Tarih")]
        public DateTime Tarih { get; set; } = DateTime.Today;

        [Display(Name = "Doktor")]
        public int? DoktorId { get; set; }
    }

    public class TahlilSonucSorguViewModel
    {
        [StringLength(11)]
        public string? TCKimlikNo { get; set; }

        public User? Hasta { get; set; }

        public List<TahlilSonuc> Sonuclar { get; set; } = new();
    }
}
