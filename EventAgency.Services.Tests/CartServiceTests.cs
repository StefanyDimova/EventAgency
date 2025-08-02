using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core;
using EventAgency.Web.ViewModels.Cart;
using MockQueryable;
using Moq;
using System.Linq.Expressions;

namespace EventAgency.Services.Tests
{
    [TestFixture]
    public class CartServiceTests
    {
        private Mock<ICartRepository> cartRepositoryMock;
        private Mock<IProductRepository> productRepositoryMock;
        private CartService cartService;

        [SetUp]
        public void Setup()
        {
            this.cartRepositoryMock = new Mock<ICartRepository>();
            this.productRepositoryMock = new Mock<IProductRepository>();
            this.cartService = new CartService(this.cartRepositoryMock.Object, this.productRepositoryMock.Object);
        }


        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        // Tests for AddProductToUserCartAsync method

        [TestCaseSource(nameof(InvalidInputs))]
        public async Task AddProductToUserCartAsync_ShouldReturnFalse_WhenInvalidInput
            (string? productId, string? userId, int quantity)
        {
            bool result = await this.cartService.AddProductToUserCartAsync(productId, userId, quantity);
            Assert.IsFalse(result);
        }


        [Test]
        public async Task AddProductToUserCartAsync_ShouldReturnFalse_WhenProductIdIsInvalidGuid()
        {
            bool result = await this.cartService.AddProductToUserCartAsync("invalid-guid", "user1", 1);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddProductToUserCartAsync_ShouldReturnFalse_WhenProductIsNullOrDeleted()
        {
            Guid productId = Guid.NewGuid();
            string userId = "user1";
            int quantity = 1;

            this.productRepositoryMock
                .Setup(pr => pr.GetByIdAsync(productId))
                .ReturnsAsync((Product?)null); // Продукт липсва

            bool result = await this.cartService.AddProductToUserCartAsync(productId.ToString(), userId, quantity);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddProductToUserCartAsync_ShouldReturnFalse_WhenUpdateExceedsStock()
        {
            Guid productId = Guid.NewGuid();
            Product product = new Product
            {
                Id = productId,
                Quantity = 5,
                IsDeleted = false
            };

            ApplicationUserProduct? userProductEntry = new ApplicationUserProduct
            {
                ApplicationUserId = "user1",
                ProductId = productId,
                IsDeleted = false
            };

            this.productRepositoryMock
                .Setup(pr => pr.GetByIdAsync(productId))
                .ReturnsAsync(product);

            this.cartRepositoryMock
                .Setup(cr => cr.GetAllAttached())
                .Returns(new List<ApplicationUserProduct> { userProductEntry }.BuildMock());

            bool result = await this.cartService.AddProductToUserCartAsync(productId.ToString(), "user1", 10);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddProductToUserCartAsync_ShouldUpdateQuantity_WhenProductExistsInCart()
        {
            Guid productId = Guid.NewGuid();
            Product product = new Product { Id = productId, Quantity = 10, IsDeleted = false };

            ApplicationUserProduct userCartItem = new ApplicationUserProduct
            {
                ApplicationUserId = "user1",
                ProductId = productId,
                Quantity = 3
            };

            this.productRepositoryMock
               .Setup(pr => pr.GetByIdAsync(productId))
               .ReturnsAsync(product);

            this.cartRepositoryMock
                .Setup(cr => cr.GetAllAttached())
                .Returns(new List<ApplicationUserProduct> { userCartItem }.BuildMock());

            this.cartRepositoryMock
                .Setup(cr => cr.UpdateAsync(It.IsAny<ApplicationUserProduct>()))
                .ReturnsAsync(true);

            bool result = await this.cartService.AddProductToUserCartAsync(productId.ToString(), "user1", 2);
            Assert.IsTrue(result);
            Assert.AreEqual(5, userCartItem.Quantity);
        }

        [Test]
        public async Task AddProductToUserCartAsync_ShouldAddNewItem_WhenProductIsNotInCart()
        {
            Guid productId = Guid.NewGuid();
            Product product = new Product { Id = productId, Quantity = 10, IsDeleted = false };

            this.productRepositoryMock
               .Setup(pr => pr.GetByIdAsync(productId))
               .ReturnsAsync(product);

            this.cartRepositoryMock
                .Setup(cr => cr.GetAllAttached())
                .Returns(new List<ApplicationUserProduct>().BuildMock());

            this.cartRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<ApplicationUserProduct>()))
                .Returns(Task.CompletedTask);

            bool result = await this.cartService.AddProductToUserCartAsync(productId.ToString(), "user1", 2);
            Assert.IsTrue(result);

        }


        // Tests for GetUserCartAsync method

        [Test]
        public async Task GetUserCartAsync_ShouldReturnMappedCartItems_WhenCartExists()
        {
            Guid userId = Guid.NewGuid();
            List<ApplicationUserProduct> userCartItems = new List<ApplicationUserProduct>
            {
                new ApplicationUserProduct
                {
                    ApplicationUserId = userId.ToString(),
                    ProductId = Guid.NewGuid(),
                    Quantity = 2,
                    Product = new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = "Test Product",
                        ImageUrl = "https://example.com/image.jpg",
                        Price = 50.00m
                    }
                }
            };

            IQueryable<ApplicationUserProduct> mockQueryable = userCartItems.BuildMock();
            this.cartRepositoryMock
                .Setup(cr => cr.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<CartItemViewModel> result = await this.cartService.GetUserCartAsync(userId.ToString());

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Test Product", result.First().ProductName);
            Assert.AreEqual(50.00m, result.First().Price);
            Assert.AreEqual(2, result.First().Quantity);
        }

        [Test]
        public async Task GetUserCartAsync_ShouldReplaceNullImageUrl_WithDefault()
        {
            Guid userId = Guid.NewGuid();
            List<ApplicationUserProduct> userCartItems = new List<ApplicationUserProduct>
            {
                new ApplicationUserProduct
                {
                    ApplicationUserId = userId.ToString(),
                    ProductId = Guid.NewGuid(),
                    Quantity = 2,
                    Product = new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = "Test Product",
                        ImageUrl = null,
                        Price = 50.00m
                    }
                }
            };

            IQueryable<ApplicationUserProduct> mockQueryable = userCartItems.BuildMock();

            this.cartRepositoryMock
                .Setup(cr => cr.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<CartItemViewModel> result = await this.cartService.GetUserCartAsync(userId.ToString());



            Assert.IsNotNull(result);
            Assert.AreEqual(result.First().ImageUrl, "/images/no-image.jpg");
        }

        // Tests for IsProductAddedToCart method

        [TestCaseSource(nameof(InvalidIsProductAddedInputs))]
        public async Task IsProductAddedToCart_ShouldReturnFalse_WhenInputIsInvalid(string? productId, string? userId)
        {
            bool result = await this.cartService.IsProductAddedToCart(productId, userId);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsProductAddedToCart_ShouldReturnFalse_WhenEntryNotFound()
        {
            Guid productGuid = Guid.NewGuid();
            string productId = productGuid.ToString();
            string userId = "user1";

            this.cartRepositoryMock
                .Setup(cr => cr.GetAllAttached())
                .Returns(new List<ApplicationUserProduct>().BuildMock());

            bool result = await this.cartService.IsProductAddedToCart(productId, userId);
            Assert.IsFalse(result);
        }

        // Tests for RemoveProductFromCartAsync method

        [TestCaseSource(nameof(InvalidIsProductRemoveInputs))]
        public async Task RemoveProductFromCartAsync_ShouldReturnFalse_WhenParamsAreNull(string? productId, string? userId)
        {
            bool result = await this.cartService.RemoveProductFromCartAsync(productId, userId);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveProductFromCartAsync_ShouldReturnFalse_WhenProductIdIsInvalidGuid()
        {
            string invalidProductId = "invalid-guid";
            string userId = "user1";

            bool result = await this.cartService.RemoveProductFromCartAsync(invalidProductId, userId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveProductFromCartAsync_ShouldReturnFalse_WhenUserProductEntryNotFound()
        {
            Guid productGuid = Guid.NewGuid();
            string userId = "user1";

            this.cartRepositoryMock
                .Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<ApplicationUserProduct, bool>>>()))
                .ReturnsAsync((ApplicationUserProduct?)null);

            bool result = await this.cartService.RemoveProductFromCartAsync(productGuid.ToString(), userId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveProductFromCartAsync_ShouldReturnTrue_WhenProductSuccessfullyRemoved()
        {
            Guid productGuid = Guid.NewGuid();
            string userId = "user1";
            ApplicationUserProduct userProductEntry = new ApplicationUserProduct
            {
                ProductId = productGuid,
                ApplicationUserId = userId,
                Quantity = 5
            };

            this.cartRepositoryMock
                .Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<ApplicationUserProduct, bool>>>()))
                .ReturnsAsync(userProductEntry);

            this.cartRepositoryMock
                .Setup(repo => repo.DeleteAsync(It.IsAny<ApplicationUserProduct>()))
                .ReturnsAsync(true);

            bool result = await this.cartService.RemoveProductFromCartAsync(productGuid.ToString(), userId);

            Assert.IsTrue(result);
            Assert.AreEqual(0, userProductEntry.Quantity);
        }

        // Tests for ClearUserCartAsync method

        [Test]
        public async Task ClearUserCartAsync_ShouldReturnTrue_WhenCartHasItems()
        {
            string userId = "testUser";

            List<ApplicationUserProduct> cartItems = new List<ApplicationUserProduct>

                 {
                     new ApplicationUserProduct { ApplicationUserId = userId, ProductId = Guid.NewGuid(), Quantity = 1 },
                     new ApplicationUserProduct { ApplicationUserId = userId, ProductId = Guid.NewGuid(), Quantity = 2 }
                 };

            IQueryable<ApplicationUserProduct> mockCartItems = cartItems.BuildMock();

            this.cartRepositoryMock
                .Setup(repo => repo.GetAllAttached())
                .Returns(mockCartItems);

            this.cartRepositoryMock
                .Setup(repo => repo.DeleteAsync(It.IsAny<ApplicationUserProduct>()))
                .ReturnsAsync(true);

            this.cartRepositoryMock
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            bool result = await this.cartService.ClearUserCartAsync(userId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task ClearUserCartAsync_ShouldReturnFalse_WhenCartIsEmpty()
        {
            string userId = "emptyUser";

            List<ApplicationUserProduct> cartItems = new List<ApplicationUserProduct>();

            IQueryable<ApplicationUserProduct> mockCartItems = cartItems.BuildMock();

            this.cartRepositoryMock
                .Setup(repo => repo.GetAllAttached())
                .Returns(mockCartItems);

            bool result = await this.cartService.ClearUserCartAsync(userId);

            Assert.IsFalse(result);
        }

        [Test]
        public void ClearUserCartAsync_ShouldThrowException_WhenRepositoryThrows()
        {
            string userId = "testUser";

            this.cartRepositoryMock
                .Setup(repo => repo.GetAllAttached())
                .Throws(new Exception("Database error"));

            Assert.ThrowsAsync<Exception>(() => this.cartService.ClearUserCartAsync(userId));
        }

        // Tests for UpdateQuantityAsync method

        [Test]
        public async Task UpdateQuantityAsync_ShouldReturnFalse_WhenProductIdIsInvalidGuid()
        {
            string userId = "user1";
            string invalidProductId = "invalid-guid";
            int quantity = 1;

            bool result = await this.cartService.UpdateQuantityAsync(userId, invalidProductId, quantity);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task UpdateQuantityAsync_ShouldReturnFalse_WhenUserProductNotFound()
        {
            string userId = "user1";
            Guid productGuid = Guid.NewGuid();
            string productId = productGuid.ToString();
            int quantity = 1;

            IQueryable<ApplicationUserProduct> emptyQuery = new List<ApplicationUserProduct>().BuildMock();

            this.cartRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(emptyQuery);

            bool result = await this.cartService.UpdateQuantityAsync(userId, productId, quantity);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task UpdateQuantityAsync_ShouldReturnFalse_WhenRequestedQuantityExceedsAvailable()
        {
            string userId = "user1";
            Guid productGuid = Guid.NewGuid();
            string productId = productGuid.ToString();
            int requestedQuantity = 15;

            List<ApplicationUserProduct> userProducts = new List<ApplicationUserProduct>
            {
            new ApplicationUserProduct()
               {
                   ApplicationUserId = userId,
                   ProductId = productGuid,
                   Product = new Product { Quantity = 10 },
                   Quantity = 1
               }
            };

            IQueryable<ApplicationUserProduct> mockQueryable = userProducts.BuildMock();
            this.cartRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockQueryable);

            bool result = await this.cartService.UpdateQuantityAsync(userId, productId, requestedQuantity);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task UpdateQuantityAsync_ShouldReturnTrue_WhenUpdateIsValid()
        {
            string userId = "user1";
            Guid productGuid = Guid.NewGuid();
            string productId = productGuid.ToString();
            int newQuantity = 5;

            List<ApplicationUserProduct> userProducts = new List<ApplicationUserProduct>
            {
                new ApplicationUserProduct
                {
                    ApplicationUserId = userId,
                    ProductId = productGuid,
                    Product = new Product { Quantity = 10 },
                    Quantity = 1
                }
            };

            IQueryable<ApplicationUserProduct> mockQueryable = userProducts.BuildMock();

            this.cartRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockQueryable);

            this.cartRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<ApplicationUserProduct>()))
                .ReturnsAsync(true);

            bool result = await this.cartService.UpdateQuantityAsync(userId, productId, newQuantity);

            Assert.IsTrue(result);
        }


        static object[] InvalidInputs =
        {
             new object[] { null, "validUser", 1 },
             new object[] { "", "validUser", 1 },
             new object[] { "validProduct", null, 1 },
             new object[] { "validProduct", "", 1 },
             new object[] { "validProduct", "validUser", 0 },
             new object[] { "validProduct", "validUser", -5 }
        };

        static object[] InvalidIsProductAddedInputs =
        {
             new object[] { null, "user1" },
             new object[] { "", "user1" },
             new object[] { "invalid-guid", "user1" }, // невалиден GUID
             new object[] { Guid.NewGuid().ToString(), null },
             new object[] { Guid.NewGuid().ToString(), "" }
        };

        static object[] InvalidIsProductRemoveInputs =
        {
             new object[] { null, "userId" },
             new object[] { "productId", null },
             new object[] { null, null }
        };


    }
}
