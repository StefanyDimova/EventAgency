using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Admin;
using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Admin.ProductManagement;
using MockQueryable;
using Moq;
using MockQueryable.Moq;

namespace EventAgency.Services.Tests
{
    [TestFixture]
    public class ProductManagementServiceTests
    {
        private Mock<IProductRepository> productRepoMock;
        private Mock<ICategoryService> categoryServiceMock;
        private Mock<IProductService> productServiceMock;
        private ProductManagementService productManagementService;

        [SetUp]
        public void Setup()
        {
            this.productRepoMock = new Mock<IProductRepository>();
            this.categoryServiceMock = new Mock<ICategoryService>();
            this.productServiceMock = new Mock<IProductService>();

            this.productManagementService = new ProductManagementService(
                this.productRepoMock.Object,
                this.categoryServiceMock.Object,
                this.productServiceMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        // Tests for GetProductManagementDataAsync method

        [Test]
        public async Task GetProductManagementDataAsync_ReturnsAllProductsMappedCorrectly()
        {
            List<Product> products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Product",
                    Description = "Test Description",
                    Price = 19.99m,
                    Quantity = 5,
                    Category = new Category { Name = "Category A" },
                    SubCategory = new Category { Name = "SubCategory A" },
                    IsDeleted = false,
                    ImageUrl = null
                },
                 new Product
                 {
                     Id = Guid.NewGuid(),
                     Name = "Second Product",
                     Description = "Another Description",
                     Price = 10.00m,
                     Quantity = 2,
                     Category = new Category { Name = "Category B" },
                     SubCategory = new Category { Name = "SubCategory B" },
                     IsDeleted = true,
                     ImageUrl = "custom.jpg"
                 }
            };

            IQueryable<Product> productQuery = products.BuildMock();

            this.productRepoMock
                .Setup(pr => pr.GetAllAttached())
                .Returns(productQuery);

            IEnumerable<ProductManagementIndexViewModel> result =
            await this.productManagementService.GetProductManagementDataAsync();

            Assert.AreEqual(2, result.Count(), "Expected two products in the result");

            ProductManagementIndexViewModel firstProduct = result.First();

            Assert.That(firstProduct.Name, Is.EqualTo("Test Product"));
            Assert.That(firstProduct.Description, Is.EqualTo("Test Description"));
            Assert.That(firstProduct.Category, Is.EqualTo("Category A"));
            Assert.That(firstProduct.SubCategory, Is.EqualTo("SubCategory A"));
            Assert.That(firstProduct.ImageUrl, Is.EqualTo("/images/no-image.jpg"));
        }

        [Test]
        public async Task GetProductManagementDataAsync_WithNoProducts_ReturnsEmptyCollection()
        {
            List<Product> products = new List<Product>();
            IQueryable<Product> productQuery = products.BuildMock();

            this.productRepoMock
                .Setup(repo => repo.GetAllAttached())
                .Returns(productQuery);

            IEnumerable<ProductManagementIndexViewModel> result =
                await this.productManagementService.GetProductManagementDataAsync();

            Assert.IsEmpty(result, "Expected an empty collection when no products exist");
            Assert.AreEqual(0, result.Count(), "Expected count to be zero when no products exist");
            Assert.IsNotNull(result, "Result should not be null even when no products exist");
        }

        // Tests for AddProductAsync method

        [Test]
        public async Task AddProductAsync_WithValidInput_CallsRepositoryWithCorrectProduct()
        {
            // Arrange
            AddProductInputModel inputModel = new AddProductInputModel()
            {
                Name = "Test Product",
                Description = "Test Description",
                ImageUrl = "test.jpg",
                Price = 29.99m,
                Quantity = 10,
                CategoryId = 1,
                SubCategoryId = 2
            };

            Product capturedProduct = null;

            this.productRepoMock
                .Setup(r => r.AddAsync(It.IsAny<Product>()))
                .Callback<Product>(p => capturedProduct = p)
                .Returns(Task.CompletedTask);

            await this.productManagementService.AddProductAsync(inputModel);

            this.productRepoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
            Assert.That(capturedProduct, Is.Not.Null);
            Assert.That(capturedProduct.Name, Is.EqualTo("Test Product"));
            Assert.That(capturedProduct.Description, Is.EqualTo("Test Description"));
            Assert.That(capturedProduct.ImageUrl, Is.EqualTo("test.jpg"));
            Assert.That(capturedProduct.Price, Is.EqualTo(29.99m));
            Assert.That(capturedProduct.Quantity, Is.EqualTo(10));
            Assert.That(capturedProduct.CategoryId, Is.EqualTo(1));
            Assert.That(capturedProduct.SubCategoryId, Is.EqualTo(2));
        }

        // Tests for GetEditableProductByIdAsync method

        [Test]
        public async Task GetEditableProductByIdAsync_WithInvalidGuid_ReturnsNull()
        {
            string invalidId = "not-a-guid";

            ProductEditInputModel result =
                await this.productManagementService.GetEditableProductByIdAsync(invalidId);

            Assert.IsNull(result, "Expected null when id is not a valid GUID");
        }

        [Test]
        public async Task GetEditableProductByIdAsync_WithValidGuidButNoProduct_ReturnsNull()
        {
            Guid productId = Guid.NewGuid();
            IQueryable<Product> emptyQuery = new List<Product>().BuildMock();

            this.productRepoMock
                .Setup(r => r.GetAllAttached())
                .Returns(emptyQuery);

            ProductEditInputModel result =
                await this.productManagementService.GetEditableProductByIdAsync(productId.ToString());

            Assert.IsNull(result, "Expected null when no product exists for the given id");
        }

        [Test]
        public async Task GetEditableProductByIdAsync_WithValidProduct_ReturnsPopulatedModel()
        {
            // Arrange
            Guid productId = Guid.NewGuid();

            List<Product> products = new List<Product>
            {
                new Product
                {
                    Id = productId,
                    Name = "Product",
                    Description = "Description",
                    Price = 10.5m,
                    Quantity = 5,
                    ImageUrl = null,
                    CategoryId = 1,
                    SubCategoryId = 2
                }
            };

            IQueryable<Product> productQuery = products.BuildMock();

            this.productRepoMock
                .Setup(r => r.GetAllAttached())
                .Returns(productQuery);

            List<AddProductCategoryDropDownModel> mockCategories = new List<AddProductCategoryDropDownModel>
            {
                new AddProductCategoryDropDownModel { Id = 1, Name = "Category A" }
            };

            this.categoryServiceMock
                .Setup(c => c.GetCategoriesDropdownDataAsync())
                .ReturnsAsync(mockCategories);

            List<AddProductCategoryDropDownModel> mockSubCategories = new List<AddProductCategoryDropDownModel>
            {
                new AddProductCategoryDropDownModel { Id = 2, Name = "SubCategory B" }
            };

            this.categoryServiceMock
                .Setup(c => c.GetSubCategoriesDropdownDataAsync(1))
                .ReturnsAsync(mockSubCategories);

            ProductEditInputModel result =
                await this.productManagementService.GetEditableProductByIdAsync(productId.ToString());

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Product"));
            Assert.That(result.ImageUrl, Is.EqualTo("/images/no-image.jpg")); // Или каквото е твоето NoImageUrl
            Assert.That(result.Categories.Count(), Is.EqualTo(1));
            Assert.That(result.SubCategories.Count(), Is.EqualTo(1));
        }

        // Tests for EditProductAsync method

        [Test]
        public async Task EditProductAsync_ProductNotFound_ReturnsFalse()
        {
            string productId = Guid.NewGuid().ToString();

            ProductEditInputModel productEditInputModel = new ProductEditInputModel
            {
                Id = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 20.00m,
                Quantity = 15,
                ImageUrl = null,
                CategoryId = 1,
                SubCategoryId = 2
            };

            IQueryable<Product> emptyQuery = new List<Product>().BuildMock();
            this.productRepoMock
                .Setup(pr => pr.GetAllAttached())
                .Returns(emptyQuery);

            bool result = await this.productManagementService.EditProductAsync(productEditInputModel);
            Assert.IsFalse(result, "Expected false when product does not exist");
        }

        [Test]
        public async Task EditProductAsync_ProductExists_UpdatesProductAndReturnsTrue()
        {
            Guid productId = Guid.NewGuid();

            Product existingProduct = new Product
            {
                Id = productId,
                Name = "Old Product",
                Description = "Old Description",
                Price = 10.00m,
                Quantity = 5,
                ImageUrl = null,
                CategoryId = 1,
                SubCategoryId = 2
            };

            ProductEditInputModel productEditInputModel = new ProductEditInputModel
            {
                Id = productId.ToString(),
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 20.00m,
                Quantity = 15,
                ImageUrl = null,
                CategoryId = 1,
                SubCategoryId = 2
            };

            this.productRepoMock
                .Setup(pr => pr.GetByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            this.productRepoMock
                .Setup(pr => pr.UpdateAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            bool result = await this.productManagementService.EditProductAsync(productEditInputModel);

            Assert.IsTrue(result, "Expected true when product is successfully updated");
            this.productRepoMock.Verify(pr => pr.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }

        // Tests for DeleteOrRestoreProductAsync method

        [Test]
        public async Task DeleteOrRestoreProductAsync_NullOrEmptyId_ReturnsFalseTuple()
        {
            Tuple<bool, bool> result1 = await this.productManagementService.DeleteOrRestoreProductAsync(null);
            Tuple<bool, bool> result2 = await this.productManagementService.DeleteOrRestoreProductAsync("");

            Assert.IsFalse(result1.Item1);
            Assert.IsFalse(result1.Item2);
            Assert.IsFalse(result2.Item1);
            Assert.IsFalse(result2.Item2);
        }

        [Test]
        public async Task DeleteOrRestoreProductAsync_ProductNotFound_ReturnsFalseTuple()
        {
            List<Product> products = new List<Product>();
            IQueryable<Product> mockProducts = products.BuildMock();

            this.productRepoMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockProducts);

            Tuple<bool, bool> result = await this.productManagementService
                .DeleteOrRestoreProductAsync(Guid.NewGuid().ToString());

            Assert.That(result.Item1, Is.False);
            Assert.That(result.Item2, Is.False);
        }

        [Test]
        public async Task DeleteOrRestoreProductAsync_ProductFoundAndDeleted_ReturnsTrueAndIsRestoredTrue()
        {
            Guid productId = Guid.NewGuid();

            List<Product> products = new List<Product>()
            {
                new Product 
                {
                    Id = productId,
                    Name = "Test Product",
                    IsDeleted = true
                }
            };

            IQueryable<Product> mockProducts = products.BuildMock();

            this.productRepoMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockProducts);

            this.productRepoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            Tuple<bool, bool> result = await this.productManagementService
                .DeleteOrRestoreProductAsync(productId.ToString());

            Assert.That(result.Item1, Is.True);
            Assert.That(result.Item2, Is.True);
            Assert.That(products.First().IsDeleted, Is.False);
        }

        [Test]
        public async Task DeleteOrRestoreProductAsync_ProductFoundAndActive_ReturnsTrueAndIsRestoredFalse()
        {
            Guid productId = Guid.NewGuid();
            List<Product> products = new List<Product>()
            {
                new Product
                {
                    Id = productId,
                    Name = "Test Product",
                    IsDeleted = false
                }
            };

            IQueryable<Product> mockProducts = products.BuildMock();

            this.productRepoMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockProducts);

            this.productRepoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            Tuple<bool, bool> result = await this.productManagementService
                .DeleteOrRestoreProductAsync(productId.ToString());

            Assert.That(result.Item1, Is.True);
            Assert.That(result.Item2, Is.False);
            Assert.That(products.First().IsDeleted, Is.True); // продуктът вече е маркиран като изтрит
        }

    }
}
