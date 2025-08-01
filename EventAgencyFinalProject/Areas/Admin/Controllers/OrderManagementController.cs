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

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(string orderId)
        {
            try
            {
                bool result = await this.orderManagementService.ConfirmOrderAsync(orderId);
                if (result)
                {
                    TempData["SuccessMessage"] = "Поръчката е потвърдена успешно!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Възникна грешка при потвърждаването на поръчката.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Възникна грешка: " + ex.Message;
            }

            return RedirectToAction(nameof(Manage));
        }

        [HttpPost]

        public async Task<IActionResult> CancelOrder(string orderId)
        {
            try
            {
                bool result = await this.orderManagementService.CancelOrderAsync(orderId);
                if (result)
                {
                    TempData["SuccessMessage"] = "Поръчката е отказана успешно!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Възникна грешка при отказване на поръчката.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Възникна грешка: " + ex.Message;
            }

            return RedirectToAction(nameof(Manage));
        }

    }
}
