using GaziHastane.Areas.Admin.Controllers;
using GaziHastane.Areas.Admin.Controllers;
using GaziHastane.Data;
using GaziHastane.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GaziHastane.UnitTests
{
    public class HaberlerControllerTests
    {
        [Fact]
        public void Create_ValidModel_RedirectsToIndex_AndAddsHaber()
        {
            // Arrange: Mock Haberler DbSet ve context ayarla.
            var data = new List<Haber>();
            var mockSet = BuildMockDbSet(data);
            mockSet.Setup(m => m.Add(It.IsAny<Haber>()))
                .Callback<Haber>(haber => data.Add(haber))
                .Returns((EntityEntry<Haber>)null!);

            var mockContext = BuildContext(mockSet);
            var controller = new HaberlerController(mockContext.Object);
            var yeniHaber = new Haber { Id = 1, Baslik = "Test Haber", Ozet = "Ozet", Kategori = "Genel", GorselUrl = "/img.jpg", IsActive = true };

            // Act: Create çaðrýsý yap.
            var result = controller.Create(yeniHaber) as RedirectToActionResult;

            // Assert: Yönlendirme, ekleme ve kayýt iþlemleri doðrulansýn.
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Single(data);
            Assert.Equal("Test Haber", data.Single().Baslik);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Create_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange: ModelState hatalý olacak þekilde controller hazýrla.
            var data = new List<Haber>();
            var mockSet = BuildMockDbSet(data);
            var mockContext = BuildContext(mockSet);
            var controller = new HaberlerController(mockContext.Object);
            controller.ModelState.AddModelError("Baslik", "Baþlýk zorunludur");
            var hataliHaber = new Haber { Id = 2, Ozet = "Eksik", Kategori = "Genel", GorselUrl = "/img.jpg" };

            // Act: Create çaðrýsý yap.
            var result = controller.Create(hataliHaber) as ViewResult;

            // Assert: ViewResult ve model dönmeli, veri eklenmemeli.
            Assert.NotNull(result);
            Assert.Equal(hataliHaber, result.Model);
            Assert.Empty(data);
            mockContext.Verify(c => c.SaveChanges(), Times.Never);
        }

        [Fact]
        public void Edit_ValidModel_RedirectsToIndex_AndUpdatesHaber()
        {
            // Arrange: Update iþlemi için mock set ve context kur.
            var data = new List<Haber>();
            var mockSet = BuildMockDbSet(data);
            mockSet.Setup(m => m.Update(It.IsAny<Haber>()))
                .Returns((EntityEntry<Haber>)null!);

            var mockContext = BuildContext(mockSet);
            var controller = new HaberlerController(mockContext.Object);
            var guncelHaber = new Haber { Id = 5, Baslik = "Güncel Haber", Ozet = "Ozet", Kategori = "Güncel", GorselUrl = "/img.jpg" };

            // Act: Edit çaðrýsý yap.
            var result = controller.Edit(guncelHaber) as RedirectToActionResult;

            // Assert: Update ve SaveChanges çaðrýlarý doðrulansýn.
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            mockSet.Verify(m => m.Update(It.Is<Haber>(h => h.Id == 5 && h.Baslik == "Güncel Haber")), Times.Once);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Edit_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange: ModelState hatalý düzenleme senaryosu.
            var data = new List<Haber>();
            var mockSet = BuildMockDbSet(data);
            var mockContext = BuildContext(mockSet);
            var controller = new HaberlerController(mockContext.Object);
            controller.ModelState.AddModelError("Baslik", "Baþlýk zorunludur");
            var guncelHaber = new Haber { Id = 6, Ozet = "Ozet", Kategori = "Genel", GorselUrl = "/img.jpg" };

            // Act: Edit çaðrýsý yap.
            var result = controller.Edit(guncelHaber) as ViewResult;

            // Assert: ViewResult dönmeli ve kayýt iþlemi yapýlmamalý.
            Assert.NotNull(result);
            Assert.Equal(guncelHaber, result.Model);
            mockSet.Verify(m => m.Update(It.IsAny<Haber>()), Times.Never);
            mockContext.Verify(c => c.SaveChanges(), Times.Never);
        }

        [Fact]
        public void Delete_RemovesHaber_AndRedirectsToIndex()
        {
            // Arrange: Silinecek haber ve mock context hazýrla.
            var mevcutHaber = new Haber { Id = 10, Baslik = "Silinecek" };
            var data = new List<Haber> { mevcutHaber };
            var mockSet = BuildMockDbSet(data);
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns((object[] ids) => data.SingleOrDefault(h => h.Id == (int)ids[0]));
            mockSet.Setup(m => m.Remove(It.IsAny<Haber>()))
                .Callback<Haber>(haber => data.Remove(haber))
                .Returns((EntityEntry<Haber>)null!);

            var mockContext = BuildContext(mockSet);
            var controller = new HaberlerController(mockContext.Object);

            // Act: Delete çaðrýsý yap.
            var result = controller.Delete(10) as RedirectToActionResult;

            // Assert: Silme ve yönlendirme doðrulansýn.
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Empty(data);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        private static Mock<DbSet<Haber>> BuildMockDbSet(List<Haber> data)
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<Haber>>();

            mockSet.As<IQueryable<Haber>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Haber>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Haber>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Haber>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return mockSet;
        }

        private static Mock<GaziHastaneContext> BuildContext(Mock<DbSet<Haber>> mockSet)
        {
            var options = new DbContextOptionsBuilder<GaziHastaneContext>().Options;
            var mockContext = new Mock<GaziHastaneContext>(options);
            mockContext.Setup(c => c.Haberler).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChanges()).Returns(1);
            return mockContext;
        }
    }
}
