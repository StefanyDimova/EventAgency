using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Admin.ProductManagement;
using EventAgency.Web.ViewModels.Category;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventAgency.Services.Core
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            var allCategories = await this.categoryRepository
                .GetAllAttached()
                .AsNoTracking()
                .Include(c => c.SubCategories)
                .Where(c => c.ParentCategoryId == null) // Само основните категории
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    SubCategories = c.SubCategories
                        .Select(sc => new SubcategoryViewModel
                        {
                            Id = sc.Id,
                            Name = sc.Name
                        })
                        .ToList()
                })
                .ToListAsync();

            return allCategories;
        }

        public async Task<IEnumerable<SelectListItem>> GetCategorySelectListAsync()
        {
            var categories = await this.categoryRepository
                .GetAllAttached()
                .Where(c => c.ParentCategoryId == null)
                .ToListAsync();
                
               return categories
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });

        }


        public async Task CreateCategoryAsync(CategoryFormModel model)
        {
            Category category = new Category()
            {
                Name = model.Name,
                ParentCategoryId = model.ParentCategoryId
            };

            await this.categoryRepository.AddAsync(category);
        }

        public async Task<IEnumerable<AddProductCategoryDropDownModel>> GetCategoriesDropdownDataAsync()
        {
            IEnumerable<AddProductCategoryDropDownModel> categoriesDropDown = await this.categoryRepository
                .GetAllAttached()
                .Where(c => c.ParentCategoryId == null)
                .Select(t => new AddProductCategoryDropDownModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                })
                .ToArrayAsync();

            return categoriesDropDown;
        }

        public async Task<IEnumerable<AddProductCategoryDropDownModel>> GetSubCategoriesDropdownDataAsync(int parentCategoryId)
        {

            IEnumerable<AddProductCategoryDropDownModel> subCategoriesDropDown = await this.categoryRepository
                .GetAllAttached()
                .Where(c => c.ParentCategoryId == parentCategoryId)
                .Select(subcategory => new AddProductCategoryDropDownModel()
                {
                    Id= subcategory.Id,
                    Name = subcategory.Name,
                })
                .ToArrayAsync();

            return subCategoriesDropDown;
        }
    }
}
