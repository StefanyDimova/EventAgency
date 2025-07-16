using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Category;
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

    }
}
