using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Admin.ProductManagement;
using EventAgency.Web.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static EventAgency.Web.ViewModels.ValidationMessages.Event;

namespace EventAgency.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }

        [HttpGet]

        public async Task<IActionResult> Index()
        {
            IEnumerable<AllProductsViewModel> allProducts = await this.productService
                .GetAllProductsAsync();


            return View(allProducts);
        }

        [HttpGet]
        public async Task<IActionResult> BySubCategory(int id)
        {
            var products = await productService.GetProductsBySubCategoryIdAsync(id);
            return View("Index", products);
        }

        [HttpGet]
        public async Task<JsonResult> GetSubcategories(int parentId)
        {
            var subcategories = await categoryService.GetSubCategoriesDropdownDataAsync(parentId);
            return Json(subcategories);
        }

        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> Details(string? id)
        {
            try
            {
                ProductDetailsViewModel? productDetails = await this.productService
                    .GetProductDetailsByIdAsync(id);

                if (productDetails == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View("Details", productDetails);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            try
            {
                DeleteProductViewModel? productToBeDeleted = await this.productService
                    .GetProductDeleteDetailsByIdAsync(id);
                if (productToBeDeleted == null)
                {
                    // TODO: Custom 404 page
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(productToBeDeleted);
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                // TODO: Add JS bars to indicate such errors
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(DeleteProductViewModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    // TODO: Implement JS notifications
                    Console.WriteLine(">>> ModelState invalid!");
                    return this.RedirectToAction(nameof(Index));
                }

                bool deleteResult = await this.productService
                    .SoftDeleteProductAsync(inputModel.Id);
                if (deleteResult == false)
                {
                    // TODO: Implement JS notifications
                    // TODO: Alt_Redirect to Not Found page
                    return this.RedirectToAction(nameof(Index));
                }

                // TODO: Success notification
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                // TODO: Add JS bars to indicate such errors
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

    }
}
