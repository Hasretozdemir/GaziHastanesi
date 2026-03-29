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
   
    }
}