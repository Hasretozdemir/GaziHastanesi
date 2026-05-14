using Microsoft.Playwright;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GaziHastane.E2ETests
{
    public class EtkinliklerE2ETests : AdminE2ETestBase
    {
        [Fact]
        public async Task Admin_CanCreate_And_View_Etkinlik()
        {
            // Arrange: Admin giriþini tamamla.
            await LoginAsAdminAsync();

            // Act: Yeni etkinlik oluþtur.
            await Page.GotoAsync($"{BaseUrl}/Admin/Etkinlikler/Create");
            var testBaslik = "E2E Etkinlik - " + Guid.NewGuid();
            await Page.FillAsync("input[name='Baslik']", testBaslik);
            await Page.FillAsync("input[name='EtkinlikTipi']", "Sempozyum");
            await Page.FillAsync("input[name='Tarih']", DateTime.Today.ToString("yyyy-MM-dd"));
            await Page.FillAsync("input[name='SaatAraligi']", "09:00 - 17:00");
            await Page.FillAsync("input[name='Konum']", "Salon A");
            await Page.FillAsync("textarea[name='Aciklama']", "E2E etkinlik açýklamasý");
            await Page.FillAsync("textarea[name='ModalIcerik']", "E2E modal içerik");
            await Page.SetInputFilesAsync("input[name='GorselDosya']", new FilePayload
            {
                Name = "test.png",
                MimeType = "image/png",
                Buffer = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }
            });
            if (!await Page.IsCheckedAsync("input[name='IsActive']"))
            {
                await Page.CheckAsync("input[name='IsActive']");
            }
            await Page.ClickAsync("button[type='submit']");
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            // Assert: Etkinlik listesinde görünmeli.
            var pageText = await Page.InnerTextAsync("body");
            Assert.Contains(testBaslik, pageText);
        }

        [Fact]
        public async Task Admin_CanDelete_Etkinlik_From_List()
        {
            // Arrange: Admin giriþ yap ve etkinlik oluþtur.
            await LoginAsAdminAsync();
            await Page.GotoAsync($"{BaseUrl}/Admin/Etkinlikler/Create");
            var testBaslik = "Silinecek Etkinlik - " + Guid.NewGuid();
            await Page.FillAsync("input[name='Baslik']", testBaslik);
            await Page.FillAsync("input[name='EtkinlikTipi']", "Eðitim");
            await Page.FillAsync("input[name='Tarih']", DateTime.Today.ToString("yyyy-MM-dd"));
            await Page.FillAsync("input[name='SaatAraligi']", "10:00 - 11:00");
            await Page.FillAsync("input[name='Konum']", "Salon B");
            await Page.ClickAsync("button[type='submit']");
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            // Act: Silme iþlemini tetikle ve onayla.
            var row = Page.Locator("tr", new() { HasText = testBaslik });
            await row.Locator("a[title='Sil']").ClickAsync();
            await Page.Locator("button.swal2-confirm").ClickAsync();
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            // Assert: Etkinlik listeden kalkmalý.
            var pageText = await Page.InnerTextAsync("body");
            Assert.DoesNotContain(testBaslik, pageText);
        }
    }
}
