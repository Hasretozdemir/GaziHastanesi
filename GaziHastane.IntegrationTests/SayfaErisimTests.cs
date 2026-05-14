using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;

namespace GaziHastane.IntegrationTests
{
    public class SayfaErisimTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public SayfaErisimTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")] // Anasayfa
        [InlineData("/Randevu/Giris")] // Randevu sayfası
        [InlineData("/Sonuc/Giris")] // Tahlil sonuç sayfası
        public async Task GecerliSayfalar_Http200_BasariliDonmeli(string url)
        {
            // Arrange
            var client = _factory.CreateClient(); // Sanal bir tarayıcı (client) oluştur

            // Act
            var response = await client.GetAsync(url); // Sayfaya git

            // Assert
            response.EnsureSuccessStatusCode(); // Gelen cevap Http 200 (Başarılı) mü? Kontrol et.
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}