using GaziHastane.Data;
using Microsoft.EntityFrameworkCore;

namespace GaziHastane
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // PostgreSQL 6.0+ sürümlerinde DateTime.Local hatasýný önlemek için 
            // eski zaman davranýþýný (Legacy Behavior) etkinleþtiriyoruz.
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // PostgreSQL veritabaný servisi ekleniyor
            builder.Services.AddDbContext<GaziHastaneContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Veritabanýna baþlangýç verilerini ekle (Seed Data)
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<GaziHastaneContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Veritabaný oluþturulurken veya veri eklenirken hata: " + ex.Message);
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // 1. AREA ROTASI (Admin paneli için - Üstte olmalý)
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            // 2. DEFAULT ROTA (Ziyaretçi önyüzü için - Altta olmalý)
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}