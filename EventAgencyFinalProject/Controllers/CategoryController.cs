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
            try
            {
                IEnumerable<CategoryViewModel> allCategories = await this.categoryService.GetAllCategoriesAsync();
                return View(allCategories);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }
        }
    }
}
