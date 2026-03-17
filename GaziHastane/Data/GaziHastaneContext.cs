using Microsoft.EntityFrameworkCore;
using GaziHastane.Models;

namespace GaziHastane.Data
{
    public class GaziHastaneContext : DbContext
    {
        public GaziHastaneContext(DbContextOptions<GaziHastaneContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Bolum> Bolumler { get; set; }
        public DbSet<Doktor> Doktorlar { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<TahlilSonuc> TahlilSonuclari { get; set; }
        public DbSet<BorcOdeme> BorclarOdemeler { get; set; }
        public DbSet<YemekListesi> YemekListesi { get; set; }
        public DbSet<Duyuru> Duyurular { get; set; }
        public DbSet<KaliteBelgesi> KaliteBelgeleri { get; set; }
        public DbSet<EgitimKomitesiUye> EgitimKomitesi { get; set; }
        public DbSet<HastaRehberi> HastaRehberleri { get; set; }

        public DbSet<Iletisim> IletisimBilgileri { get; set; }
        public DbSet<UlasimRehberi> UlasimRehberleri { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TahlilSonuclari -> Doktor (SetNull)
            modelBuilder.Entity<TahlilSonuc>()
                .HasOne(t => t.Doktor)
                .WithMany(d => d.TahlilSonuclari)
                .HasForeignKey(t => t.DoktorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Doktor -> Bolum (Restrict - simplified as no cascade)
            modelBuilder.Entity<Doktor>()
                .HasOne(d => d.Bolum)
                .WithMany(b => b.Doktorlar)
                .HasForeignKey(d => d.BolumId)
                .OnDelete(DeleteBehavior.Restrict);

             // Randevu -> Bolum (Cascade)
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Bolum)
                .WithMany(b => b.Randevular)
                .HasForeignKey(r => r.BolumId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
