using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgencyFinalProject.Data;
using Microsoft.EntityFrameworkCore;

namespace EventAgency.Data.Repository
{
    public class CartRepository : BaseRepository<ApplicationUserProduct, object>, ICartRepository
    {

        public CartRepository(EventAgencyDbContext dbContext) : base(dbContext)
        {
        }

        public bool Exists(string userId, string productId)
        {
            return this
                .GetAllAttached()
                .Any(ap => ap.ApplicationUserId.ToLower() == (userId.ToLower()) &&
                                ap.ProductId.ToString().ToLower() == (productId.ToLower()));
        }

        public Task<bool> ExistsAsync(string userId, string productId)
        {
            return this
               .GetAllAttached()
               .AnyAsync(ap => ap.ApplicationUserId.ToLower() == (userId.ToLower()) &&
                               ap.ProductId.ToString().ToLower() == (productId.ToLower()));
        }

        public ApplicationUserProduct? GetByCompositeKey(string userId, string productId)
        {
            return this
                .GetAllAttached()
                .SingleOrDefault(ap => ap.ApplicationUserId.ToLower() == (userId.ToLower()) &&
                                ap.ProductId.ToString().ToLower() == (productId.ToLower()));
        }

        public Task<ApplicationUserProduct?> GetByCompositeKeyAsync(string userId, string productId)
        {
            return this
                .GetAllAttached()
                .SingleOrDefaultAsync(ap => ap.ApplicationUserId.ToLower() == (userId.ToLower()) &&
                                ap.ProductId.ToString().ToLower() == (productId.ToLower()));
        }
    }
}
