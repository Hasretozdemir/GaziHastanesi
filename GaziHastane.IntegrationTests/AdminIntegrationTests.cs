using Microsoft.AspNetCore.Mvc.Testing;
using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GaziHastane.IntegrationTests
{
    public class AdminIntegrationTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public AdminIntegrationTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Post_HaberlerCreate_PersistsRecord()
        {
            // Arrange: Haberler için form verilerini hazýrla.
            var baslik = $"Integration Haber {Guid.NewGuid()}";
            var formData = new Dictionary<string, string>
            {
                ["Baslik"] = baslik,
                ["Ozet"] = "Kýsa özet",
                ["Kategori"] = "Genel",
                ["GorselUrl"] = "/test.jpg",
                ["Icerik"] = "Test içerik",
                ["YayinTarihi"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm"),
                ["IsActive"] = "true"
            };

            // Act: Create endpoint'ine POST isteði at.
            var response = await _client.PostAsync("/Admin/Haberler/Create", new FormUrlEncodedContent(formData));

            // Assert: Redirect ve DB kaydý doðrula.
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GaziHastaneContext>();
            var kayit = await context.Haberler.SingleOrDefaultAsync(h => h.Baslik == baslik);
            Assert.NotNull(kayit);
        }

        [Fact]
        public async Task Get_HaberlerIndex_RendersInsertedRecord()
        {
            // Arrange: Veritabanýna haber ekle.
            var baslik = $"Liste Haber {Guid.NewGuid()}";
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<GaziHastaneContext>();
                context.Haberler.Add(new Haber { Baslik = baslik, Ozet = "Özet", Kategori = "Genel", GorselUrl = "/test.jpg" });
                await context.SaveChangesAsync();
            }

            // Act: Index sayfasýný çaðýr.
            var response = await _client.GetAsync("/Admin/Haberler/Index");
            var html = await response.Content.ReadAsStringAsync();

            // Assert: HTML içeriðinde eklenen baþlýk yer almalý.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains(baslik, html);
        }

        [Fact]
        public async Task Post_DuyurularCreate_PersistsRecord()
        {
            // Arrange: Duyuru form verileri hazýrla.
            var baslik = $"Integration Duyuru {Guid.NewGuid()}";
            var formData = new Dictionary<string, string>
            {
                ["Baslik"] = baslik,
                ["Icerik"] = "Duyuru içeriði",
                ["GorselUrl"] = "/duyuru.jpg",
                ["YayinTarihi"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm"),
                ["IsActive"] = "true"
            };

            // Act: Create endpoint'ine POST isteði at.
            var response = await _client.PostAsync("/Admin/Duyurular/Create", new FormUrlEncodedContent(formData));

            // Assert: Redirect ve DB kaydý doðrula.
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GaziHastaneContext>();
            var kayit = await context.Duyurular.SingleOrDefaultAsync(d => d.Baslik == baslik);
            Assert.NotNull(kayit);
        }

        [Fact]
        public async Task Get_DuyurularIndex_RendersInsertedRecord()
        {
            // Arrange: Veritabanýna duyuru ekle.
            var baslik = $"Liste Duyuru {Guid.NewGuid()}";
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<GaziHastaneContext>();
                context.Duyurular.Add(new Duyuru { Baslik = baslik, Icerik = "Duyuru içeriði" });
                await context.SaveChangesAsync();
            }

            // Act: Index sayfasýna GET isteði at.
            var response = await _client.GetAsync("/Admin/Duyurular/Index");
            var html = await response.Content.ReadAsStringAsync();

            // Assert: Eklenen duyuru sayfada görünmeli.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains(baslik, html);
        }

        [Fact]
        public async Task Post_EtkinliklerCreate_PersistsRecord()
        {
            // Arrange: Etkinlik form verilerini hazýrla.
            var baslik = $"Integration Etkinlik {Guid.NewGuid()}";
            var formData = new Dictionary<string, string>
            {
                ["Baslik"] = baslik,
                ["EtkinlikTipi"] = "Sempozyum",
                ["Tarih"] = DateTime.Today.ToString("yyyy-MM-dd"),
                ["SaatAraligi"] = "09:00 - 11:00",
                ["Konum"] = "Salon X",
                ["Aciklama"] = "Etkinlik açýklamasý",
                ["IsActive"] = "true"
            };

            // Act: Create endpoint'ine POST isteði at.
            var response = await _client.PostAsync("/Admin/Etkinlikler/Create", new FormUrlEncodedContent(formData));

            // Assert: Redirect ve DB kaydý doðrula.
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GaziHastaneContext>();
            var kayit = await context.Etkinlikler.SingleOrDefaultAsync(e => e.Baslik == baslik);
            Assert.NotNull(kayit);
        }

        [Fact]
        public async Task Get_EtkinliklerIndex_RendersInsertedRecord()
        {
            // Arrange: Veritabanýna etkinlik ekle.
            var baslik = $"Liste Etkinlik {Guid.NewGuid()}";
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<GaziHastaneContext>();
                context.Etkinlikler.Add(new Etkinlik
                {
                    Baslik = baslik,
                    EtkinlikTipi = "Kongre",
                    Tarih = DateTime.Today,
                    SaatAraligi = "10:00 - 12:00",
                    Konum = "Salon Y"
                });
                await context.SaveChangesAsync();
            }

            // Act: Index sayfasýna GET isteði at.
            var response = await _client.GetAsync("/Admin/Etkinlikler/Index");
            var html = await response.Content.ReadAsStringAsync();

            // Assert: Eklenen etkinlik sayfada görünmeli.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains(baslik, html);
        }
    }
}
