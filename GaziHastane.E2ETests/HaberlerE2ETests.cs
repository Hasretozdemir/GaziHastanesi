using Microsoft.Playwright;
using Microsoft.Playwright;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GaziHastane.E2ETests
{
    public class HaberlerE2ETests : AdminE2ETestBase
    {
        [Fact]
        public async Task Admin_CanCreate_And_View_Haber()
        {
            // Arrange: Admin olarak giriþ yap.
            await LoginAsAdminAsync();

            // Act: Yeni haber oluþtur.
            await Page.GotoAsync($"{BaseUrl}/Admin/Haberler/Create");
            var testBaslik = "E2E Test Haberi - " + Guid.NewGuid();
            await Page.FillAsync("input[name='Baslik']", testBaslik);
            await Page.FillAsync("input[name='Kategori']", "E2E");
            await Page.FillAsync("input[name='YayinTarihi']", DateTime.Now.ToString("yyyy-MM-ddTHH:mm"));
            await Page.FillAsync("input[name='GorselUrl']", "/uploads/test.jpg");
            await Page.FillAsync("textarea[name='Ozet']", "Kýsa özet");
            await Page.FillAsync("textarea[name='Icerik']", "Bu bir E2E test haberidir.");
            if (!await Page.IsCheckedAsync("input[name='IsActive']"))
            {
                await Page.CheckAsync("input[name='IsActive']");
            }
            await Page.ClickAsync("button[type='submit']");
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            // Assert: Haber listesinde görünmeli.
            var pageText = await Page.InnerTextAsync("body");
            Assert.Contains(testBaslik, pageText);
        }

        [Fact]
        public async Task Admin_CanDelete_Haber_From_List()
        {
            // Arrange: Admin giriþ yap ve yeni haber oluþtur.
            await LoginAsAdminAsync();
            await Page.GotoAsync($"{BaseUrl}/Admin/Haberler/Create");
            var testBaslik = "Silinecek Haber - " + Guid.NewGuid();
            await Page.FillAsync("input[name='Baslik']", testBaslik);
            await Page.FillAsync("input[name='Kategori']", "E2E");
            await Page.FillAsync("input[name='YayinTarihi']", DateTime.Now.ToString("yyyy-MM-ddTHH:mm"));
            await Page.FillAsync("input[name='GorselUrl']", "/uploads/test.jpg");
            await Page.FillAsync("textarea[name='Ozet']", "Kýsa özet");
            await Page.FillAsync("textarea[name='Icerik']", "Silme testi");
            await Page.ClickAsync("button[type='submit']");
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            // Act: Sil butonuna týkla ve onayla.
            var row = Page.Locator("tr", new() { HasText = testBaslik });
            await row.Locator("a[title='Sil']").ClickAsync();
            await Page.Locator("button.swal2-confirm").ClickAsync();
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            // Assert: Kayýt listeden kaybolmalý.
            var pageText = await Page.InnerTextAsync("body");
            Assert.DoesNotContain(testBaslik, pageText);
        }
    }
}
