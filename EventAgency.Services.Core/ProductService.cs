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
        private readonly ICategoryService categoryService;

        public ProductService(IProductRepository productRepository, ICategoryService categoryService)
        {
            this.productRepository = productRepository;
            this.categoryService = categoryService;
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
                SubCategoryId = inputModel.SubCategoryId
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
                Price = p.Price
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

        public async Task<ProductEditInputModel?> GetEditableProductByIdAsync(string? id)
        {
            ProductEditInputModel? editableProduct = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid productId);
            if (!isIdValidGuid)
            {
                return null;
            }
            var productEntity = await this.productRepository
                .GetAllAttached()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (productEntity == null)
            {
                return null;
            }
            var categories = await this.categoryService.GetCategoriesDropdownDataAsync();

            var subcategories = await this.categoryService.GetSubCategoriesDropdownDataAsync(productEntity.CategoryId);

            editableProduct = new ProductEditInputModel()
            {
                Name = productEntity.Name,
                Description = productEntity.Description,
                Price = productEntity.Price,
                Quantity = productEntity.Quantity,
                ImageUrl = productEntity.ImageUrl ?? $"/images/{NoImageUrl}",
                CategoryId = productEntity.CategoryId,
                SubCategoryId = productEntity.SubCategoryId,
                Categories = categories,
                SubCategories = subcategories
            };

            return editableProduct;
        }


        public async Task<bool> EditProductAsync(ProductEditInputModel inputModel)
        {
            Product? editableProduct = await this.FindProductByStringId(inputModel.Id);

            bool result = false;
            if (editableProduct == null)
            {
                return false;
            }

            editableProduct.Name = inputModel.Name;
            editableProduct.Description = inputModel.Description;
            editableProduct.ImageUrl = inputModel.ImageUrl ?? $"/images/{NoImageUrl}";
            editableProduct.Quantity = inputModel.Quantity;
            editableProduct.Price = inputModel.Price;
            editableProduct.CategoryId = inputModel.CategoryId;
            editableProduct.SubCategoryId = inputModel.SubCategoryId;

            result = await this.productRepository.UpdateAsync(editableProduct);

            return result;
        }



        public async Task<DeleteProductViewModel?> GetProductDeleteDetailsByIdAsync(string? id)
        {
            DeleteProductViewModel? deleteProductViewModel = null;

            Product? productToBeDeleted = await this.FindProductByStringId(id);
            if (productToBeDeleted != null)
            {
                deleteProductViewModel = new DeleteProductViewModel()
                {
                    Id = productToBeDeleted.Id.ToString(),
                    Name = productToBeDeleted.Name,
                    ImageUrl = productToBeDeleted.ImageUrl ?? $"/images/{NoImageUrl}"
                    
                };
            }

            return deleteProductViewModel;
        }

        public async Task<bool> SoftDeleteProductAsync(string? id)
        {
            bool result = false;
            Product? productToDelete = await this.FindProductByStringId(id);

            if (productToDelete == null)
            {
                return false;
            }

            result = await this.productRepository.DeleteAsync(productToDelete);
            return result;
        }

        public async Task<bool> DeleteProductAsync(string? id)
        {
            Product? productToDelete = await this.FindProductByStringId(id);

            if (productToDelete == null)
            {
                return false;
            }
            await this.productRepository.HardDeleteAsync(productToDelete);

            return true;
        }

        private async Task<Product?> FindProductByStringId(string? id)
        {
            Product? product = null;

            if (!string.IsNullOrWhiteSpace(id))
            {
                bool isGuidValid = Guid.TryParse(id, out Guid productGuid);
                if (isGuidValid)
                {
                    product = await this.productRepository.GetByIdAsync(productGuid);
                }
            }

            return product;
        }

    }
}
