using EventAgency.Web.ViewModels.Admin.ProductManagement;
using EventAgency.Web.ViewModels.Category;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventAgency.Services.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();

        Task<IEnumerable<SelectListItem>> GetCategorySelectListAsync();

        Task<IEnumerable<AddProductCategoryDropDownModel>> GetCategoriesDropdownDataAsync();
        Task<IEnumerable<AddProductCategoryDropDownModel>> GetSubCategoriesDropdownDataAsync(int parentCategoryId);
    }
}
