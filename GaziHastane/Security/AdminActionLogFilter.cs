using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GaziHastane.Security
{
    public class AdminActionLogFilter : IAsyncActionFilter
    {
        private readonly GaziHastaneContext _context;

        public AdminActionLogFilter(GaziHastaneContext context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();

            var area = context.RouteData.Values["area"]?.ToString();
            if (!string.Equals(area, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (executedContext.Exception != null)
            {
                return;
            }

            var statusCode = context.HttpContext.Response.StatusCode;
            if (statusCode >= 400)
            {
                return;
            }

            if (executedContext.Controller is Controller controllerBase && !controllerBase.ModelState.IsValid)
            {
                return;
            }

            var controller = context.RouteData.Values["controller"]?.ToString() ?? "Bilinmeyen";
            var action = context.RouteData.Values["action"]?.ToString() ?? "Bilinmeyen";
            var method = context.HttpContext.Request.Method;

            if (!ShouldLogAction(method, action))
            {
                return;
            }

            if (IsFailedLogin(context, controller, action))
            {
                return;
            }

            var islemTipi = GetIslemTipi(method, controller, action, context);
            var kullanici = context.HttpContext.User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(kullanici)
                && string.Equals(controller, "Auth", StringComparison.OrdinalIgnoreCase)
                && string.Equals(action, "Login", StringComparison.OrdinalIgnoreCase)
                && context.HttpContext.Request.HasFormContentType)
            {
                kullanici = context.HttpContext.Request.Form["email"].FirstOrDefault();
            }

            var ipAdresi = context.HttpContext.Connection.RemoteIpAddress?.ToString();

            _context.AdminLoglari.Add(new AdminLog
            {
                KullaniciAdi = string.IsNullOrWhiteSpace(kullanici) ? "Bilinmiyor" : kullanici,
                IslemTipi = islemTipi,
                Modul = GetModul(controller, action),
                Aciklama = $"{controller}/{action} iþlemi gerçekleþtirildi.",
                IpAdresi = ipAdresi
            });

            await _context.SaveChangesAsync();
        }

        private static bool ShouldLogAction(string method, string action)
        {
            if (string.Equals(method, "HEAD", StringComparison.OrdinalIgnoreCase)
                || string.Equals(method, "OPTIONS", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var normalizedAction = action.ToLowerInvariant();
            var isDeleteAction = normalizedAction.Contains("sil")
                                 || normalizedAction.Contains("delete")
                                 || normalizedAction.Contains("remove");

            if (string.Equals(method, "GET", StringComparison.OrdinalIgnoreCase))
            {
                return isDeleteAction;
            }

            return true;
        }

        private static bool IsFailedLogin(ActionExecutingContext context, string controller, string action)
        {
            if (!string.Equals(controller, "Auth", StringComparison.OrdinalIgnoreCase)
                || !string.Equals(action, "Login", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return context.HttpContext.User.Identity?.IsAuthenticated != true;
        }

        private static string GetModul(string controller, string action)
        {
            if (string.Equals(controller, "Auth", StringComparison.OrdinalIgnoreCase))
            {
                return "Giriþ Paneli";
            }

            if (string.Equals(controller, "HizliIslem", StringComparison.OrdinalIgnoreCase))
            {
                if (action.StartsWith("AnaSayfaGorsel", StringComparison.OrdinalIgnoreCase)
                    || action.StartsWith("AnaSayfaSlider", StringComparison.OrdinalIgnoreCase))
                {
                    return "Ana Sayfa Slider";
                }

                if (action.StartsWith("Belge", StringComparison.OrdinalIgnoreCase))
                {
                    return "Belgeler";
                }

                if (action.StartsWith("Kalite", StringComparison.OrdinalIgnoreCase))
                {
                    return "Kalite Yönetimi";
                }

                if (action.StartsWith("Yemek", StringComparison.OrdinalIgnoreCase))
                {
                    return "Yemek Listesi";
                }

                if (action.StartsWith("Gorsel", StringComparison.OrdinalIgnoreCase)
                    || action.StartsWith("Slider", StringComparison.OrdinalIgnoreCase))
                {
                    return "Görseller";
                }

                return "Dijital Ýþlemler";
            }

            if (string.Equals(controller, "TahlilSonuclari", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(action, "Giris", StringComparison.OrdinalIgnoreCase))
                {
                    return "Tahlil Sonuç Giriþi";
                }

                if (string.Equals(action, "Sorgula", StringComparison.OrdinalIgnoreCase))
                {
                    return "Tahlil Sonuç Sorgu";
                }

                return "Tahlil Sonuç";
            }

            return controller.ToLowerInvariant() switch
            {
                "doktorrandevuplan" => "Doktor Planlama",
                "doktorlar" => "Doktorlar",
                "bolumler" => "Poliklinikler",
                "hastarehberi" => "Hasta Rehberi",
                "iletisim" => "Ýletiþim",
                "borcyonetim" => "Borç Yönetimi",
                "haberler" => "Haberler",
                "etkinlikler" => "Etkinlikler",
                "duyurular" => "Duyurular",
                "kurumsal" => "Kurumsal",
                "bashekimlik" => "Kurumsal",
                "basmudurluk" => "Kurumsal",
                "yetkililer" => "Yetkili Listesi",
                "egitim" => "Eðitim Komitesi",
                "kroki" => "Kroki Yönetimi",
                _ => controller
            };
        }

        private static string GetIslemTipi(string method, string controller, string action, ActionExecutingContext context)
        {
            if (string.Equals(controller, "Auth", StringComparison.OrdinalIgnoreCase)
                && string.Equals(action, "Login", StringComparison.OrdinalIgnoreCase))
            {
                return "GÝRÝÞ";
            }

            var normalizedAction = action.ToLowerInvariant();
            if (normalizedAction.Contains("sil")
                || normalizedAction.Contains("delete")
                || normalizedAction.Contains("remove")
                || normalizedAction.Contains("iptal")
                || string.Equals(method, "DELETE", StringComparison.OrdinalIgnoreCase))
            {
                return "SÝL";
            }

            if (normalizedAction.Contains("ekle")
                || normalizedAction.Contains("create")
                || normalizedAction.Contains("add")
                || normalizedAction.Contains("yukle"))
            {
                return "EKLE";
            }

            if (normalizedAction.Contains("kaydet"))
            {
                return IsNewRecord(context) ? "EKLE" : "GÜNCELLE";
            }

            if (normalizedAction.Contains("duzenle")
                || normalizedAction.Contains("edit")
                || normalizedAction.Contains("guncelle")
                || normalizedAction.Contains("update")
                || normalizedAction.Contains("durumguncelle"))
            {
                return "GÜNCELLE";
            }

            if (string.Equals(method, "POST", StringComparison.OrdinalIgnoreCase)
                || string.Equals(method, "PUT", StringComparison.OrdinalIgnoreCase)
                || string.Equals(method, "PATCH", StringComparison.OrdinalIgnoreCase)
                || string.Equals(method, "GET", StringComparison.OrdinalIgnoreCase))
            {
                return "GÜNCELLE";
            }

            return "ÝÞLEM";
        }

        private static bool IsNewRecord(ActionExecutingContext context)
        {
            foreach (var arg in context.ActionArguments.Values)
            {
                if (arg == null)
                {
                    continue;
                }

                var type = arg.GetType();
                var idProperty = type.GetProperty("Id");
                if (idProperty == null)
                {
                    continue;
                }

                var value = idProperty.GetValue(arg);
                if (value is int id)
                {
                    return id <= 0;
                }
            }

            return false;
        }
    }
}
