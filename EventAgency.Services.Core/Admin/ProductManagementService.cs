using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Admin.Interfaces;
using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Admin.ProductManagement;
using Microsoft.EntityFrameworkCore;

using static EventAgency.GCommon.ApplicationConstants;

namespace EventAgency.Services.Core.Admin
{
    public class ProductManagementService : ProductService, IProductManagementService
    {
        private readonly IProductService productService;
        private readonly IProductRepository productRepository;
        public ProductManagementService(IProductRepository productRepository, ICategoryService categoryService, IProductService productService) 
            : base(productRepository, categoryService)
        {
            this.productService = productService;
            this.productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductManagementIndexViewModel>> GetProductManagementDataAsync()
        {
            IEnumerable<ProductManagementIndexViewModel> allProducts = await this.productRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .Select(p => new ProductManagementIndexViewModel()
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Category = p.Category.Name,
                    SubCategory = p.SubCategory.Name,
                    IsDeleted = p.IsDeleted,
                    ImageUrl = p.ImageUrl ?? $"/images/{NoImageUrl}"

                })
                .ToArrayAsync();

            return allProducts;
        }
    }
}
