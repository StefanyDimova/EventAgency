using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Admin.Interfaces;
using EventAgency.Web.ViewModels.Admin.CategoryManagement;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventAgency.Services.Core.Admin
{
    public class CategoryManagementService : ICategoryManagementService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryManagementService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryManagementViewModel>> GetAllCategoriesAsync()
        {
            var allCategories = await this.categoryRepository
                .GetAllAttached()
                .AsNoTracking()
                .Include(c => c.SubCategories)
                .Where(c => c.ParentCategoryId == null)
                .Select(c => new CategoryManagementViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    SubCategories = c.SubCategories
                        .Select(sc => new SubCategoryManagementViewModel
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

        public async Task CreateCategoryAsync(CreateCategoryManagementFormModel model)
        {
            Category category = new Category()
            {
                Name = model.Name,
                ParentCategoryId = model.ParentCategoryId
            };

            await this.categoryRepository.AddAsync(category);
        }

    }
}
