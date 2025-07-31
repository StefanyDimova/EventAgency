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

        public OrderManagementService(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
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
