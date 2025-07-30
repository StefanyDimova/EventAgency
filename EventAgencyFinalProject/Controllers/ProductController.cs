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

    }
}
