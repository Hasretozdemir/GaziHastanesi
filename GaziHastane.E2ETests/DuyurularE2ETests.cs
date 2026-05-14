using Microsoft.Playwright;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GaziHastane.E2ETests
{
    public class DuyurularE2ETests : AdminE2ETestBase
    {
        [Fact]
        public async Task Admin_CanCreate_And_View_Duyuru()
        {
            // Arrange: Admin giriþini tamamla.
            await LoginAsAdminAsync();

            // Act: Yeni duyuru oluþtur.
            await Page.GotoAsync($"{BaseUrl}/Admin/Duyurular/Create");
            var testBaslik = "E2E Duyuru - " + Guid.NewGuid();
            await Page.FillAsync("input[name='Baslik']", testBaslik);
            await Page.FillAsync("textarea[name='Icerik']", "E2E duyuru içeriði");
            await Page.FillAsync("input[name='GorselUrl']", "/uploads/duyuru.jpg");
            await Page.FillAsync("input[name='YayinTarihi']", DateTime.Now.ToString("yyyy-MM-ddTHH:mm"));
            if (!await Page.IsCheckedAsync("input[name='IsActive']"))
            {
                await Page.CheckAsync("input[name='IsActive']");
            }
            await Page.ClickAsync("button[type='submit']");
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            // Assert: Duyuru listesinde görünmeli.
            var pageText = await Page.InnerTextAsync("body");
            Assert.Contains(testBaslik, pageText);
        }

        [Fact]
        public async Task Admin_CanDelete_Duyuru_From_List()
        {
            // Arrange: Admin giriþ yap ve duyuru oluþtur.
            await LoginAsAdminAsync();
            await Page.GotoAsync($"{BaseUrl}/Admin/Duyurular/Create");
            var testBaslik = "Silinecek Duyuru - " + Guid.NewGuid();
            await Page.FillAsync("input[name='Baslik']", testBaslik);
            await Page.FillAsync("textarea[name='Icerik']", "Silme testi");
            await Page.FillAsync("input[name='YayinTarihi']", DateTime.Now.ToString("yyyy-MM-ddTHH:mm"));
            await Page.ClickAsync("button[type='submit']");
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            // Act: Silme iþlemini tetikle ve onayla.
            var row = Page.Locator("tr", new() { HasText = testBaslik });
            await row.Locator("a[title='Sil']").ClickAsync();
            await Page.Locator("button.swal2-confirm").ClickAsync();
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            // Assert: Duyuru listeden kalkmalý.
            var pageText = await Page.InnerTextAsync("body");
            Assert.DoesNotContain(testBaslik, pageText);
        }
    }
}
