using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Admin.Interfaces;
using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Admin.ProductManagement;
using EventAgency.Web.ViewModels.Product;
using Microsoft.EntityFrameworkCore;

using static EventAgency.GCommon.ApplicationConstants;

namespace EventAgency.Services.Core.Admin
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryService categoryService;
        private readonly IProductService productService;

        public ProductManagementService(IProductRepository productRepository, ICategoryService categoryService, IProductService productService)
        {
            this.productRepository = productRepository;
            this.categoryService = categoryService;
            this.productService = productService;
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

        public async Task<Tuple<bool, bool>> DeleteOrRestoreProductAsync(string? id)
        {
            bool result = false;
            bool isRestored = false;
            if (!String.IsNullOrWhiteSpace(id))
            {
                Product? product = await this.productRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(p => p.Id.ToString().ToLower() == id.ToLower());
                if (product != null)
                {
                    if (product.IsDeleted)
                    {
                        isRestored = true;
                    }

                    product.IsDeleted = !product.IsDeleted;

                    result = await this.productRepository
                        .UpdateAsync(product);
                }
            }

            return new Tuple<bool, bool>(result, isRestored);
        }

    }
}
