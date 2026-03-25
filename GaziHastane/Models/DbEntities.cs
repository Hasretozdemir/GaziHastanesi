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

    [Table("KaliteBelgeleri")]
    public class KaliteBelgesi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Belge veya sayfa baþlýðý zorunludur.")]
        [StringLength(255)]
        public string BelgeAdi { get; set; } = null!; // Kartýn Baþlýðý

        [StringLength(100)]
        public string? Kategori { get; set; } // Organizasyon Þemasý, Kalite Ekibi vb.

        // --- YAZI YAZMA KISMI ---
        // Uzun metinler veya HTML kodlarý (CKEditor içeriði gibi) burada tutulacak.
        public string? Aciklama { get; set; }

        // --- FOTOÐRAF EKLEME KISMI ---
        [StringLength(255)]
        public string? FotoUrl { get; set; }

        // --- BELGE EKLEME KISMI ---
        [Required]
        [StringLength(255)]
        public string DosyaUrl { get; set; } = null!; // PDF, Docx vb. dosya yolu

        public DateTime YayinTarihi { get; set; } = DateTime.UtcNow;
    }
    [Table("EgitimIcerikleri")]
    public class EgitimKarti
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kart baþlýðý zorunludur.")]
        [StringLength(150)]
        public string Baslik { get; set; } = null!; // Örn: Eðitim Komitesi

        [StringLength(255)]
        public string? KisaAciklama { get; set; } // Örn: Yürütme kurulu ve üyelerimiz.

        [StringLength(50)]
        public string? Ikon { get; set; } // Örn: fa-users-crown

        [StringLength(50)]
        public string? Renk { get; set; } // Örn: text-blue-400

        [StringLength(50)]
        public string? Tip { get; set; } // "Panel" (Modal açar) veya "Link" (Sayfaya gider)

        [StringLength(255)]
        public string? Hedef { get; set; } // Örn: /Egitim/Index veya #hakkimizdaPanel

        // --- ZENGÝN ÝÇERÝK BÖLÜMÜ (CKEditor) ---
        public string? Icerik { get; set; } // Modal içinde görünecek HTML metin

        // --- MEDYA VE DOSYA BÖLÜMÜ ---
        [StringLength(255)]
        public string? FotoUrl { get; set; } // Modal kapak fotoðrafý

        [StringLength(255)]
        public string? DosyaUrl { get; set; } = "#"; // Ýndirilebilir PDF/Doc dosyasý
    }



    // 11. HastaRehberi Table
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
        public string? Tema { get; set; }
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
        public string Ozet { get; set; } = null!;

        public string? Icerik { get; set; }

        [StringLength(255)]
        public string GorselUrl { get; set; } = null!;

        [StringLength(50)]
        public string Kategori { get; set; } = null!;

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
        public string EtkinlikTipi { get; set; } = null!;

        public DateTime Tarih { get; set; }

        [StringLength(50)]
        public string SaatAraligi { get; set; } = null!;

        [StringLength(200)]
        public string Konum { get; set; } = null!;

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
        public string SifreHash { get; set; } = null!;

        [StringLength(50)]
        public string Rol { get; set; } = "Yönetici";

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? SonGirisTarihi { get; set; }

        public bool IsActive { get; set; } = true;
    }

    // --- YENÝ EKLENEN TABLOLAR ---

    // 17. Belgeler Table (Genel Dosya Yönetimi)
    [Table("Belgeler")]
    public class Belge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Baslik { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public string DosyaYolu { get; set; } = null!; // wwwroot altýndaki konumu

        [StringLength(50)]
        public string? DosyaTipi { get; set; } // .pdf, .docx vb.

        public DateTime YuklenmeTarihi { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }

    // 18. Medya Table (Görsel ve Slider Yönetimi)
    [Table("Medya")]
    public class Medya
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        public string? Baslik { get; set; }

        [Required]
        [StringLength(500)]
        public string GorselYolu { get; set; } = null!;

        public bool IsSlider { get; set; } = false; // Slider'da görünüp görünmeyeceði

        public int SiraNo { get; set; } = 0; // Slider'daki görüntülenme sýrasý

        public DateTime YuklenmeTarihi { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}