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

        [Column("blok")]
        [StringLength(50)]
        public string? Blok { get; set; }

        [Column("kat")]
        [StringLength(50)]
        public string? Kat { get; set; }

        [Column("isactive")]
        public bool IsActive { get; set; } = true;

        [NotMapped]
        public IFormFile? GorselDosya { get; set; }

        public virtual ICollection<Doktor> Doktorlar { get; set; } = new List<Doktor>();
        public virtual ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
    }

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

        [Column("hekimtipi")]
        public short HekimTipi { get; set; }

        [Column("oda_konumu")]
        [StringLength(100)]
        public string? OdaKonumu { get; set; }

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

        [NotMapped]
        public IFormFile? GorselDosya { get; set; }

        public virtual ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
        public virtual ICollection<TahlilSonuc> TahlilSonuclari { get; set; } = new List<TahlilSonuc>();
    }

    [Table("Randevular")]
    public class Randevu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("hastaid")]
        public int? HastaId { get; set; }
        [ForeignKey("HastaId")]
        public virtual User? Hasta { get; set; }

        [Column("doktorid")]
        public int? DoktorId { get; set; }
        [ForeignKey("DoktorId")]
        public virtual Doktor? Doktor { get; set; }

        [Column("bolumid")]
        public int? BolumId { get; set; }
        [ForeignKey("BolumId")]
        public virtual Bolum? Bolum { get; set; }

        [Column("randevutarihi")]
        public DateTime RandevuTarihi { get; set; }

        [Column("durum")]
        public short Durum { get; set; }

        [Column("randevutipi")]
        public short RandevuTipi { get; set; } = 1;

        [Column("sikayet")]
        public string? Sikayet { get; set; }

        [Column("olusturulmatarihi")]
        public DateTime OlusturulmaTarihi { get; set; } = DateTime.UtcNow;
    }

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

        [StringLength(30)]
        public string? ProtakolNo { get; set; }

        public DateTime? EklenmeTarihi { get; set; }
    }

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

        [Required]
        [StringLength(255)]
        public string BelgeAdi { get; set; } = null!;

        [StringLength(100)]
        public string? Kategori { get; set; }

        public string? Aciklama { get; set; }

        [StringLength(255)]
        public string? FotoUrl { get; set; }

        [Required]
        [StringLength(255)]
        public string DosyaUrl { get; set; } = null!;

        public DateTime YayinTarihi { get; set; } = DateTime.UtcNow;
    }

    [Table("EgitimIcerikleri")]
    public class EgitimKarti
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Baslik { get; set; } = null!;

        [StringLength(255)]
        public string? KisaAciklama { get; set; }

        [StringLength(50)]
        public string? Ikon { get; set; }

        [StringLength(50)]
        public string? Renk { get; set; }

        [StringLength(50)]
        public string? Tip { get; set; }

        [StringLength(255)]
        public string? Hedef { get; set; }

        public string? Icerik { get; set; }

        [StringLength(255)]
        public string? FotoUrl { get; set; }

        [StringLength(255)]
        public string? DosyaUrl { get; set; } = "#";
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

        public string? ModalIcerik { get; set; }

        [StringLength(20)]
        public string AcilisTipi { get; set; } = "Modal";

        [StringLength(500)]
        public string? HedefUrl { get; set; }

        [StringLength(50)]
        public string? Ikon { get; set; }

        public int SiraNo { get; set; } = 1;

        public bool IsActive { get; set; } = true;

        [StringLength(20)]
        public string? Tema { get; set; }
    }

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

        public string? ModalIcerik { get; set; }

        [StringLength(255)]
        public string? GorselUrl { get; set; }

        [NotMapped]
        public IFormFile? GorselDosya { get; set; }

        public bool IsActive { get; set; } = true;
    }

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

        [StringLength(2000)]
        public string? AdminSayfaYetkileri { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? SonGirisTarihi { get; set; }

        public bool IsActive { get; set; } = true;

        [NotMapped]
        public List<string> SecilenSayfaYetkileri { get; set; } = new();
    }

    [Table("Medya")]
    public class Medya
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        public string? Baslik { get; set; }

        [StringLength(50)]
        public string Alan { get; set; } = "Genel";

        [Required]
        [StringLength(500)]
        public string GorselYolu { get; set; } = null!;

        [StringLength(500)]
        public string? HedefUrl { get; set; }

        public bool IsSlider { get; set; } = false;

        public int SiraNo { get; set; } = 0;

        public DateTime YuklenmeTarihi { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }

    [Table("KrokiBirimleri")]
    public class KrokiBirim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string KatAdi { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Baslik { get; set; } = null!;

        [StringLength(500)]
        public string? Aciklama { get; set; }

        [StringLength(50)]
        public string? Ikon { get; set; }

        [Required]
        [StringLength(20)]
        public string GridColumn { get; set; } = "1 / 2";

        [Required]
        [StringLength(20)]
        public string GridRow { get; set; } = "1 / 2";

        [StringLength(50)]
        public string TipSinifi { get; set; } = "room";

        public int? BolumId { get; set; }
        [ForeignKey("BolumId")]
        public virtual Bolum? Bolum { get; set; }
    }

    public class BashekimlikPersonel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string AdSoyad { get; set; } = string.Empty;

        [Required]
        public string Unvan { get; set; } = string.Empty;

        public bool IsBashekim { get; set; }

        public string UzmanlikAlani { get; set; } = string.Empty;

        public string KurumBilgisi { get; set; } = string.Empty;

        public string FotografYolu { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string CvYolu { get; set; } = string.Empty;

        public int Sira { get; set; }

        public bool AktifMi { get; set; } = true;
    }

    public class BasmudurlikPersonel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string AdSoyad { get; set; } = string.Empty;

        [Required]
        public string Unvan { get; set; } = string.Empty;

        public bool IsBasmudur { get; set; }

        public string UzmanlikAlani { get; set; } = string.Empty;

        public string KurumBilgisi { get; set; } = string.Empty;

        public string FotografYolu { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string CvYolu { get; set; } = string.Empty;

        public int Sira { get; set; }

        public bool AktifMi { get; set; } = true;
    }

    [Table("ArsivSekmeler")]
    public class ArsivSekme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Baslik { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string TabId { get; set; } = null!;

        [StringLength(50)]
        public string? Ikon { get; set; }

        public string? Icerik { get; set; }

        public int SiraNo { get; set; } = 1;

        public bool SabitTasarimMi { get; set; } = false;

        public bool IsActive { get; set; } = true;
    }

    [Table("HizliIslemler")]
    public class HizliIslem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Baslik { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Ikon { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string Url { get; set; } = null!;

        public bool YeniSekme { get; set; } = false;

        [StringLength(20)]
        public string TemaRengi { get; set; } = "blue";

        public int SiraNo { get; set; } = 1;

        public bool IsActive { get; set; } = true;
    }

    [Table("KurumsalSekmeler")]
    public class KurumsalSekme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Baslik { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string SekmeId { get; set; } = string.Empty;

        public string? Icerik { get; set; }

        [StringLength(100)]
        public string? IconClass { get; set; }

        public int Sira { get; set; } = 99;

        public bool AktifMi { get; set; } = true;

        [Required]
        [StringLength(50)]
        public string SayfaKey { get; set; } = string.Empty;
    }

    [Table("AdminMenuItems")]
    public class AdminMenuItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Section { get; set; } = null!;

        [StringLength(50)]
        public string? PermissionKey { get; set; }

        [Required]
        [StringLength(255)]
        public string Url { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string Label { get; set; } = null!;

        [StringLength(100)]
        public string? IconClass { get; set; }

        [StringLength(50)]
        public string? Controller { get; set; }

        [StringLength(50)]
        public string? Action { get; set; }

        [StringLength(250)]
        public string? ActiveIconClass { get; set; }

        [StringLength(250)]
        public string? HoverIconClass { get; set; }

        public int SortOrder { get; set; } = 1;

        public bool IsSuperAdminOnly { get; set; } = false;

        public bool IsActive { get; set; } = true;
    }

    [Table("PanelAyarlar")]
    public class PanelAyar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string AyarKey { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public string AyarValue { get; set; } = null!;
    }

    [Table("DoktorRandevuPlanlari")]
    public class DoktorRandevuPlani
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int DoktorId { get; set; }

        public int? BolumId { get; set; }

        public int Yil { get; set; }

        public int Ay { get; set; }

        public int SlotSureDakika { get; set; } = 30;

        public TimeSpan BaslangicSaati { get; set; } = new TimeSpan(9, 0, 0);

        public TimeSpan BitisSaati { get; set; } = new TimeSpan(17, 0, 0);

        public TimeSpan OgleMolaBaslangicSaati { get; set; } = new TimeSpan(12, 0, 0);

        public TimeSpan OgleMolaBitisSaati { get; set; } = new TimeSpan(13, 0, 0);

        public virtual Doktor? Doktor { get; set; }

        public virtual ICollection<DoktorRandevuPlanGunu> Gunler { get; set; } = new List<DoktorRandevuPlanGunu>();
    }

    [Table("DoktorRandevuPlanGunleri")]
    public class DoktorRandevuPlanGunu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PlanId { get; set; }

        public DateTime Tarih { get; set; }

        public bool IsRandevuAcik { get; set; } = true;

        [Column("GunlukRandevuSayisi")]
        public int GunlukMaxRandevu { get; set; } = 20;

        public TimeSpan? BaslangicSaati { get; set; }

        public TimeSpan? BitisSaati { get; set; }

        [ForeignKey("PlanId")]
        public virtual DoktorRandevuPlani? Plan { get; set; }
    }

    [Table("BasinKurumsalIletisim")]
    public class BasinKurumsalIletisim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Baslik { get; set; } = "Basın ve Kurumsal İletişim Birimi";

        [Required]
        public string? Aciklama { get; set; }

        [StringLength(50)]
        public string? Telefon { get; set; }

        [StringLength(150)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(150)]
        public string? Lokasyon { get; set; }

        public DateTime SonGuncelleme { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}