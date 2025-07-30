using EventAgency.Services.Core;
using EventAgency.Services.Core.Admin.Interfaces;
using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Admin.ProductManagement;
using Microsoft.AspNetCore.Mvc;

using static EventAgency.GCommon.ApplicationConstants;

using static EventAgency.Web.ViewModels.ValidationMessages.Event;


namespace EventAgency.Web.Areas.Admin.Controllers
{
    public class ProductManagementController : BaseAdminController
    {
        private readonly IProductManagementService productManagementService;
        private readonly ICategoryService categoryService;
        private readonly ILogger<ProductManagementController> logger;

        public ProductManagementController(IProductManagementService productManagementService, ICategoryService categoryService, ILogger<ProductManagementController> logger)
        {
            this.productManagementService = productManagementService;
            this.categoryService = categoryService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            IEnumerable<ProductManagementIndexViewModel> allProducts = await this.productManagementService
                .GetProductManagementDataAsync();

            return View(allProducts);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                AddProductInputModel model = new AddProductInputModel()
                {
                    Categories = await this.categoryService.GetCategoriesDropdownDataAsync(),
                    SubCategories = new List<AddProductCategoryDropDownModel>()
                };

                return this.View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetSubcategories(int parentId)
        {
            var subcategories = await categoryService.GetSubCategoriesDropdownDataAsync(parentId);
            return Json(subcategories);
        }


        [HttpPost]
        public async Task<IActionResult> Create(AddProductInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                inputModel.Categories = await this.categoryService.GetCategoriesDropdownDataAsync();
                inputModel.SubCategories = await this.categoryService.GetSubCategoriesDropdownDataAsync(inputModel.CategoryId);

                return this.View(inputModel);
            }
            try
            {
                await this.productManagementService.AddProductAsync(inputModel);
                TempData[SuccessMessageKey] = "Product added successfully!";
                return this.RedirectToAction(nameof(Manage));
            }
            catch (Exception e)
            {
                this.logger.LogCritical(e.Message);
                TempData[ErrorMessageKey] = "Fatal error occurred while adding your product! Please try again later!";

                return this.RedirectToAction(nameof(Manage));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            try
            {
                ProductEditInputModel? editableProduct = await this.productManagementService.GetEditableProductByIdAsync(id);
                if (editableProduct == null)
                {
                    return this.RedirectToAction(nameof(Manage));
                }

                editableProduct.Categories = await categoryService.GetCategoriesDropdownDataAsync();

                if (editableProduct.CategoryId != 0)
                {
                    editableProduct.SubCategories = await categoryService.GetSubCategoriesDropdownDataAsync(editableProduct.CategoryId);
                }
                else
                {
                    editableProduct.SubCategories = new List<AddProductCategoryDropDownModel>();
                }

                return this.View(editableProduct);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Manage));
            }
        }



        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                inputModel.Categories = await categoryService.GetCategoriesDropdownDataAsync();
                if (inputModel.CategoryId != 0)
                {
                    inputModel.SubCategories = await categoryService.GetSubCategoriesDropdownDataAsync(inputModel.CategoryId);
                }
                else
                {
                    inputModel.SubCategories = new List<AddProductCategoryDropDownModel>();
                }
                return this.View(inputModel);
            }

            try
            {
                bool editSuccess = await this.productManagementService.EditProductAsync(inputModel);
                if (!editSuccess)
                {
                    return this.RedirectToAction(nameof(Edit), new { id = inputModel.Id });
                }

                return this.RedirectToAction(nameof(Manage));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Manage));
            }
        }

    }
}
