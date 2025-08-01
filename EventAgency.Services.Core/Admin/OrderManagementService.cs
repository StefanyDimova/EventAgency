using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Admin.Interfaces;
using EventAgency.Web.ViewModels.Admin.OrderManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Services.Core.Admin
{
    public class OrderManagementService : IOrderManagementService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;

        public OrderManagementService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
        }

        public async Task<bool> CancelOrderAsync(string orderId)
        {
            Order order = await this.orderRepository
                .GetAllAttached()
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id.ToString().ToLower() == orderId.ToLower());

            if (order == null || order.IsConfirmed || order.IsCancelled)
            {
                return false;
            }

            order.IsCancelled = true;
            await this.orderRepository.UpdateAsync(order);

            return true;
        }

        public async Task<bool> ConfirmOrderAsync(string orderId)
        {
            Order order = await this.orderRepository
                .GetAllAttached()
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id.ToString().ToLower() == orderId.ToLower());

            if (order == null || order.IsConfirmed || order.IsCancelled)
            {
                return false;
            }
                
            foreach (OrderItem item in order.OrderItems)
            {
                Product product = item.Product;

                // Проверка за наличност
                if (product.Quantity < item.Quantity)
                    return false;  // Ако няма достатъчно наличност

                // Намаляване на наличността
                product.Quantity -= item.Quantity;
                await this.productRepository.UpdateAsync(product);
            }

            // Потвърдете поръчката
            order.IsConfirmed = true;
            await this.orderRepository.UpdateAsync(order);

            return true;
        }

        public async Task<IEnumerable<OrderManagementViewModel>> GetAllOrdersForAdminAsync()
        {
            IEnumerable<OrderManagementViewModel> orders = await this.orderRepository
                .GetAllAttached()
                .Include(o => o.User)
                .Select(o => new OrderManagementViewModel
                {
                    Id = o.Id.ToString(),
                    UserEmail = o.User.Email,
                    CreatedAt = o.CreatedAt,
                    TotalPrice = o.TotalPrice,
                    IsConfirmed = o.IsConfirmed,
                    IsCancelled = o.IsCancelled
                })
                .ToListAsync();

            return orders;
        }
    }
}
