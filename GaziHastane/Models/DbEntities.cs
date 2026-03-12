using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GaziHastane.Models
{
    // 1. Users Table
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(11)]
        public string TCKimlikNo { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Ad { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Soyad { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime DogumTarihi { get; set; }

        [StringLength(20)]
        public string? Telefon { get; set; }

        [StringLength(150)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(255)]
        public string SifreHash { get; set; } = null!;

        [StringLength(10)]
        public string? Cinsiyet { get; set; }

        /// <summary>
        /// 1: Hasta, 2: Doktor, 3: Admin
        /// </summary>
        public short KullaniciTipi { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
        public virtual ICollection<TahlilSonuc> TahlilSonuclari { get; set; } = new List<TahlilSonuc>();
        public virtual ICollection<BorcOdeme> BorclarOdemeler { get; set; } = new List<BorcOdeme>();
    }

    // 2. Bolumler Table
    [Table("bolumler")]
    public class Bolum
    {
       
            [Key]
            [Column("id")]
            public int Id { get; set; }

            [Required]
            [StringLength(150)]
            [Column("ad")]
            public string Ad { get; set; } = null!;

            [Column("aciklama")]
            public string? Aciklama { get; set; }

            [Column("fotografurl")]
            public string? FotografUrl { get; set; }

            [Column("kategori")]
            public string? Kategori { get; set; }

            [Column("isactive")]
            public bool IsActive { get; set; } = true;
        

        // Navigation properties
        public virtual ICollection<Doktor> Doktorlar { get; set; } = new List<Doktor>();
        public virtual ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
    }

 
        // 3. Doktorlar Table
        [Table("doktorlar")]
        public class Doktor
        {
            [Key]
            [Column("id")]
            public int Id { get; set; }

            [Column("kullaniciid")]
            public int? KullaniciId { get; set; }
            [ForeignKey("KullaniciId")]
            public virtual User? Kullanici { get; set; }

            [Column("bolumid")]
            public int? BolumId { get; set; }
            [ForeignKey("BolumId")]
            public virtual Bolum? Bolum { get; set; }

            [StringLength(50)]
            [Column("unvan")]
            public string? Unvan { get; set; }

            [Required]
            [StringLength(100)]
            [Column("ad")]
            public string Ad { get; set; } = null!;

            [Required]
            [StringLength(100)]
            [Column("soyad")]
            public string Soyad { get; set; } = null!;

            [StringLength(150)]
            [Column("uzmanlikalani")]
            public string? UzmanlikAlani { get; set; }

            [Column("ozgecmis")]
            public string? Ozgecmis { get; set; }

            [StringLength(255)]
            [Column("fotografurl")]
            public string? FotografUrl { get; set; }

            [Column("isactive")]
            public bool IsActive { get; set; } = true;

            // Navigation properties
            public virtual ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
            public virtual ICollection<TahlilSonuc> TahlilSonuclari { get; set; } = new List<TahlilSonuc>();
        }
        // 4. Randevular Table
        [Table("Randevular")]
    public class Randevu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? HastaId { get; set; }
        [ForeignKey("HastaId")]
        public virtual User? Hasta { get; set; }

        public int? DoktorId { get; set; }
        [ForeignKey("DoktorId")]
        public virtual Doktor? Doktor { get; set; }

        public int? BolumId { get; set; }
        [ForeignKey("BolumId")]
        public virtual Bolum? Bolum { get; set; }

        public DateTime RandevuTarihi { get; set; }

        /// <summary>
        /// 1: Bekliyor, 2: Onaylandi, 3: Iptal Edildi, 4: Tamamlandi
        /// </summary>
        public short Durum { get; set; }

        public string? Sikayet { get; set; }

        public DateTime OlusturulmaTarihi { get; set; } = DateTime.UtcNow;
    }

    // 5. TahlilSonuclari Table
    [Table("TahlilSonuclari")]
    public class TahlilSonuc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? HastaId { get; set; }
        [ForeignKey("HastaId")]
        public virtual User? Hasta { get; set; }

        public int? DoktorId { get; set; }
        [ForeignKey("DoktorId")]
        public virtual Doktor? Doktor { get; set; }

        public DateTime Tarih { get; set; }

        [StringLength(100)]
        public string? TestKategorisi { get; set; }

        [Required]
        [StringLength(150)]
        public string TestAdi { get; set; } = null!;

        [StringLength(50)]
        public string? SonucDegeri { get; set; }

        [StringLength(50)]
        public string? ReferansAraligi { get; set; }

        [StringLength(255)]
        public string? RaporDosyaUrl { get; set; }
    }

    // 6. BorclarOdemeler Table
    [Table("BorclarOdemeler")]
    public class BorcOdeme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? HastaId { get; set; }
        [ForeignKey("HastaId")]
        public virtual User? Hasta { get; set; }

        [Required]
        [StringLength(150)]
        public string IslemTipi { get; set; } = null!;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Tutar { get; set; }

        public bool OdendiMi { get; set; } = false;

        [DataType(DataType.Date)]
        public DateTime? SonOdemeTarihi { get; set; }

        public DateTime? OdemeTarihi { get; set; }

        [StringLength(100)]
        public string? DekontNo { get; set; }
    }

    // 7. YemekListesi Table
    [Table("YemekListesi")]
    public class YemekListesi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Tarih { get; set; }

        /// <summary>
        /// 1: Sabah, 2: Öðle, 3: Akþam
        /// </summary>
        public short Ogun { get; set; } 

        [StringLength(150)]
        public string? Corba { get; set; }

        [StringLength(150)]
        public string? AnaYemek { get; set; }

        [StringLength(150)]
        public string? YardimciYemek { get; set; }

        [StringLength(150)]
        public string? TatliMeyve { get; set; }

        public int? ToplamKalori { get; set; }
    }

    // 8. Duyurular Table
    [Table("Duyurular")]
    public class Duyuru
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Baslik { get; set; } = null!;

        [Required]
        public string Icerik { get; set; } = null!;

        [StringLength(255)]
        public string? GorselUrl { get; set; }

        public DateTime YayinTarihi { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }

    // 9. KaliteBelgeleri Table
    [Table("KaliteBelgeleri")]
    public class KaliteBelgesi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string BelgeAdi { get; set; } = null!;

        [StringLength(100)]
        public string? Kategori { get; set; }

        [Required]
        [StringLength(255)]
        public string DosyaUrl { get; set; } = null!;

        public DateTime YayinTarihi { get; set; } = DateTime.UtcNow;
    }

    // 10. EgitimKomitesi Table
    [Table("EgitimKomitesi")]
    public class EgitimKomitesiUye
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string UyeAdSoyad { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string Gorev { get; set; } = null!;

        // Ekran görüntüsündeki "Unvaný" sütunu için bu alaný ekliyoruz
        [StringLength(150)]
        public string? Unvan { get; set; }

        [StringLength(255)]
        public string? FotografUrl { get; set; }
    }


}
