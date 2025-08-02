using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core;
using EventAgency.Web.ViewModels.Product;
using MockQueryable;
using Moq;

namespace EventAgency.Services.Tests
{
    [TestFixture]
    public class ProductServiceTests
    {

        private Mock<IProductRepository> productRepositoryMock;
        private ProductService productService;

        [SetUp]
        public void Setup()
        {
            this.productRepositoryMock = new Mock<IProductRepository>();
            this.productService = new ProductService(this.productRepositoryMock.Object);
        }

        // Tests for GetAllProductsAsync method

        [Test]
        public async Task GetAllProductsAsync_ShouldReturnEmptyCollection_WhenNoProductsExist()
        {
            List<Product> products = new List<Product>();
            IQueryable<Product> productsQueryable = products.BuildMock();

            this.productRepositoryMock
                .Setup(pr => pr.GetAllAttached())
                .Returns(productsQueryable);

            IEnumerable<AllProductsViewModel> emptyViewModelCollection = await this.productService.GetAllProductsAsync();

            Assert.IsNotNull(emptyViewModelCollection);
            Assert.AreEqual(products.Count(), emptyViewModelCollection.Count());
        }

        [Test]
        public async Task GetAllProductsAsync_ShouldMapCorrectly_WhenProductsExist()
        {
            List<Product> products = new List<Product>()
            {
                new Product()
                {
                    Id = Guid.Parse("51fdfb4b-858f-4dec-a42c-9622e4627af2"),
                    Name = "Test Product 1",
                    ImageUrl = "https://example.com/image1.jpg",
                    Price = 100.00m,
                    Quantity = 10,
                    SubCategoryId = 1,
                    IsDeleted = false
                }
            };

            IQueryable<Product> productsQueryable = products.BuildMock();

            this.productRepositoryMock
                .Setup(pr => pr.GetAllAttached())
                .Returns(productsQueryable);

            IEnumerable<AllProductsViewModel> viewModelCollection = await this.productService.GetAllProductsAsync();

            Assert.IsNotNull(viewModelCollection);
            Assert.AreEqual(products.Count(), viewModelCollection.Count());

            AllProductsViewModel firstProductViewModel = viewModelCollection.First();
            Assert.AreEqual(products[0].Id.ToString(), firstProductViewModel.Id);
            Assert.AreEqual(products[0].Name, firstProductViewModel.Name);
            Assert.AreEqual(products[0].ImageUrl, firstProductViewModel.ImageUrl);
            Assert.AreEqual(products[0].Price, firstProductViewModel.Price);
            Assert.AreEqual(products[0].Quantity, firstProductViewModel.Quantity);

        }

        [Test]
        public async Task GetAllProductsAsync_ShouldUseDefaultImage_WhenImageUrlIsNullOrEmpty()
        {
            List<Product> products = new List<Product>()
            {
                new Product()
                {
                    Id = Guid.Parse("51fdfb4b-858f-4dec-a42c-9622e4627af2"),
                    Name = "Test Product 1",
                    ImageUrl = null,
                    Price = 100.00m,
                    Quantity = 10,
                    SubCategoryId = 1,
                    IsDeleted = false
                }
            };

            IQueryable<Product> productsQueryable = products.BuildMock();

            this.productRepositoryMock
                .Setup(pr => pr.GetAllAttached())
                .Returns(productsQueryable);

            IEnumerable<AllProductsViewModel> viewModelCollection = await this.productService.GetAllProductsAsync();

            Assert.IsNotNull(viewModelCollection);
            Assert.AreEqual(products.Count(), viewModelCollection.Count());
            AllProductsViewModel firstProductViewModel = viewModelCollection.First();
            Assert.AreEqual(firstProductViewModel.ImageUrl, "/images/no-image.jpg");

        }

        // Tests for GetProductsBySubCategoryIdAsync method

        [Test]
        public async Task GetProductsBySubCategoryIdAsync_ShouldReturnEmpty_WhenNoProducts()
        {
            List<Product> products = new List<Product>();
            IQueryable<Product> productsQueryable = products.BuildMock();

            this.productRepositoryMock
                .Setup(pr => pr.GetAllAttached())
                .Returns(productsQueryable);

            IEnumerable<AllProductsViewModel> emptyViewModelCollection = await this.productService.GetProductsBySubCategoryIdAsync(1);

            Assert.IsNotNull(emptyViewModelCollection);
            Assert.AreEqual(products.Count(), emptyViewModelCollection.Count());
            Assert.IsEmpty(emptyViewModelCollection);

        }

        [Test]
        public async Task GetProductsBySubCategoryIdAsync_ShouldReturnMappedProducts()
        {
            List<Product> products = new List<Product>()
            {
                new Product()
                {
                    Id = Guid.Parse("51fdfb4b-858f-4dec-a42c-9622e4627af2"),
                    Name = "Test Product 1",
                    ImageUrl = "https://example.com/image1.jpg",
                    Price = 100.00m,
                    Quantity = 10,
                    SubCategoryId = 1,
                    IsDeleted = false
                }
            };

            IQueryable<Product> productsQueryable = products.BuildMock();

            this.productRepositoryMock
                .Setup(pr => pr.GetAllAttached())
                .Returns(productsQueryable);

            IEnumerable<AllProductsViewModel> emptyViewModelCollection = await this.productService.GetProductsBySubCategoryIdAsync(1);

            Assert.IsNotNull(emptyViewModelCollection);
            Assert.AreEqual(products.Count(), emptyViewModelCollection.Count());
            AllProductsViewModel firstProductViewModel = emptyViewModelCollection.First();
        }

        [Test]
        public async Task GetProductsBySubCategoryIdAsync_ShouldFilterOutDeletedProducts()
        {
            List<Product> products = new List<Product>()
            {
                new Product()
                {
                    Id = Guid.Parse("51fdfb4b-858f-4dec-a42c-9622e4627af2"),
                    Name = "Test Product 1",
                    ImageUrl = "https://example.com/image1.jpg",
                    Price = 100.00m,
                    Quantity = 10,
                    SubCategoryId = 1,
                    IsDeleted = true
                }
            };

            IQueryable<Product> productsQueryable = products.BuildMock();

            this.productRepositoryMock
                .Setup(pr => pr.GetAllAttached())
                .Returns(productsQueryable);

            IEnumerable<AllProductsViewModel> emptyViewModelCollection = await this.productService.GetProductsBySubCategoryIdAsync(1);

            Assert.IsEmpty(emptyViewModelCollection);
        }

        // Tests for GetProductDetailsByIdAsync method

        [Test]
        public async Task GetProductDetailsByIdAsync_ShouldReturnNull_WhenIdIsInvalidGuid()
        {
            string? invalidId = "invalid-guid";
            ProductDetailsViewModel? detailsViewModel = await this.productService.GetProductDetailsByIdAsync(invalidId);
            Assert.IsNull(detailsViewModel, "Expected null when id is invalid GUID");
        }

        [Test]
        public async Task GetProductDetailsByIdAsync_ShouldReturnNull_WhenProductNotFound()
        {
            string? validId = Guid.NewGuid().ToString();
            this.productRepositoryMock
                .Setup(pr => pr.GetAllAttached())
                .Returns(new List<Product>().BuildMock());

            ProductDetailsViewModel? detailsViewModel = await this.productService.GetProductDetailsByIdAsync(validId);

            Assert.IsNull(detailsViewModel, "Expected null when product is not found");
        }

        [Test]
        public async Task GetProductDetailsByIdAsync_ShouldReturnMappedProduct_WhenProductExists()
        {
            List<Product> products = new List<Product>()
            {
                new Product()
                {
                    Id = Guid.Parse("51fdfb4b-858f-4dec-a42c-9622e4627af2"),
                    Name = "Test Product 1",
                    ImageUrl = "https://example.com/image1.jpg",
                    Price = 100.00m,
                    Quantity = 10,
                    SubCategoryId = 1,
                    IsDeleted = false,
                    Category = new Category()
                    {
                        Name = "Test Category"
                    },
                }
            };

            IQueryable<Product> productsQueryable = products.BuildMock();

            this.productRepositoryMock
                .Setup(pr => pr.GetAllAttached())
                .Returns(productsQueryable);

            ProductDetailsViewModel? detailsViewModel = await this.productService.GetProductDetailsByIdAsync("51fdfb4b-858f-4dec-a42c-9622e4627af2");

            Assert.IsNotNull(detailsViewModel, "Expected non-null when product exists");

        }
    }
}
