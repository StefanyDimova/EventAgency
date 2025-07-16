using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgencyFinalProject.Data;
using Microsoft.EntityFrameworkCore;

namespace EventAgency.Data.Repository
{
    public class CategoryRepository : BaseRepository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(EventAgencyDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Category>> GetAllWithProductsAsync()
        {
            return await dbContext.Categories
                                  .Include(c => c.Products)
                                  .Where(c => !c.Products.All(p => p.IsDeleted))
                                  .ToListAsync();
        }
    }
}
