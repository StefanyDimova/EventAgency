using EventAgency.Services.Core.Admin.Interfaces;
using EventAgency.Web.ViewModels.Admin.ProductManagement;
using Microsoft.AspNetCore.Mvc;

namespace EventAgency.Web.Areas.Admin.Controllers
{
    public class ProductManagementController : BaseAdminController
    {
        private readonly IProductManagementService productManagementService;

        public ProductManagementController(IProductManagementService productManagementService)
        {
            this.productManagementService = productManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            IEnumerable<ProductManagementIndexViewModel> allProducts = await this.productManagementService
                .GetProductManagementDataAsync();

            return View(allProducts);
        }

    }
}
