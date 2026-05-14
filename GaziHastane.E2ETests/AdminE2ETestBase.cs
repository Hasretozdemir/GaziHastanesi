using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Threading.Tasks;

namespace GaziHastane.E2ETests
{
    public abstract class AdminE2ETestBase : PageTest
    {
        protected string BaseUrl => TestSettings.BaseUrl;
        protected string AdminEmail => TestSettings.AdminEmail;
        protected string AdminPassword => TestSettings.AdminPassword;

        protected async Task LoginAsAdminAsync()
        {
            await Page.GotoAsync($"{BaseUrl}/Admin/Auth/Login");
            await Page.FillAsync("input[name='email']", AdminEmail);
            await Page.FillAsync("input[name='sifre']", AdminPassword);
            await Page.ClickAsync("button[type='submit']");
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }
    }
}
