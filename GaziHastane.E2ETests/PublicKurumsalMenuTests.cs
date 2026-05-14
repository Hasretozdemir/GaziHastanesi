using GaziHastane.E2ETests.Helpers;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Xunit;

namespace GaziHastane.E2ETests;

public class PublicKurumsalMenuTests : PageTest
{
    [Fact]
    public async Task KurumsalMenuLinks_AreReadable_AndReachable()
    {
        await Page.GotoAsync($"{TestSettings.BaseUrl}/Kurumsal/Index");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var linkTexts = await Page.Locator("a.sidebar-link, a.submenu-link").EvaluateAllAsync<string[]>(
            "els => els.map(e => (e.textContent || '').trim())");
        Assert.DoesNotContain(linkTexts, text => string.IsNullOrWhiteSpace(text));

        var rawLinks = await Page.Locator("a.sidebar-link, a.submenu-link").EvaluateAllAsync<string[]>(
            "els => els.map(e => e.getAttribute('href') || '')");

        var links = rawLinks
            .Select(link => link.Split('#')[0])
            .Where(link => !string.IsNullOrWhiteSpace(link))
            .Where(link => link.StartsWith("/Kurumsal", StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var link in links)
        {
            await Page.GotoAsync($"{TestSettings.BaseUrl}{link}");
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await PageAssertions.AssertReadableMainContentAsync(Page);
        }
    }
}
