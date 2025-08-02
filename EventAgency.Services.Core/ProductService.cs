using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Admin.ProductManagement;
using EventAgency.Web.ViewModels.Product;
using Microsoft.EntityFrameworkCore;

using static EventAgency.GCommon.ApplicationConstants;

namespace EventAgency.Services.Core
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<IEnumerable<AllProductsViewModel>> GetAllProductsAsync()
        {
            IEnumerable<AllProductsViewModel> allProducts = await this.productRepository
               .GetAllAttached()
               .AsNoTracking()
               .Select(product => new AllProductsViewModel()
               {
                   Id = product.Id.ToString(),
                   Name = product.Name,
                   ImageUrl = product.ImageUrl,
                   Price = product.Price,
                   Quantity = product.Quantity
               })
               .ToListAsync();

            foreach (AllProductsViewModel product in allProducts)
            {
                if (String.IsNullOrEmpty(product.ImageUrl))
                {
                    product.ImageUrl = $"/images/{NoImageUrl}";
                }
            }

            return allProducts;
        }


        public async Task<IEnumerable<AllProductsViewModel>> GetProductsBySubCategoryIdAsync(int subCategoryId)
        {
            var products = await this.productRepository
                .GetAllAttached()
                .Where(p => p.SubCategoryId == subCategoryId && !p.IsDeleted)
                .ToListAsync();

            return products.Select(p => new AllProductsViewModel
            {
                Id = p.Id.ToString(),
                Name = p.Name,
                ImageUrl = string.IsNullOrEmpty(p.ImageUrl) ? $"/images/{NoImageUrl}" : p.ImageUrl,
                Price = p.Price,
                Quantity = p.Quantity
            });
        }
        public async Task<ProductDetailsViewModel> GetProductDetailsByIdAsync(string? id)
        {
            ProductDetailsViewModel? productDetails = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid productId);

            if (isIdValidGuid)
            {
                productDetails = await this.productRepository
                    .GetAllAttached()
                    .AsNoTracking()
                    .Where(p => p.Id == productId)
                    .Select(p => new ProductDetailsViewModel()
                    {
                        Id = p.Id.ToString(),
                        Name = p.Name,
                        Description = p.Description,
                        CategoryName = p.Category.Name,
                        ImageUrl = p.ImageUrl ?? $"/images/{NoImageUrl}",
                        Price = p.Price,
                        Quantity = p.Quantity
                    })
                    .SingleOrDefaultAsync();
            }

            return productDetails;
        }

    }
}
