using GaziHastane.Areas.Admin.Controllers;
using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GaziHastane.UnitTests
{
    public class EtkinliklerControllerTests
    {
        [Fact]
        public void Create_ValidModel_RedirectsToIndex_AndAddsEtkinlik()
        {
            // Arrange: Mock Etkinlikler DbSet ve context hazýrla.
            var data = new List<Etkinlik>();
            var mockSet = BuildMockDbSet(data);
            mockSet.Setup(m => m.Add(It.IsAny<Etkinlik>()))
                .Callback<Etkinlik>(etkinlik => data.Add(etkinlik))
                .Returns((EntityEntry<Etkinlik>)null!);

            var mockContext = BuildContext(mockSet);
            var env = new Mock<IWebHostEnvironment>();
            var controller = new EtkinliklerController(env.Object, mockContext.Object);
            var yeniEtkinlik = new Etkinlik
            {
                Id = 1,
                Baslik = "Test Etkinlik",
                EtkinlikTipi = "Sempozyum",
                Tarih = DateTime.Today,
                SaatAraligi = "09:00 - 17:00",
                Konum = "Salon A",
                IsActive = true
            };

            // Act: Create çaðrýsý yap.
            var result = controller.Create(yeniEtkinlik) as RedirectToActionResult;

            // Assert: Yönlendirme ve ekleme doðrulansýn.
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Single(data);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Create_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange: Hatalý model ile controller hazýrla.
            var data = new List<Etkinlik>();
            var mockSet = BuildMockDbSet(data);
            var mockContext = BuildContext(mockSet);
            var env = new Mock<IWebHostEnvironment>();
            var controller = new EtkinliklerController(env.Object, mockContext.Object);
            controller.ModelState.AddModelError("Baslik", "Baþlýk zorunludur");
            var hataliEtkinlik = new Etkinlik { Id = 2, EtkinlikTipi = "Eðitim", Konum = "Salon B" };

            // Act: Create çaðrýsý yap.
            var result = controller.Create(hataliEtkinlik) as ViewResult;

            // Assert: ViewResult dönmeli ve kayýt olmamalý.
            Assert.NotNull(result);
            Assert.Equal(hataliEtkinlik, result.Model);
            Assert.Empty(data);
            mockContext.Verify(c => c.SaveChanges(), Times.Never);
        }

        [Fact]
        public void Edit_ValidModel_RedirectsToIndex_AndUpdatesEtkinlik()
        {
            // Arrange: Mevcut etkinliði listeye ekle ve Find setup et.
            var mevcutEtkinlik = new Etkinlik
            {
                Id = 3,
                Baslik = "Eski Baþlýk",
                EtkinlikTipi = "Kongre",
                Tarih = DateTime.Today,
                SaatAraligi = "10:00 - 12:00",
                Konum = "Salon C",
                IsActive = true
            };

            var data = new List<Etkinlik> { mevcutEtkinlik };
            var mockSet = BuildMockDbSet(data);
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns((object[] ids) => data.SingleOrDefault(e => e.Id == (int)ids[0]));

            var mockContext = BuildContext(mockSet);
            var env = new Mock<IWebHostEnvironment>();
            var controller = new EtkinliklerController(env.Object, mockContext.Object);
            var guncelEtkinlik = new Etkinlik
            {
                Id = 3,
                Baslik = "Yeni Baþlýk",
                EtkinlikTipi = "Kongre",
                Tarih = DateTime.Today.AddDays(1),
                SaatAraligi = "13:00 - 15:00",
                Konum = "Salon D",
                IsActive = false
            };

            // Act: Edit çaðrýsý yap.
            var result = controller.Edit(guncelEtkinlik) as RedirectToActionResult;

            // Assert: Yönlendirme ve güncelleme doðrulansýn.
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Yeni Baþlýk", mevcutEtkinlik.Baslik);
            Assert.Equal("Salon D", mevcutEtkinlik.Konum);
            Assert.False(mevcutEtkinlik.IsActive);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Edit_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange: Hatalý model ile edit senaryosu.
            var data = new List<Etkinlik>();
            var mockSet = BuildMockDbSet(data);
            var mockContext = BuildContext(mockSet);
            var env = new Mock<IWebHostEnvironment>();
            var controller = new EtkinliklerController(env.Object, mockContext.Object);
            controller.ModelState.AddModelError("Baslik", "Baþlýk zorunludur");
            var guncelEtkinlik = new Etkinlik { Id = 4, EtkinlikTipi = "Eðitim", Konum = "Salon E" };

            // Act: Edit çaðrýsý yap.
            var result = controller.Edit(guncelEtkinlik) as ViewResult;

            // Assert: ViewResult dönmeli ve kayýt olmamalý.
            Assert.NotNull(result);
            Assert.Equal(guncelEtkinlik, result.Model);
            mockContext.Verify(c => c.SaveChanges(), Times.Never);
        }

        [Fact]
        public void Delete_RemovesEtkinlik_AndRedirectsToIndex()
        {
            // Arrange: Silinecek etkinlik ve mock context hazýrla.
            var mevcutEtkinlik = new Etkinlik { Id = 10, Baslik = "Silinecek" };
            var data = new List<Etkinlik> { mevcutEtkinlik };
            var mockSet = BuildMockDbSet(data);
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns((object[] ids) => data.SingleOrDefault(e => e.Id == (int)ids[0]));
            mockSet.Setup(m => m.Remove(It.IsAny<Etkinlik>()))
                .Callback<Etkinlik>(etkinlik => data.Remove(etkinlik))
                .Returns((EntityEntry<Etkinlik>)null!);

            var mockContext = BuildContext(mockSet);
            var env = new Mock<IWebHostEnvironment>();
            var controller = new EtkinliklerController(env.Object, mockContext.Object);

            // Act: Delete çaðrýsý yap.
            var result = controller.Delete(10) as RedirectToActionResult;

            // Assert: Silme iþlemi ve yönlendirme doðrulansýn.
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Empty(data);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        private static Mock<DbSet<Etkinlik>> BuildMockDbSet(List<Etkinlik> data)
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<Etkinlik>>();

            mockSet.As<IQueryable<Etkinlik>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Etkinlik>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Etkinlik>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Etkinlik>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return mockSet;
        }

        private static Mock<GaziHastaneContext> BuildContext(Mock<DbSet<Etkinlik>> mockSet)
        {
            var options = new DbContextOptionsBuilder<GaziHastaneContext>().Options;
            var mockContext = new Mock<GaziHastaneContext>(options);
            mockContext.Setup(c => c.Etkinlikler).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChanges()).Returns(1);
            return mockContext;
        }
    }
}
