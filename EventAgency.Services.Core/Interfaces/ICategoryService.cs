using EventAgency.Web.ViewModels.Category;
using EventAgency.Web.ViewModels.Event;
using EventAgency.Web.ViewModels.Product;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Services.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();

        Task<IEnumerable<SelectListItem>> GetCategorySelectListAsync();

        Task CreateCategoryAsync(CategoryFormModel model);

        Task<IEnumerable<AddProductCategoryDropDownModel>> GetCategoriesDropdownDataAsync();
        Task<IEnumerable<AddProductCategoryDropDownModel>> GetSubCategoriesDropdownDataAsync(int parentCategoryId);
    }
}
