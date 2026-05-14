using Microsoft.Playwright;
using Xunit;

namespace GaziHastane.E2ETests.Helpers;

public static class PageAssertions
{
    public static async Task AssertReadableMainContentAsync(IPage page)
    {
        var main = page.Locator("main");
        if (!await main.IsVisibleAsync())
        {
            main = page.Locator("body");
        }

        var text = (await main.InnerTextAsync()).Trim();
        Assert.False(string.IsNullOrWhiteSpace(text));
        Assert.DoesNotContain("undefined", text, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("null", text, StringComparison.OrdinalIgnoreCase);

        var headings = page.Locator("h1, h2, h3");
        if (await headings.CountAsync() > 0)
        {
            var headingTexts = await headings.EvaluateAllAsync<string[]>("els => els.map(e => e.textContent || '')");
            Assert.Contains(headingTexts, h => !string.IsNullOrWhiteSpace(h));
        }
    }
}
