using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Admin.Interfaces;
using EventAgency.Web.ViewModels.Admin.OrderManagement;
using EventAgency.Web.ViewModels.Order;
using Microsoft.EntityFrameworkCore;

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

        public async Task<OrderManagementDetailsViewModel?> GetOrderDetailsAsync(string orderId)
        {
            var order = await this.orderRepository
                .GetAllAttached()
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id.ToString().ToLower() == orderId.ToLower());

            if (order == null)
            {
                return null;
            }

            if (order.User == null)
            {
                return null;
            }

            var orderItems = order.OrderItems?
                .Where(oi => oi.Product != null)
                .Select(oi => new OrderManagementItemViewModel
                {
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    TotalPrice = oi.TotalPrice
                })
                .ToList() ?? new List<OrderManagementItemViewModel>();

            var orderDetailsViewModel = new OrderManagementDetailsViewModel
            {
                OrderId = order.Id.ToString(),
                UserEmail = order.User.Email,
                Address = order.Address,
                Phone = order.Phone,
                CreatedAt = order.CreatedAt,
                TotalPrice = order.TotalPrice,
                IsConfirmed = order.IsConfirmed,
                IsCancelled = order.IsCancelled,
                OrderItems = orderItems
            };

            return orderDetailsViewModel;
        }
    }
}
