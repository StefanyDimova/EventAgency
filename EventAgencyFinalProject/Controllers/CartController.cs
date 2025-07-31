using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Cart;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventAgency.Web.Controllers
{
    public class CartController : BaseController
    {
        private readonly ICartService cartService;
        private readonly IProductService productService;


        public CartController(ICartService cartService, IProductService productService)
        {
            this.cartService = cartService;
            this.productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                string userId = this.GetUserId();

                if (userId == null)
                {
                    return this.Forbid();
                }
                IEnumerable<CartItemViewModel> userCart = await this.cartService
                    .GetUserCartAsync(userId);

                if (userCart == null)
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 404 });
                }

                return View(userCart);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddToCartViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                var productDetails = await this.productService.GetProductDetailsByIdAsync(model.ProductId);
                if (productDetails == null)
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 404 });
                }
                return View("~/Views/Product/Details.cshtml", productDetails);
            }

            try
            {
                bool result = await this.cartService.AddProductToUserCartAsync(model.ProductId, userId, model.Quantity);

                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Няма достатъчна наличност от този продукт.");
                    var productDetails = await this.productService.GetProductDetailsByIdAsync(model.ProductId);
                    if (productDetails == null)
                    {
                        return RedirectToAction("Error", "Home", new { statusCode = 404 });
                    }
                    return View("~/Views/Product/Details.cshtml", productDetails);
                }

                return RedirectToAction("Index", "Cart");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }
        }




        [HttpPost]
        public async Task<IActionResult> Remove(string? productId)
        {
            try
            {
                string? userId = this.GetUserId();
                if (userId == null)
                {
                    return this.Forbid();
                }

                bool result = await this.cartService
                    .RemoveProductFromCartAsync(productId, userId);
                if (result == false)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.RedirectToAction(nameof(Index), "Cart");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(string productId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(productId) || quantity < 1)
            {
                TempData["ErrorMessage"] = "Невалидни данни за обновяване на количката.";
                return RedirectToAction("Index");
            }

            try
            {
                bool updated = await this.cartService.UpdateQuantityAsync(userId, productId, quantity);

                if (!updated)
                {
                    TempData["ErrorMessage"] = "Продуктът не беше намерен в количката или количеството надвишава наличността.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Количеството беше обновено успешно.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["ErrorMessage"] = "Възникна грешка при обновяване на количката.";
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }

            return RedirectToAction("Index");
        }


    }
}
