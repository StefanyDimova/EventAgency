using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Admin.ProductManagement;

namespace EventAgency.Services.Core.Admin.Interfaces
{
    public interface IProductManagementService : IProductService
    {
        Task<IEnumerable<ProductManagementIndexViewModel>> GetProductManagementDataAsync();

        Task AddProductAsync(AddProductInputModel inputModel);

        Task<ProductEditInputModel?> GetEditableProductByIdAsync(string? id);

        Task<bool> EditProductAsync(ProductEditInputModel inputModel);
    }
}
