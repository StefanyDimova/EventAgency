using EventAgency.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Data.Repository.Interfaces
{
    public interface ICartRepository : IRepository<ApplicationUserProduct, object>,
        IAsyncRepository<ApplicationUserProduct, object>
    {
        ApplicationUserProduct? GetByCompositeKey(string userId, string productId);
        Task<ApplicationUserProduct?> GetByCompositeKeyAsync(string userId, string productId);

        bool Exists(string userId, string productId);

        Task<bool> ExistsAsync(string userId, string productId);
    }
}
