using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Admin;
using EventAgency.Web.ViewModels.Admin.OrderManagement;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Services.Tests
{
    [TestFixture]
    public class OrderManagementServiceTests
    {
        private Mock<IOrderRepository> orderRepositoryMock;
        private Mock<IProductRepository> productRepositoryMock;
        private OrderManagementService orderManagementService;

        [SetUp]
        public void Setup()
        {
            this.orderRepositoryMock = new Mock<IOrderRepository>();
            this.productRepositoryMock = new Mock<IProductRepository>();
            this.orderManagementService = new OrderManagementService(this.orderRepositoryMock.Object, this.productRepositoryMock.Object);
        }


        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        // Tests for GetAllOrdersForAdminAsync method

        [Test]
        public async Task GetAllOrdersForAdminAsync_WithOrders_ReturnsMappedList()
        {
            Guid orderId = Guid.NewGuid();
            IdentityUser user = new IdentityUser
            {
                Id = "user-id",
                Email = "test@example.com"
            };

            List<Order> orders = new List<Order>
            {
                new Order
                {
                    Id = orderId,
                    User = user,
                    CreatedAt = DateTime.UtcNow,
                    TotalPrice = 99.99m,
                    IsConfirmed = true,
                    IsCancelled = false
                }
            };

            IQueryable<Order> orderQuery = orders.BuildMock();

            this.orderRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(orderQuery);

            IEnumerable<OrderManagementViewModel> result = await this.orderManagementService.GetAllOrdersForAdminAsync();

            Assert.That(result.Count(), Is.EqualTo(1));
            OrderManagementViewModel order = result.First();
            Assert.That(order.Id, Is.EqualTo(orderId.ToString()));
            Assert.That(order.UserEmail, Is.EqualTo("test@example.com"));
            Assert.That(order.TotalPrice, Is.EqualTo(99.99m));
            Assert.That(order.IsConfirmed, Is.True);
            Assert.That(order.IsCancelled, Is.False);
        }

        [Test]
        public async Task GetAllOrdersForAdminAsync_NoOrders_ReturnsEmptyList()
        {
            List<Order> orders = new List<Order>();
            IQueryable<Order> emptyQuery = orders.BuildMock();

            this.orderRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(emptyQuery);

            IEnumerable<OrderManagementViewModel> result = await this.orderManagementService.GetAllOrdersForAdminAsync();

            Assert.IsEmpty(result);
            Assert.IsNotNull(result);
        }

        // Tests for GetOrderDetailsAsync method

        [Test]
        public async Task GetOrderDetailsAsync_OrderNotFound_ReturnsNull()
        {
            List<Order> orders = new List<Order>();
            IQueryable<Order> emptyQuery = orders.BuildMock();

            this.orderRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(emptyQuery);

            OrderManagementDetailsViewModel? result = await this.orderManagementService.GetOrderDetailsAsync(Guid.NewGuid().ToString());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetOrderDetailsAsync_UserIsNull_ReturnsNull()
        {
            Guid orderId = Guid.NewGuid();

            List<Order> orders = new List<Order>
            {
                new Order
                {
                  Id = orderId,
                  User = null
                }
            };

            IQueryable<Order> query = orders.BuildMock();

            this.orderRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(query);

            OrderManagementDetailsViewModel? result = await this.orderManagementService.GetOrderDetailsAsync(orderId.ToString());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetOrderDetailsAsync_ValidOrder_ReturnsFullModel()
        {
            Guid orderId = Guid.NewGuid();

            IdentityUser user = new IdentityUser { Id = "user123", Email = "user@mail.com" };

            Product product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Balloon"
            };

            OrderItem item = new OrderItem
            {
                Product = product,
                Quantity = 3,
                Price = 5.00m,
                TotalPrice = 15.00m
            };

            List<Order> orders = new List<Order>()
            {
                new Order
                {
                     Id = orderId,
                     User = user,
                     Address = "Sofia",
                     Phone = "0888123456",
                     CreatedAt = DateTime.UtcNow,
                     TotalPrice = 15.00m,
                     IsConfirmed = true,
                     IsCancelled = false,
                     OrderItems = new List<OrderItem> { item }
                }
            };

            IQueryable<Order> query = orders.BuildMock();

            this.orderRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(query);

            OrderManagementDetailsViewModel? result = await this.orderManagementService.GetOrderDetailsAsync(orderId.ToString());

            Assert.IsNotNull(result);
            Assert.That(result!.UserEmail, Is.EqualTo("user@mail.com"));
            Assert.That(result.OrderItems.Count, Is.EqualTo(1));
            Assert.That(result.OrderItems.First().ProductName, Is.EqualTo("Balloon"));
        }

        [Test]
        public async Task GetOrderDetailsAsync_SkipsItemsWithNullProduct()
        {
            Guid orderId = Guid.NewGuid();

            IdentityUser user = new IdentityUser { Email = "user@mail.com" };

            OrderItem itemWithNullProduct = new OrderItem
            {
                Product = null,
                Quantity = 1,
                Price = 0,
                TotalPrice = 0
            };

            OrderItem validItem = new OrderItem
            {
                Product = new Product { Name = "Valid" },
                Quantity = 2,
                Price = 10,
                TotalPrice = 20
            };

            List<Order> orders = new List<Order>()
            {
                new Order
                {
                    Id = orderId,
                    User = user,
                    Address = "Test",
                    Phone = "000000",
                    CreatedAt = DateTime.UtcNow,
                    TotalPrice = 20,
                    IsConfirmed = false,
                    IsCancelled = false,
                    OrderItems = new List<OrderItem> { itemWithNullProduct, validItem }
                }
            };

            IQueryable<Order> query = orders.BuildMock();

            this.orderRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(query);

            OrderManagementDetailsViewModel? result = await this.orderManagementService.GetOrderDetailsAsync(orderId.ToString());

            Assert.IsNotNull(result);
            Assert.That(result!.OrderItems.Count, Is.EqualTo(1));
            Assert.That(result.OrderItems.First().ProductName, Is.EqualTo("Valid"));
        }

        // Tests for ConfirmOrderAsync method

        [Test]
        public async Task ConfirmOrderAsync_OrderNotFound_ReturnsFalse()
        {
            List<Order> orders = new List<Order>();
            IQueryable<Order> emptyQuery = orders.BuildMock();

            this.orderRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(emptyQuery);

            bool result = await this.orderManagementService.ConfirmOrderAsync(Guid.NewGuid().ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task ConfirmOrderAsync_OrderAlreadyConfirmedOrCancelled_ReturnsFalse()
        {
            Guid orderId = Guid.NewGuid();

            List<Order> orders = new List<Order>
            {
                new Order
                {
                    Id = orderId,
                    IsConfirmed = true,
                    IsCancelled = false
                }
            };
            

            IQueryable<Order> query = orders.BuildMock();

            this.orderRepositoryMock.Setup(r => r.GetAllAttached()).Returns(query);

            bool result = await this.orderManagementService.ConfirmOrderAsync(orderId.ToString());

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ConfirmOrderAsync_ProductQuantityTooLow_ReturnsFalse()
        {
            Guid orderId = Guid.NewGuid();

            Product product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Quantity = 1
            };

            OrderItem item = new OrderItem
            {
                Product = product,
                Quantity = 2
            };

            List<Order> orders = new List<Order>
            {
                new Order
                {
                    Id = orderId,
                    IsConfirmed = false,
                    IsCancelled = false,
                    OrderItems = new List<OrderItem> { item }
                }
            };

            IQueryable<Order> query = orders.BuildMock();

            this.orderRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(query);

            bool result = await this.orderManagementService.ConfirmOrderAsync(orderId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task ConfirmOrderAsync_ValidOrder_UpdatesProductsAndOrder_ReturnsTrue()
        {
            Guid orderId = Guid.NewGuid();

            Product product = new Product
            {
                Id = Guid.NewGuid(),
                Quantity = 10
            };

            OrderItem item = new OrderItem
            {
                Product = product,
                Quantity = 3
            };

            List<Order> orders = new List<Order>()
            {
                new Order
                {
                    Id = orderId,
                    IsConfirmed = false,
                    IsCancelled = false,
                    OrderItems = new List<OrderItem> { item }
                }
            };

            IQueryable<Order> query = orders.BuildMock();

            this.orderRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(query);

            this.productRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            this.orderRepositoryMock
                .Setup(r => r.UpdateAsync(orders.First()))
                .ReturnsAsync(true);

            bool result = await this.orderManagementService.ConfirmOrderAsync(orderId.ToString());

            Assert.IsTrue(result);
            Assert.That(product.Quantity, Is.EqualTo(7));
            this.productRepositoryMock.Verify(r => r.UpdateAsync(product), Times.Once);
            this.orderRepositoryMock.Verify(r => r.UpdateAsync(orders.First()), Times.Once);
        }

        // Tests for CancelOrderAsync method

        [Test]
        public async Task CancelOrderAsync_OrderNotFound_ReturnsFalse()
        {
            List<Order> orders = new List<Order>();
            IQueryable<Order> emptyQuery = orders.BuildMock();

            this.orderRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(emptyQuery);

            bool result = await this.orderManagementService.CancelOrderAsync(Guid.NewGuid().ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task CancelOrderAsync_OrderAlreadyConfirmedOrCancelled_ReturnsFalse()
        {
            Guid orderId = Guid.NewGuid();

            List<Order> orders = new List<Order>
            {
                new Order
                {
                    Id = orderId,
                    IsConfirmed = true,
                    IsCancelled = false
                }
            };

            IQueryable<Order> query = orders.BuildMock();

            this.orderRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(query);

            bool result = await this.orderManagementService.CancelOrderAsync(orderId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task CancelOrderAsync_ValidOrder_SetsIsCancelledAndReturnsTrue()
        {
            Guid orderId = Guid.NewGuid();

            List<Order> orders = new List<Order>
            {
                new Order
                {
                    Id = orderId,
                    IsConfirmed = false,
                    IsCancelled = false
                }
            };

            IQueryable<Order> query = orders.BuildMock();

            this.orderRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(query);

            this.orderRepositoryMock
                .Setup(r => r.UpdateAsync(orders.First()))
                .ReturnsAsync(true);

            bool result = await this.orderManagementService.CancelOrderAsync(orderId.ToString());

            Assert.IsTrue(result);
            Assert.That(orders.First().IsCancelled, Is.True);
            this.orderRepositoryMock.Verify(r => r.UpdateAsync(orders.First()), Times.Once);
        }
    }
}
