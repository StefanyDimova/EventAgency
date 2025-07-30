using EventAgency.Web.ViewModels.Admin.CategoryManagement;
using EventAgency.Web.ViewModels.Category;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventAgency.Services.Core.Admin.Interfaces
{
    public interface ICategoryManagementService
    {
        Task<IEnumerable<CategoryManagementViewModel>> GetAllCategoriesAsync();

        Task<IEnumerable<SelectListItem>> GetCategorySelectListAsync();
        Task CreateCategoryAsync(CreateCategoryManagementFormModel model);

    }
}
