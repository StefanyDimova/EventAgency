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
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<AllProductsViewModel> allProducts = await this.productService.GetAllProductsAsync();
                return View(allProducts);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> BySubCategory(int id)
        {
            try
            {
                var products = await productService.GetProductsBySubCategoryIdAsync(id);
                return View("Index", products);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetSubcategories(int parentId)
        {
            try
            {
                var subcategories = await categoryService.GetSubCategoriesDropdownDataAsync(parentId);
                return Json(subcategories);
            }
            catch (Exception)
            {
                return Json(new { error = "An error occurred while retrieving subcategories." });
            }
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
                    return RedirectToAction("Error", "Home", new { statusCode = 404 });
                }

                return this.View("Details", productDetails);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);

                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }
        }

    }
}

