using EventAgency.Data.Models;
using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Cart;
using EventAgency.Web.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;

namespace EventAgency.Web.Controllers
{
    public class OrderController : BaseController
    {
        private readonly ICartService cartService;
        private readonly IOrderService orderService;

        public OrderController(ICartService cartService, IOrderService orderService)
        {
            this.cartService = cartService;
            this.orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            string userId = this.GetUserId(); // Получаваме потребителя

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Вземаме продуктите от количката на потребителя
            IEnumerable<CartItemViewModel> cartItems = await this.cartService.GetUserCartAsync(userId);

            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Количката ви е празна!";
                return RedirectToAction("Index", "Cart");
            }

            decimal totalPriceBGN = cartItems.Sum(item => item.Price * item.Quantity);
            decimal rate = 1.95583M;
            decimal totalPriceEUR = decimal.Round(totalPriceBGN / rate, 2);

            OrderCheckoutViewModel checkoutViewModel = new OrderCheckoutViewModel
            {
                TotalPriceBGN = totalPriceBGN,
                TotalPriceEUR = totalPriceEUR,
                OrderItems = cartItems.Select(item => new OrderItemViewModel
                {
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    TotalPrice = item.Price * item.Quantity
                }).ToList()
            };

            return View(checkoutViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmOrder(OrderCheckoutViewModel checkoutModel)
        {
            try
            {
                string userId = this.GetUserId(); 

                if (userId == null)
                {
                    TempData["ErrorMessage"] = "Не сте влезли в системата.";
                    return RedirectToAction("Login", "Account");
                }

                Order order = await this.orderService.CreateOrderAsync(checkoutModel, userId);

                TempData["SuccessMessage"] = $"Поръчката беше успешно създадена. Номер на поръчка: {order.Id}";

                return RedirectToAction("Index", "Cart");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Възникна грешка при създаване на поръчката.";
                return View(checkoutModel);
            }
        }
    }
}
