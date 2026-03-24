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

        public short KullaniciTipi { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
        public virtual ICollection<TahlilSonuc> TahlilSonuclari { get; set; } = new List<TahlilSonuc>();
        public virtual ICollection<BorcOdeme> BorclarOdemeler { get; set; } = new List<BorcOdeme>();
    }

    // 2. Bolumler Table
    [Table("Bolumler")]
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

        public virtual ICollection<Doktor> Doktorlar { get; set; } = new List<Doktor>();
        public virtual ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
    }

    // 3. Doktorlar Table
    [Table("Doktorlar")]
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

        [StringLength(150)]
        public string? Unvan { get; set; }

        [StringLength(255)]
        public string? FotografUrl { get; set; }
    }

    [Table("HastaRehberi")]
    public class HastaRehberi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Baslik { get; set; } = null!;

        [Required]
        public string Icerik { get; set; } = null!;

        [StringLength(50)]
        public string? Ikon { get; set; }

        public int SiraNo { get; set; } = 1;

        public bool IsActive { get; set; } = true;

        [StringLength(20)]
        public string? Tema { get; set; } // Tasarýmdaki: blue, purple, red, teal, orange, violet, emerald, amber, rose
    }

    // 12. Iletisim Table
    [Table("Iletisim")]
    public class Iletisim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(150)]
        public string Baslik { get; set; } = null!;

        [StringLength(150)]
        public string? AltBaslik { get; set; }

        [StringLength(100)]
        public string? KisaAdres { get; set; }

        [StringLength(50)]
        public string? Koordinat { get; set; }

        public string Adres { get; set; } = null!;

        [StringLength(50)]
        public string? CagriMerkezi { get; set; }

        [StringLength(50)]
        public string? Santral { get; set; }

        [StringLength(255)]
        public string? DigerTelefonlar { get; set; }

        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(150)]
        public string? CalismaSaatleri { get; set; }

        [StringLength(255)]
        public string? EkBilgi { get; set; }

        public string? HaritaUrl { get; set; }

        [StringLength(50)]
        public string? TemaRengi { get; set; }

        public bool IsActive { get; set; } = true;
    }

    // 13. UlasimRehberi Table
    [Table("UlasimRehberi")]
    public class UlasimRehberi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string UlasimTipi { get; set; } = null!;

        [StringLength(50)]
        public string Ikon { get; set; } = null!;

        public string Icerik { get; set; } = null!;

        [StringLength(50)]
        public string TemaRengi { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }

    // 14. Haberler Table
    [Table("Haberler")]
    public class Haber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Baslik { get; set; } = null!;

        [StringLength(500)]
        public string Ozet { get; set; } = null!; // Kartlarda görünen kýsa yazý

        public string? Icerik { get; set; } // Týklanýnca açýlacak detay sayfasý için

        [StringLength(255)]
        public string GorselUrl { get; set; } = null!;

        [StringLength(50)]
        public string Kategori { get; set; } = null!; // Örn: "Týp Dünyasý", "Baþarýlarýmýz"

        public DateTime YayinTarihi { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }

    // 15. Etkinlikler Table
    [Table("Etkinlikler")]
    public class Etkinlik
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Baslik { get; set; } = null!;

        [StringLength(50)]
        public string EtkinlikTipi { get; set; } = null!; // Örn: "Sempozyum", "Eðitim", "Kongre"

        public DateTime Tarih { get; set; } // Gün ve ay bilgisini buradan alacaðýz

        [StringLength(50)]
        public string SaatAraligi { get; set; } = null!; // Örn: "09:00 - 17:00"

        [StringLength(200)]
        public string Konum { get; set; } = null!; // Örn: "Gazi Üniversitesi Ana Konferans Salonu"

        public string? Aciklama { get; set; }

        public bool IsActive { get; set; } = true;
    }
    // 16. Yetkililer Table
    [Table("Yetkililer")]
    public class Yetkili
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string AdSoyad { get; set; } = null!;

        [Required]
        [StringLength(150)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string SifreHash { get; set; } = null!; // Gerçek senaryoda þifrelenmiþ (MD5, SHA256 vb.) tutulur

        [StringLength(50)]
        public string Rol { get; set; } = "Yönetici"; // Örn: "Süper Admin", "Editör", "Yönetici"

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? SonGirisTarihi { get; set; } // Kullanýcý login oldukça güncellenecek alan

        public bool IsActive { get; set; } = true;
    }
}
