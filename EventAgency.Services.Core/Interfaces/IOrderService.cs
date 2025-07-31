using EventAgency.Data.Models;
using EventAgency.Web.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;

namespace EventAgency.Services.Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(OrderCheckoutViewModel checkoutModel, string userId);
    }
}
