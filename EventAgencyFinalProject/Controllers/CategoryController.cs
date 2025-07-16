using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventAgency.Web.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<CategoryViewModel> allCategories = await this.categoryService
                .GetAllCategoriesAsync();

            return View(allCategories);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CategoryFormModel model = new CategoryFormModel
            {
                AvailableCategories = await categoryService.GetCategorySelectListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableCategories = await categoryService.GetCategorySelectListAsync();
                return View(model);
            }

            await categoryService.CreateCategoryAsync(model);

            return RedirectToAction("Index", "Category");
        }
    }
}
