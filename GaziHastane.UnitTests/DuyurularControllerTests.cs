using GaziHastane.Controllers;
using AdminDuyurularController = GaziHastane.Areas.Admin.Controllers.DuyurularController;
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
    public class DuyurularControllerTests
    {
        [Fact]
        public void Create_ValidModel_RedirectsToIndex_AndAddsDuyuru()
        {
            // Arrange: Mock Duyurular DbSet ve context oluştur.
            var data = new List<Duyuru>();
            var mockSet = BuildMockDbSet(data);
            mockSet.Setup(m => m.Add(It.IsAny<Duyuru>()))
                .Callback<Duyuru>(duyuru => data.Add(duyuru))
                .Returns((EntityEntry<Duyuru>)null!);

            var mockContext = BuildContext(mockSet);
            var controller = new AdminDuyurularController(mockContext.Object);
            var yeniDuyuru = new Duyuru { Id = 1, Baslik = "Test Duyuru", Icerik = "İçerik", IsActive = true };

            // Act: Create çağrısı yap.
            var result = controller.Create(yeniDuyuru) as RedirectToActionResult;

            // Assert: Yönlendirme ve kayıt işlemleri doğrulansın.
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Single(data);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Create_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange: Hatalı ModelState ile controller hazırla.
            var data = new List<Duyuru>();
            var mockSet = BuildMockDbSet(data);
            var mockContext = BuildContext(mockSet);
            var controller = new AdminDuyurularController(mockContext.Object);
            controller.ModelState.AddModelError("Baslik", "Başlık zorunludur");
            var hataliDuyuru = new Duyuru { Id = 2, Icerik = "Eksik" };

            // Act: Create çağrısı yap.
            var result = controller.Create(hataliDuyuru) as ViewResult;

            // Assert: ViewResult dönmeli, kayıt yapılmamalı.
            Assert.NotNull(result);
            Assert.Equal(hataliDuyuru, result.Model);
            Assert.Empty(data);
            mockContext.Verify(c => c.SaveChanges(), Times.Never);
        }

        [Fact]
        public void Edit_ValidModel_RedirectsToIndex_AndUpdatesDuyuru()
        {
            // Arrange: Update için mock context hazırla.
            var data = new List<Duyuru>();
            var mockSet = BuildMockDbSet(data);
            mockSet.Setup(m => m.Update(It.IsAny<Duyuru>()))
                .Returns((EntityEntry<Duyuru>)null!);

            var mockContext = BuildContext(mockSet);
            var controller = new AdminDuyurularController(mockContext.Object);
            var guncelDuyuru = new Duyuru { Id = 3, Baslik = "Güncel", Icerik = "Yeni içerik" };

            // Act: Edit çağrısı yap.
            var result = controller.Edit(guncelDuyuru) as RedirectToActionResult;

            // Assert: Update ve SaveChanges çağrıları doğrulansın.
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            mockSet.Verify(m => m.Update(It.Is<Duyuru>(d => d.Id == 3 && d.Baslik == "Güncel")), Times.Once);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Edit_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange: Hatalı ModelState ile edit senaryosu hazırla.
            var data = new List<Duyuru>();
            var mockSet = BuildMockDbSet(data);
            var mockContext = BuildContext(mockSet);
            var controller = new AdminDuyurularController(mockContext.Object);
            controller.ModelState.AddModelError("Baslik", "Başlık zorunludur");
            var guncelDuyuru = new Duyuru { Id = 4, Icerik = "Eksik" };

            // Act: Edit çağrısı yap.
            var result = controller.Edit(guncelDuyuru) as ViewResult;

            // Assert: ViewResult dönmeli ve Update yapılmamalı.
            Assert.NotNull(result);
            Assert.Equal(guncelDuyuru, result.Model);
            mockSet.Verify(m => m.Update(It.IsAny<Duyuru>()), Times.Never);
            mockContext.Verify(c => c.SaveChanges(), Times.Never);
        }

        [Fact]
        public void Delete_RemovesDuyuru_AndRedirectsToIndex()
        {
            // Arrange: Silinecek duyuru ile mock context hazırla.
            var mevcutDuyuru = new Duyuru { Id = 10, Baslik = "Silinecek" };
            var data = new List<Duyuru> { mevcutDuyuru };
            var mockSet = BuildMockDbSet(data);
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns((object[] ids) => data.SingleOrDefault(d => d.Id == (int)ids[0]));
            mockSet.Setup(m => m.Remove(It.IsAny<Duyuru>()))
                .Callback<Duyuru>(duyuru => data.Remove(duyuru))
                .Returns((EntityEntry<Duyuru>)null!);

            var mockContext = BuildContext(mockSet);
            var controller = new AdminDuyurularController(mockContext.Object);

            // Act: Delete çağrısı yap.
            var result = controller.Delete(10) as RedirectToActionResult;

            // Assert: Silme işlemi ve yönlendirme doğrulansın.
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Empty(data);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        private static Mock<DbSet<Duyuru>> BuildMockDbSet(List<Duyuru> data)
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<Duyuru>>();

            mockSet.As<IQueryable<Duyuru>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Duyuru>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Duyuru>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Duyuru>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return mockSet;
        }

        private static Mock<GaziHastaneContext> BuildContext(Mock<DbSet<Duyuru>> mockSet)
        {
            var options = new DbContextOptionsBuilder<GaziHastaneContext>().Options;
            var mockContext = new Mock<GaziHastaneContext>(options);
            mockContext.Setup(c => c.Duyurular).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChanges()).Returns(1);
            return mockContext;
        }
    }
}
