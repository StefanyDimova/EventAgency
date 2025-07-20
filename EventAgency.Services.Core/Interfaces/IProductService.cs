using EventAgency.Web.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Services.Core.Interfaces
{
    public interface IProductService
    {
        Task AddProductAsync(AddProductInputModel inputModel);

        Task<IEnumerable<AllProductsViewModel>> GetAllProductsAsync();

        Task<IEnumerable<AllProductsViewModel>> GetProductsByCategoryIdAsync(int categoryId);
    }
}
