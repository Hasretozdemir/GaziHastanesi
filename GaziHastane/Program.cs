using GaziHastane.Data;
using GaziHastane.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

namespace GaziHastane
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("Logs/SistemLog-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                // PostgreSQL 6.0+ sürümlerinde DateTime.Local hatasýný önlemek için 
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog();

            // PostgreSQL veritabaný servisi ekleniyor
            builder.Services.AddDbContext<GaziHastaneContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<AdminPagePermissionFilter>();
                options.Filters.Add<AdminActionLogFilter>();
            });

            // ------------------------------------------------------------------
            // KÝMLÝK DOÐRULAMA VE ÇEREZ (COOKIE) AYARLARI
            // ------------------------------------------------------------------
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Admin/Auth/Login"; // Giriþ yapýlmamýþsa yönlendirilecek sayfa
                    options.LogoutPath = "/Admin/Auth/Logout"; // Çýkýþ yapýldýðýnda gidilecek sayfa
                    options.AccessDeniedPath = "/Admin/Auth/Login"; // Yetkisiz eriþimde yönlendirilecek sayfa
                    options.Cookie.Name = "GaziMedAdminAuth"; // Tarayýcýda tutulacak çerez (cookie) adý
                    options.ExpireTimeSpan = TimeSpan.FromHours(8); // Oturum 8 saat açýk kalsýn
                });

                var app = builder.Build();

                app.UseSerilogRequestLogging();

                // Veritabanýna baþlangýç verilerini ekle (Seed Data)
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<GaziHastaneContext>();
                        context.Database.Migrate();

                        context.Database.ExecuteSqlRaw("""
                            ALTER TABLE "Etkinlikler"
                            ADD COLUMN IF NOT EXISTS "GorselUrl" character varying(255);
                            """);

                        context.Database.ExecuteSqlRaw("""
                            ALTER TABLE "Etkinlikler"
                            ADD COLUMN IF NOT EXISTS "ModalIcerik" text;
                            """);

                        DbInitializer.Initialize(context);
                    }
                    catch (Exception ex)
                    {
                        Log.Fatal(ex, "Veritabaný oluþturulurken veya veri eklenirken hata oluþtu.");
                    }
                }

                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();

                // DÝKKAT: UseAuthentication HER ZAMAN UseAuthorization'dan ÖNCE GELMELÝ!
                app.UseAuthentication();
                app.UseAuthorization();

                // 1. AREA ROTASI (Admin paneli için)
                app.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                // 2. DEFAULT ROTA (Ziyaretçi önyüzü için)
                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                app.Run();
            }
            catch (Exception ex) when (!string.Equals(ex.GetType().Name, "HostAbortedException", StringComparison.Ordinal))
            {
                Log.Fatal(ex, "Uygulama beklenmeyen bir þekilde çöktü.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}