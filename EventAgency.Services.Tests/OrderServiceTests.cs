using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core;
using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Cart;
using EventAgency.Web.ViewModels.Order;
using Moq;

namespace EventAgency.Services.Tests
{
    public class OrderServiceTests
    {
        private Mock<ICartService> cartServiceMock;
        private Mock<IOrderRepository> orderRepositoryMock;
        private OrderService orderService;

        [SetUp]
        public void Setup()
        {
            this.cartServiceMock = new Mock<ICartService>();
            this.orderRepositoryMock = new Mock<IOrderRepository>();
            this.orderService = new OrderService(this.cartServiceMock.Object, this.orderRepositoryMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        // Tests for CreateOrderAsync method

        [Test]
        public async Task CreateOrderAsync_ShouldCreateOrderSuccessfully_WhenCartIsNotEmpty()
        {
            string userId = "user123";

            List<CartItemViewModel> cartItems = new List<CartItemViewModel>
            {
                new CartItemViewModel
                {
                    ProductId = Guid.NewGuid().ToString(),
                    ProductName = "Test Product",
                    ImageUrl = "https://example.com/image.jpg",
                    Quantity = 2,
                    Price = 50.00m
                }
            };

            OrderCheckoutViewModel orderCheckoutViewModel = new OrderCheckoutViewModel
            {
                TotalPriceBGN = 100.00m,
                Address = "123 Test St",
                Phone = "1234567890",
                PaymentMethod = "Credit Card"
            };

            this.cartServiceMock
                .Setup(cs => cs.GetUserCartAsync(userId))
                .ReturnsAsync(cartItems);

            this.orderRepositoryMock
                .Setup(or => or.AddAsync(It.IsAny<Order>()))
                .Returns(Task.CompletedTask);

            Order createdOrder = await this.orderService.CreateOrderAsync(orderCheckoutViewModel, userId);

            Assert.IsNotNull(createdOrder);
            Assert.AreEqual(userId, createdOrder.UserId);
            Assert.AreEqual(orderCheckoutViewModel.TotalPriceBGN, createdOrder.TotalPrice);
            Assert.AreEqual(orderCheckoutViewModel.Address, createdOrder.Address);
            Assert.AreEqual(orderCheckoutViewModel.Phone, createdOrder.Phone);
            Assert.AreEqual(orderCheckoutViewModel.PaymentMethod, createdOrder.PaymentMethod);
            Assert.IsFalse(createdOrder.IsConfirmed);
            Assert.IsFalse(createdOrder.IsCancelled);
            Assert.IsNotEmpty(createdOrder.OrderItems);
            Assert.AreEqual(cartItems.Count, createdOrder.OrderItems.Count);

        }

        [Test]
        public void CreateOrderAsync_ShouldThrow_WhenCartIsEmpty()
        {
            string userId = "user123";

            OrderCheckoutViewModel orderCheckoutViewModel = new OrderCheckoutViewModel
            {
                TotalPriceBGN = 0.0m,
                Address = "123 Test St",
                Phone = "1234567890",
                PaymentMethod = "Credit Card"
            };

            this.cartServiceMock.Setup(cs => cs.GetUserCartAsync(userId))
                .ReturnsAsync(new List<CartItemViewModel>());

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => 
            {
                await this.orderService.CreateOrderAsync(orderCheckoutViewModel, userId);
            }, "Количката е празна.");

            Assert.AreEqual("Количката е празна.", ex.Message);
            orderRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Never);
            cartServiceMock.Verify(c => c.ClearUserCartAsync(It.IsAny<string>()), Times.Never);
        }
    }
}
