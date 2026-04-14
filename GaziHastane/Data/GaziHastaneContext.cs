using Microsoft.EntityFrameworkCore;
using GaziHastane.Models;

namespace GaziHastane.Data
{
    public class GaziHastaneContext : DbContext
    {
        public GaziHastaneContext(DbContextOptions<GaziHastaneContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Bolum> Bolumler { get; set; }
        public DbSet<Doktor> Doktorlar { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<TahlilSonuc> TahlilSonuclari { get; set; }
        public DbSet<BorcOdeme> BorclarOdemeler { get; set; }
        public DbSet<YemekListesi> YemekListesi { get; set; }
        public DbSet<Duyuru> Duyurular { get; set; }
        public DbSet<KaliteBelgesi> KaliteBelgeleri { get; set; }
        public DbSet<EgitimKarti> EgitimIcerikleri { get; set; }
        public DbSet<HastaRehberi> HastaRehberleri { get; set; }
        public DbSet<KrokiBlok> KrokiBloklar { get; set; }
        public DbSet<KrokiKat> KrokiKatlar { get; set; }
        public DbSet<KrokiBolum> KrokiBolumler { get; set; }
        public DbSet<KrokiBirim> KrokiBirimleri { get; set; }
        public DbSet<Iletisim> IletisimBilgileri { get; set; }
        public DbSet<UlasimRehberi> UlasimRehberleri { get; set; }

        public DbSet<Yetkili> Yetkililer { get; set; }
        public DbSet<Haber> Haberler { get; set; }
        public DbSet<Etkinlik> Etkinlikler { get; set; }
        public DbSet<Medya> Medyalar { get; set; }
        public DbSet<Belge> Belgeler { get; set; }
        public DbSet<KurumsalMenuGrup> KurumsalMenuGruplar { get; set; }
        public DbSet<KurumsalMenu> KurumsalMenuler { get; set; }
        public DbSet<KurumsalSayfa> KurumsalSayfalar { get; set; }
        public DbSet<BashekimlikPersonel> BashekimlikPersoneller { get; set; }
        public DbSet<BasmudurlikPersonel> BasmudurlikPersoneller { get; set; }
        public DbSet<HemsirelikAyar> HemsirelikAyarlar { get; set; }
        public DbSet<HemsirelikIcerik> HemsirelikIcerikler { get; set; }
        public DbSet<HemsirelikSekme> HemsirelikSekmeler { get; set; }
        public DbSet<ArsivSekme> ArsivSekmeler { get; set; }
        public DbSet<HizliIslem> HizliIslemler { get; set; }
        public DbSet<AdminMenuItem> AdminMenuItems { get; set; }
        public DbSet<PanelAyar> PanelAyarlari { get; set; }
        public DbSet<DoktorRandevuPlani> DoktorRandevuPlanlari { get; set; }
        public DbSet<DoktorRandevuPlanGunu> DoktorRandevuPlanGunleri { get; set; }
    }
}