using System.Security.Claims;
using GaziHastane.Models;

namespace GaziHastane.Security
{
    public sealed class AdminPermissionItem
    {
        public string Key { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }

    public sealed class AdminSidebarLink
    {
        public string PermissionKey { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string IconClass { get; set; } = string.Empty;
        public string Controller { get; set; } = string.Empty;
        public string? Action { get; set; }
        public string ActiveIconClass { get; set; } = "neon-icon";
        public string HoverIconClass { get; set; } = string.Empty;
    }

    public sealed class AdminSidebarSubLink
    {
        public string Url { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Controller { get; set; } = string.Empty;
        public string? Action { get; set; }
    }

    public static class AdminPanelPermissions
    {
        public const string ClaimType = "AdminPages";

        public static class Keys
        {
            public const string Dashboard = "dashboard";
            public const string Doktorlar = "doktorlar";
            public const string Bolumler = "bolumler";
            public const string HastaRehberi = "hastarehberi";
            public const string Iletisim = "iletisim";
            public const string KaliteYonetimi = "kaliteyonetimi";
            public const string Egitim = "egitim";
            public const string YemekListesi = "yemeklistesi";
            public const string Kroki = "kroki";
            public const string Medya = "medya";
            public const string DijitalIslemler = "dijitalislemler";
            public const string Haberler = "haberler";
            public const string Etkinlikler = "etkinlikler";
            public const string Duyurular = "duyurular";
            public const string Kurumsal = "kurumsal";
            public const string DoktorRandevuPlani = "doktorrandevuplani";
            public const string Yetkililer = "yetkililer";
            public const string TahlilSonuclari = "tahlilsonuclari";
            public const string BorcYonetimi = "borcyonetimi";
        }

        public static readonly IReadOnlyList<AdminPermissionItem> All = new List<AdminPermissionItem>
        {
            new() { Key = Keys.Dashboard, DisplayName = "Gösterge Paneli" },
            new() { Key = Keys.Doktorlar, DisplayName = "Doktorlar" },
            new() { Key = Keys.Bolumler, DisplayName = "Poliklinikler" },
            new() { Key = Keys.HastaRehberi, DisplayName = "Hasta Rehberi" },
            new() { Key = Keys.Iletisim, DisplayName = "İletişim" },
            new() { Key = Keys.KaliteYonetimi, DisplayName = "Kalite Yönetimi" },
            new() { Key = Keys.Egitim, DisplayName = "Eğitim Komitesi" },
            new() { Key = Keys.YemekListesi, DisplayName = "Yemek Listesi" },
            new() { Key = Keys.Kroki, DisplayName = "Kroki Yönetimi" },
            new() { Key = Keys.Medya, DisplayName = "Görseller / Slider / Belgeler" },
            new() { Key = Keys.DijitalIslemler, DisplayName = "Dijital İşlemler" },
            new() { Key = Keys.Haberler, DisplayName = "Haberler" },
            new() { Key = Keys.Etkinlikler, DisplayName = "Etkinlikler" },
            new() { Key = Keys.Duyurular, DisplayName = "Duyurular" },
            new() { Key = Keys.Kurumsal, DisplayName = "Kurumsal İçerikler" },
            new() { Key = Keys.DoktorRandevuPlani, DisplayName = "Doktor Randevu Planı" },
            new() { Key = Keys.Yetkililer, DisplayName = "Yetkili Yönetimi" },
            new() { Key = Keys.TahlilSonuclari, DisplayName = "Tahlil Sonuç Yönetimi" },
            new() { Key = Keys.BorcYonetimi, DisplayName = "Borç ve Ödeme Yönetimi" }
        };

        public static readonly IReadOnlyList<AdminSidebarLink> MainScreenLinks = new List<AdminSidebarLink>
        {
            new() { PermissionKey = Keys.Dashboard, Url = "/Admin/Home/Index", Label = "Gösterge Paneli", IconClass = "fa-solid fa-cube", Controller = "Home", HoverIconClass = "group-hover:text-cyan-400" },
            new() { PermissionKey = Keys.Doktorlar, Url = "/Admin/Doktorlar/Index", Label = "Doktorlar", IconClass = "fa-solid fa-user-doctor", Controller = "Doktorlar", HoverIconClass = "group-hover:text-purple-400" },
            new() { PermissionKey = Keys.Bolumler, Url = "/Admin/Bolumler/Index", Label = "Poliklinikler", IconClass = "fa-solid fa-network-wired", Controller = "Bolumler", HoverIconClass = "group-hover:text-pink-400" },
            new() { PermissionKey = Keys.HastaRehberi, Url = "/Admin/HastaRehberi/Index", Label = "Hasta Rehberi", IconClass = "fa-solid fa-book-medical", Controller = "HastaRehberi", HoverIconClass = "group-hover:text-emerald-400" },
            new() { PermissionKey = Keys.Iletisim, Url = "/Admin/Iletisim/Index", Label = "İletişim", IconClass = "fa-solid fa-address-book", Controller = "Iletisim", HoverIconClass = "group-hover:text-orange-400" },
            new() { PermissionKey = Keys.BorcYonetimi, Url = "/Admin/BorcYonetim/Index", Label = "Borç Yönetimi", IconClass = "fa-solid fa-file-invoice-dollar", Controller = "BorcYonetim", HoverIconClass = "group-hover:text-yellow-400" }
        };

        public static readonly IReadOnlyList<AdminSidebarLink> SystemSettingsLinks = new List<AdminSidebarLink>
        {
            new() { PermissionKey = Keys.KaliteYonetimi, Url = "/Admin/HizliIslem/Kalite", Label = "Kalite Yönetimi", IconClass = "fa-solid fa-shield-heart", Controller = "HizliIslem", Action = "Kalite", HoverIconClass = "group-hover:text-blue-400" },
            new() { PermissionKey = Keys.Egitim, Url = "/Admin/Egitim/Index", Label = "Eğitim Komitesi", IconClass = "fa-solid fa-graduation-cap", Controller = "Egitim", HoverIconClass = "group-hover:text-orange-500" },
            new() { PermissionKey = Keys.YemekListesi, Url = "/Admin/HizliIslem/YemekListesi", Label = "Yemek Listesi", IconClass = "fa-solid fa-utensils", Controller = "HizliIslem", Action = "YemekListesi", HoverIconClass = "group-hover:text-orange-500" },
            new() { PermissionKey = Keys.Kroki, Url = "/Admin/Kroki/Index", Label = "Kroki Yönetimi", IconClass = "fa-solid fa-map-location-dot", Controller = "Kroki", HoverIconClass = "group-hover:text-cyan-400" },
            new() { PermissionKey = Keys.Medya, Url = "/Admin/HizliIslem/AnaSayfaGorsel", Label = "Ana Sayfa Slider", IconClass = "fa-solid fa-image", Controller = "HizliIslem", Action = "AnaSayfaGorsel", ActiveIconClass = "text-yellow-500 drop-shadow-[0_0_8px_rgba(234,179,8,0.8)] scale-110", HoverIconClass = "group-hover:text-yellow-400 group-hover:scale-110" },
            new() { PermissionKey = Keys.DoktorRandevuPlani, Url = "/Admin/DoktorRandevuPlan/Index", Label = "Doktor Planlama", IconClass = "fa-solid fa-calendar-plus", Controller = "DoktorRandevuPlan", HoverIconClass = "group-hover:text-cyan-400" },
            new() { PermissionKey = Keys.TahlilSonuclari, Url = "/Admin/TahlilSonuclari/Giris", Label = "Tahlil Sonuç Girişi", IconClass = "fa-solid fa-vials", Controller = "TahlilSonuclari", Action = "Giris", HoverIconClass = "group-hover:text-emerald-400" }
        };

        public static readonly IReadOnlyList<AdminSidebarLink> SecurityLinks = new List<AdminSidebarLink>
        {
            new() { PermissionKey = Keys.Yetkililer, Url = "/Admin/Yetkililer/Index", Label = "Yetkili Kontrolü", IconClass = "fa-solid fa-user-shield", Controller = "Yetkililer", HoverIconClass = "group-hover:text-rose-400" }
        };

        public static readonly IReadOnlyList<AdminSidebarLink> ContentLinks = new List<AdminSidebarLink>
        {
            new() { PermissionKey = Keys.DijitalIslemler, Url = "/Admin/HizliIslem/Index", Label = "Dijital İşlemler", IconClass = "fa-solid fa-layer-group", Controller = "HizliIslem", Action = "Index", ActiveIconClass = "neon-icon text-cyan-400", HoverIconClass = "group-hover:text-cyan-400" },
            new() { PermissionKey = Keys.Haberler, Url = "/Admin/Haberler/Index", Label = "Haberler", IconClass = "fa-solid fa-newspaper", Controller = "Haberler", HoverIconClass = "group-hover:text-blue-400" },
            new() { PermissionKey = Keys.Etkinlikler, Url = "/Admin/Etkinlikler/Index", Label = "Etkinlikler", IconClass = "fa-solid fa-calendar-days", Controller = "Etkinlikler", HoverIconClass = "group-hover:text-fuchsia-400" },
            new() { PermissionKey = Keys.Duyurular, Url = "/Admin/Duyurular/Index", Label = "Duyurular", IconClass = "fa-solid fa-bullhorn", Controller = "Duyurular", HoverIconClass = "group-hover:text-yellow-400" }
        };

        public static readonly IReadOnlyList<AdminSidebarSubLink> KurumsalSubLinks = new List<AdminSidebarSubLink>
        {
            new() { Url = "/Admin/Kurumsal/Hakkimizda", Label = "Hakkımızda Sayfası", Controller = "Kurumsal", Action = "Hakkimizda" },
            new() { Url = "/Admin/Kurumsal/Index", Label = "Sidebar Yönetimi", Controller = "Kurumsal", Action = "Index" },
            new() { Url = "/Admin/Bashekimlik/Index", Label = "Başhekimlik", Controller = "Bashekimlik" },
            new() { Url = "/Admin/Basmudurluk/Index", Label = "Başmüdürlük", Controller = "Basmudurluk" },
            new() { Url = "/Admin/Kurumsal/HemsirelikHizmetleri", Label = "Hemşirelik Hizmetleri", Controller = "Kurumsal", Action = "HemsirelikHizmetleri" },
            new() { Url = "/Admin/Kurumsal/BilgiIslemMerkezi", Label = "Bilgi İşlem Merkezi", Controller = "Kurumsal", Action = "BilgiIslemMerkezi" },
            new() { Url = "/Admin/Kurumsal/IsSagligiVeGuvenligi", Label = "İş Sağlığı ve Güvenliği", Controller = "Kurumsal", Action = "IsSagligiVeGuvenligi" },
            new() { Url = "/Admin/Kurumsal/EnfeksiyonKontrol", Label = "Enfeksiyon Kontrol", Controller = "Kurumsal", Action = "EnfeksiyonKontrol" },
            new() { Url = "/Admin/Kurumsal/EczacilikHizmetleri", Label = "Eczacılık Hizmetleri", Controller = "Kurumsal", Action = "EczacilikHizmetleri" },
            new() { Url = "/Admin/Kurumsal/SatinAlma", Label = "Satın Alma", Controller = "Kurumsal", Action = "SatinAlma" },
            new() { Url = "/Admin/Kurumsal/IstatistikVeRaporlama", Label = "İstatistik ve Raporlama", Controller = "Kurumsal", Action = "IstatistikVeRaporlama" },
            new() { Url = "/Admin/Kurumsal/ArsivBirimi", Label = "Arşiv Birimi", Controller = "Kurumsal", Action = "ArsivBirimi" },
            new() { Url = "/Admin/Kurumsal/HastaIletisimBirimi", Label = "Hasta İletişim Birimi", Controller = "Kurumsal", Action = "HastaIletisimBirimi" },
            new() { Url = "/Admin/Kurumsal/IsAkisSemalari", Label = "İş Akış Şemaları", Controller = "Kurumsal", Action = "IsAkisSemalari" },
            new() { Url = "/Admin/Kurumsal/OrganizasyonSemalari", Label = "Organizasyon Şemaları", Controller = "Kurumsal", Action = "OrganizasyonSemalari" },
            new() { Url = "/Admin/Kurumsal/IcKontrol", Label = "İç Kontrol", Controller = "Kurumsal", Action = "IcKontrol" },
            new() { Url = "/Admin/Kurumsal/BasinVeKurumsalIletisim", Label = "Basın ve Kurumsal İletişim", Controller = "Kurumsal", Action = "BasinVeKurumsalIletisim" }
        };

        private static readonly Dictionary<string, string> ControllerPermissions = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Home"] = Keys.Dashboard,
            ["Doktorlar"] = Keys.Doktorlar,
            ["Bolumler"] = Keys.Bolumler,
            ["HastaRehberi"] = Keys.HastaRehberi,
            ["Iletisim"] = Keys.Iletisim,
            ["Egitim"] = Keys.Egitim,
            ["Kroki"] = Keys.Kroki,
            ["Haberler"] = Keys.Haberler,
            ["Etkinlikler"] = Keys.Etkinlikler,
            ["Duyurular"] = Keys.Duyurular,
            ["Kurumsal"] = Keys.Kurumsal,
            ["Bashekimlik"] = Keys.Kurumsal,
            ["Basmudurluk"] = Keys.Kurumsal,
            ["DoktorRandevuPlan"] = Keys.DoktorRandevuPlani,
            ["Yetkililer"] = Keys.Yetkililer,
            ["TahlilSonuclari"] = Keys.TahlilSonuclari,
            ["BorcYonetim"] = Keys.BorcYonetimi
        };

        private static readonly Dictionary<string, string> HizliIslemActionPermissions = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Kalite"] = Keys.KaliteYonetimi,
            ["YemekListesi"] = Keys.YemekListesi,
            ["Gorsel"] = Keys.Medya,
            ["AnaSayfaGorsel"] = Keys.Medya,
            ["Belge"] = Keys.Medya,
            ["Index"] = Keys.DijitalIslemler
        };

        public static HashSet<string> AllKeys => All.Select(x => x.Key).ToHashSet(StringComparer.OrdinalIgnoreCase);

        public static bool IsSuperAdmin(ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);
            return user.IsInRole("Süper Admin") || email == "admin@gazihastanesi.com";
        }

        public static bool IsSidebarLinkActive(string? currentController, string? currentAction, AdminSidebarLink link)
        {
            if (!string.Equals(currentController, link.Controller, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(link.Action))
            {
                return true;
            }

            if (string.Equals(link.Controller, "HizliIslem", StringComparison.OrdinalIgnoreCase) &&
                string.Equals(link.Action, "Index", StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(currentAction, "Index", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(currentAction);
            }

            return string.Equals(currentAction, link.Action, StringComparison.OrdinalIgnoreCase);
        }

        public static List<AdminSidebarLink> BuildSidebarLinks(IEnumerable<AdminMenuItem> allItems, string section)
        {
            return allItems
                .Where(x => x.IsActive && x.Section.Equals(section, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.SortOrder)
                .Select(x => new AdminSidebarLink
                {
                    PermissionKey = x.PermissionKey ?? string.Empty,
                    Url = x.Url,
                    Label = x.Label,
                    IconClass = string.IsNullOrWhiteSpace(x.IconClass) ? "fa-solid fa-circle" : x.IconClass,
                    Controller = x.Controller ?? string.Empty,
                    Action = x.Action,
                    ActiveIconClass = string.IsNullOrWhiteSpace(x.ActiveIconClass) ? "neon-icon" : x.ActiveIconClass,
                    HoverIconClass = x.HoverIconClass ?? string.Empty
                })
                .ToList();
        }

        public static List<AdminSidebarSubLink> BuildSidebarSubLinks(IEnumerable<AdminMenuItem> allItems, string section)
        {
            return allItems
                .Where(x => x.IsActive && x.Section.Equals(section, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.SortOrder)
                .Select(x => new AdminSidebarSubLink
                {
                    Url = x.Url,
                    Label = x.Label,
                    Controller = x.Controller ?? string.Empty,
                    Action = x.Action
                })
                .ToList();
        }

        public static bool IsSidebarSubLinkActive(string? currentController, string? currentAction, AdminSidebarSubLink link)
        {
            if (!string.Equals(currentController, link.Controller, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(link.Action))
            {
                return true;
            }

            return string.Equals(currentAction, link.Action, StringComparison.OrdinalIgnoreCase);
        }

        public static string Serialize(IEnumerable<string>? permissions)
        {
            if (permissions == null)
            {
                return string.Empty;
            }

            var normalized = permissions
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Where(AllKeys.Contains)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            return string.Join(",", normalized);
        }

        public static HashSet<string> Parse(string? permissions)
        {
            if (string.IsNullOrWhiteSpace(permissions))
            {
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }

            return permissions
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(AllKeys.Contains)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
        }

        public static bool UserHasPermission(ClaimsPrincipal user, string key)
        {
            if (IsSuperAdmin(user))
            {
                return true;
            }

            var rawPermissions = user.FindFirstValue(ClaimType);
            var permissions = Parse(rawPermissions);
            return permissions.Contains(key);
        }

        public static bool CanAccessController(ClaimsPrincipal user, string? controller, string? action)
        {
            if (string.IsNullOrWhiteSpace(controller))
            {
                return true;
            }

            if (IsSuperAdmin(user))
            {
                return true;
            }

            if (controller.Equals("Auth", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (controller.Equals("Yetkililer", StringComparison.OrdinalIgnoreCase))
            {
                return action != null && action.Equals("Profil", StringComparison.OrdinalIgnoreCase);
            }

            if (controller.Equals("HizliIslem", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrWhiteSpace(action) && HizliIslemActionPermissions.TryGetValue(action, out var actionPermission))
                {
                    return UserHasPermission(user, actionPermission);
                }

                return UserHasPermission(user, Keys.DijitalIslemler);
            }

            if (ControllerPermissions.TryGetValue(controller, out var permission))
            {
                return UserHasPermission(user, permission);
            }

            return true;
        }
    }
}
