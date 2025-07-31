using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Services.Core
{
    public class OrderService : IOrderService
    {
        private readonly ICartService cartService;
        private readonly IOrderRepository orderRepository;

        public OrderService(ICartService cartService, IOrderRepository orderRepository)
        {
            this.cartService = cartService;
            this.orderRepository = orderRepository;
        }
        public async Task<Order> CreateOrderAsync(OrderCheckoutViewModel checkoutModel, string userId)
        {
            
            // Вземаме продуктите от количката на потребителя
            var cartItems = await this.cartService.GetUserCartAsync(userId);
            if (!cartItems.Any())
            {
                throw new InvalidOperationException("Количката е празна.");
            }

            // Създаваме поръчката
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                TotalPrice = checkoutModel.TotalPriceBGN,
                Address = checkoutModel.Address,
                Phone = checkoutModel.Phone,
                PaymentMethod = checkoutModel.PaymentMethod,
                IsConfirmed = false,  // Ще бъде потвърдена от администратора
                IsCancelled = false,
                OrderItems = cartItems.Select(item => new OrderItem
                {
                    ProductId = Guid.Parse(item.ProductId),
                    ProductName = item.ProductName,
                    ImageUrl = item.ImageUrl,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    TotalPrice = item.Price * item.Quantity
                }).ToList()
            };

            // Записваме поръчката в базата данни
            await this.orderRepository.AddAsync(order);

            // След създаване на поръчката, изпразваме количката
            await this.cartService.ClearUserCartAsync(userId);

            return order;
        }

    }
}
