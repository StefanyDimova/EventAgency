using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.Controllers;
using EventAgency.Web.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventAgency.Services.Tests.Controllers
{
    [TestFixture]
    public class ProductControllerTests
    {
        private Mock<IProductService> mockProductService;
        private Mock<ICategoryService> mockCategoryService;
        private ProductController controller;

        [SetUp]
        public void SetUp()
        {
            this.mockProductService = new Mock<IProductService>();
            this.mockCategoryService = new Mock<ICategoryService>();
            this.controller = new ProductController(
                this.mockProductService.Object,
                this.mockCategoryService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            this.controller?.Dispose();
        }

        [Test]
        public async Task Index_ShouldReturnViewWithProducts_WhenNoExceptionIsThrown()
        {
            List<AllProductsViewModel> testProducts = new List<AllProductsViewModel>
            {
                new AllProductsViewModel { Id = "1", Name = "Product 1" },
                new AllProductsViewModel { Id = "2", Name = "Product 2" }
            };

            this.mockProductService.Setup(x => x.GetAllProductsAsync())
                .ReturnsAsync(testProducts);

            IActionResult result = await this.controller.Index();

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOf<IEnumerable<AllProductsViewModel>>(viewResult.Model);
        }

        [Test]
        public async Task Index_ShouldRedirectToError_WhenExceptionIsThrown()
        {
            this.mockProductService.Setup(x => x.GetAllProductsAsync())
                .ThrowsAsync(new Exception());

            IActionResult result = await this.controller.Index();

            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Error", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
            Assert.AreEqual(500, redirectResult.RouteValues["statusCode"]);
        }

        [Test]
        public async Task BySubCategory_ShouldReturnViewWithFilteredProducts()
        {
            int subCategoryId = 3;
            List<AllProductsViewModel> filtered = new List<AllProductsViewModel>()
            {
                new AllProductsViewModel { Id = "1", Name = "Filtered Product" }
            };

            this.mockProductService.Setup(x => x.GetProductsBySubCategoryIdAsync(subCategoryId))
                .ReturnsAsync(filtered);

            IActionResult result = await this.controller.BySubCategory(subCategoryId);

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Index", viewResult.ViewName);
            Assert.IsInstanceOf<IEnumerable<AllProductsViewModel>>(viewResult.Model);
        }

        [Test]
        public async Task BySubCategory_ShouldRedirectToError_WhenExceptionIsThrown()
        {
            this.mockProductService.Setup(x => x.GetProductsBySubCategoryIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            IActionResult result = await this.controller.BySubCategory(3);

            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Error", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
            Assert.AreEqual(500, redirectResult.RouteValues["statusCode"]);
        }

        [Test]
        public async Task Details_ShouldReturnView_WhenProductExists()
        {
            string productId = "abc";
            ProductDetailsViewModel model = new ProductDetailsViewModel { Id = productId };

            this.mockProductService.Setup(x => x.GetProductDetailsByIdAsync(productId))
                .ReturnsAsync(model);

            IActionResult result = await this.controller.Details(productId);

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOf<ProductDetailsViewModel>(viewResult.Model);
        }

        [Test]
        public async Task Details_ShouldRedirectToNotFound_WhenProductDoesNotExist()
        {
            this.mockProductService.Setup(x => x.GetProductDetailsByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ProductDetailsViewModel)null);

            IActionResult result = await this.controller.Details("invalid");

            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Error", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
            Assert.AreEqual(404, redirectResult.RouteValues["statusCode"]);
        }

        [Test]
        public async Task Details_ShouldRedirectToServerError_WhenExceptionIsThrown()
        {
            this.mockProductService.Setup(x => x.GetProductDetailsByIdAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            IActionResult result = await this.controller.Details("abc");

            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Error", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
            Assert.AreEqual(500, redirectResult.RouteValues["statusCode"]);
        }
    }
}
