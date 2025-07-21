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

        Task<ProductDetailsViewModel> GetProductDetailsByIdAsync(string? id);

        Task<ProductEditInputModel?> GetEditableProductByIdAsync(string? id);

        Task<bool> EditProductAsync(ProductEditInputModel inputModel);

        Task<DeleteProductViewModel?> GetProductDeleteDetailsByIdAsync(string? id);

        Task<bool> SoftDeleteProductAsync(string? id);
        Task<bool> DeleteProductAsync(string? id);
    }
}
