using EventAgency.Services.Core;
using EventAgency.Services.Core.Admin.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventAgency.Web.Areas.Admin.Controllers
{
    public class OrderManagementController : BaseAdminController
    {
        private readonly IOrderManagementService orderManagementService;

        public OrderManagementController(IOrderManagementService orderManagementService)
        {
            this.orderManagementService = orderManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var orders = await orderManagementService.GetAllOrdersForAdminAsync();
            return View(orders);
        }
    }
}
