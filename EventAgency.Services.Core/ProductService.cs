using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Interfaces;
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
        public async Task AddProductAsync(AddProductInputModel inputModel)
        {
            Product newProduct = new Product()
            {
                Name = inputModel.Name,
                Description = inputModel.Description,
                ImageUrl = inputModel.ImageUrl,
                Price = inputModel.Price,
                Quantity = inputModel.Quantity,
                CategoryId = inputModel.CategoryId,
            };

            await this.productRepository.AddAsync(newProduct);
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
                   Price = product.Price
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

        public async Task<IEnumerable<AllProductsViewModel>> GetProductsByCategoryIdAsync(int categoryId)
        {
            var products = await this.productRepository
                 .GetAllAttached()
                 .Where(p => p.CategoryId == categoryId && !p.IsDeleted)
                 .ToListAsync();

            return products.Select(p => new AllProductsViewModel
            {
                Id = p.Id.ToString(),
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                Price = p.Price
            });
        }
    }
}
