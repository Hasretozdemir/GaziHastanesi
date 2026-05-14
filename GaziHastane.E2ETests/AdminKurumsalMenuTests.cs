using GaziHastane.E2ETests.Helpers;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Xunit;

namespace GaziHastane.E2ETests;

public class AdminKurumsalMenuTests : PageTest
{
    [Fact]
    public async Task KurumsalAdminPages_AreReadable_AndReachable()
    {
        await LoginAsync();

        var adminPages = new[]
        {
            "/Admin/Kurumsal/Index",
            "/Admin/Kurumsal/Hakkimizda",
            "/Admin/Kurumsal/ArsivBirimi",
            "/Admin/Kurumsal/BasinVeKurumsalIletisim",
            "/Admin/Kurumsal/BilgiIslemMerkezi",
            "/Admin/Kurumsal/EczacilikHizmetleri",
            "/Admin/Kurumsal/EnfeksiyonKontrol",
            "/Admin/Kurumsal/HastaIletisimBirimi",
            "/Admin/Kurumsal/HemsirelikHizmetleri",
            "/Admin/Kurumsal/IcKontrol",
            "/Admin/Kurumsal/IsAkisSemalari",
            "/Admin/Kurumsal/IsSagligiVeGuvenligi",
            "/Admin/Kurumsal/IstatistikVeRaporlama",
            "/Admin/Kurumsal/OrganizasyonSemalari",
            "/Admin/Kurumsal/SatinAlma"
        };

        foreach (var path in adminPages)
        {
            await Page.GotoAsync($"{TestSettings.BaseUrl}{path}");
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await PageAssertions.AssertReadableMainContentAsync(Page);
        }
    }

    private async Task LoginAsync()
    {
        await Page.GotoAsync($"{TestSettings.BaseUrl}/Admin/Auth/Login");
        await Page.FillAsync("input[name='email']", TestSettings.AdminEmail);
        await Page.FillAsync("input[name='sifre']", TestSettings.AdminPassword);
        await Page.ClickAsync("button[type='submit']");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}
