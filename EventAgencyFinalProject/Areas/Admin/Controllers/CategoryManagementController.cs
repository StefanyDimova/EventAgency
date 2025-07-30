using EventAgency.Services.Core;
using EventAgency.Services.Core.Admin.Interfaces;
using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Admin.CategoryManagement;
using EventAgency.Web.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

using static EventAgency.GCommon.ApplicationConstants;

namespace EventAgency.Web.Areas.Admin.Controllers
{
    public class CategoryManagementController : BaseAdminController
    {
        private readonly ICategoryManagementService categoryManagementService;

        public CategoryManagementController(ICategoryManagementService categoryManagementService)
        {
            this.categoryManagementService = categoryManagementService;
        }
        public async Task<IActionResult> Manage()
        {
            IEnumerable<CategoryManagementViewModel> allCategories = await this.categoryManagementService
               .GetAllCategoriesAsync();

            return View(allCategories);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateCategoryManagementFormModel model = new CreateCategoryManagementFormModel
            {
                AvailableCategories = await categoryManagementService.GetCategorySelectListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryManagementFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableCategories = await categoryManagementService.GetCategorySelectListAsync();
                return View(model);
            }

            await categoryManagementService.CreateCategoryAsync(model);

            return RedirectToAction(nameof(Manage));
        }


    }
}
