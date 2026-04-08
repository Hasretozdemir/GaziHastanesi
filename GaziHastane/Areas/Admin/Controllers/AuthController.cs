using Microsoft.AspNetCore.Mvc;
using GaziHastane.Data;
using GaziHastane.Models; // Yetkili modeline ulaşmak için bu şart
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using GaziHastane.Security;

namespace GaziHastane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // DİKKAT: Giriş yapmayanların da bu sayfayı görebilmesi için şart!
    public class AuthController : Controller
    {
        private readonly GaziHastaneContext _context;

        public AuthController(GaziHastaneContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. GİRİŞ (LOGIN) İŞLEMLERİ
        // ==========================================

        // GET: Giriş Ekranını Aç
        [HttpGet]
        public IActionResult Login()
        {
            // Eğer kişi zaten giriş yapmışsa, login sayfasını görmesin, direkt panele gitsin
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            return View();
        }

        // POST: Form Doldurulup Gönderildiğinde Çalışır
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string sifre)
        {
            // Veritabanında Yetkililer tablosuna bak (Şifre ve E-posta doğru mu? Aktif mi?)
            var yetkili = _context.Yetkililer.FirstOrDefault(y => y.Email == email && y.SifreHash == sifre && y.IsActive);

            if (yetkili != null)
            {
                // Giriş Başarılı! Kullanıcının kimlik kartını (Claims) oluşturuyoruz
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, yetkili.AdSoyad),
                    new Claim(ClaimTypes.Email, yetkili.Email),
                    new Claim(ClaimTypes.Role, yetkili.Rol),
                    new Claim(AdminPanelPermissions.ClaimType, yetkili.AdminSayfaYetkileri ?? string.Empty)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Tarayıcı kapansa da hatırla
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8) // 8 saat sonra otomatik çıkış yap
                };

                // Çerezi (Cookie) tarayıcıya kaydet ve giriş yap
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Son giriş tarihini güncelle
                yetkili.SonGirisTarihi = DateTime.UtcNow;
                _context.Update(yetkili);
                await _context.SaveChangesAsync();

                // Admin Ana Sayfasına (Dashboard) yönlendir
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            // Hata varsa mesajı ekrana gönder
            ViewBag.ErrorMessage = "E-posta veya şifre hatalı, ya da hesabınız pasif durumda.";
            return View();
        }

        // ==========================================
        // 2. ÇIKIŞ YAPMA İŞLEMİ
        // ==========================================
        public async Task<IActionResult> Logout()
        {
            // Çerezi sil ve oturumu kapat
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Tekrar login sayfasına yönlendir
            return RedirectToAction("Login", "Auth", new { area = "Admin" });
        }

        // ==========================================
        // 3. PROFİL VE AYARLAR İŞLEMLERİ (YENİ EKLENDİ)
        // ==========================================

        // PROFİL SAYFASI (Görüntüleme)
        [Authorize] // Sadece giriş yapanlar görebilir
        [HttpGet]
        public IActionResult Profil()
        {
            // Giriş yapmış kullanıcının e-postasını çerezden (Claim) alıyoruz
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var yetkili = _context.Yetkililer.FirstOrDefault(y => y.Email == userEmail);

            if (yetkili == null) return NotFound();

            return View(yetkili);
        }

        // PROFİL SAYFASI (Güncelleme İşlemi)
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profil(Yetkili model)
        {
            var yetkili = _context.Yetkililer.Find(model.Id);
            if (yetkili != null)
            {
                yetkili.AdSoyad = model.AdSoyad;
                yetkili.Email = model.Email;

                // Eğer şifre alanı boş bırakılmadıysa şifreyi de güncelle
                if (!string.IsNullOrEmpty(model.SifreHash))
                {
                    yetkili.SifreHash = model.SifreHash;
                }

                _context.Update(yetkili);
                await _context.SaveChangesAsync();

                // Başarılı kaydetme mesajı
                ViewBag.SuccessMessage = "Profil bilgileriniz başarıyla güncellendi.";
            }
            return View(yetkili);
        }

        // AYARLAR SAYFASI
        [Authorize]
        [HttpGet]
        public IActionResult Ayarlar()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}