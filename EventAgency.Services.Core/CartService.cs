using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Cart;
using Microsoft.EntityFrameworkCore;

using static EventAgency.GCommon.ApplicationConstants;

namespace EventAgency.Services.Core
{
    public class CartService : ICartService
    {
        private readonly ICartRepository cartRepository;
        private readonly IProductRepository productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            this.cartRepository = cartRepository;
            this.productRepository = productRepository;
        }
        public async Task<bool> AddProductToUserCartAsync(string? productId, string? userId, int quantity)
        {
            if (string.IsNullOrWhiteSpace(productId) || string.IsNullOrWhiteSpace(userId) || quantity < 1)
                return false;

            bool isProductGuidValid = Guid.TryParse(productId, out Guid productGuid);
            if (!isProductGuidValid)
                return false;

            // Вземаме продукта от репозитория за проверка на наличност
            Product? product = await this.productRepository.GetByIdAsync(productGuid);
            if (product == null || product.IsDeleted)
                return false;

            ApplicationUserProduct? userProductEntry = await this.cartRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(aup =>
                    aup.ApplicationUserId.ToLower() == userId.ToLower() &&
                    aup.ProductId == productGuid);

            if (userProductEntry != null)
            {
                // Проверка дали общото количество няма да превиши наличното
                if (userProductEntry.Quantity + quantity > product.Quantity)
                {
                    return false; // Няма достатъчно наличност
                }

                userProductEntry.IsDeleted = false;
                userProductEntry.Quantity += quantity;
                return await this.cartRepository.UpdateAsync(userProductEntry);
            }
            else
            {
                // Проверка дали исканото количество е повече от наличното
                if (quantity > product.Quantity)
                {
                    return false;
                }

                userProductEntry = new ApplicationUserProduct()
                {
                    ApplicationUserId = userId,
                    ProductId = productGuid,
                    Quantity = quantity,
                    IsDeleted = false
                };

                await this.cartRepository.AddAsync(userProductEntry);
                return true;
            }
        }

        public async Task<IEnumerable<CartItemViewModel>> GetUserCartAsync(string userId)
        {
            IEnumerable<CartItemViewModel> userCart = await this.cartRepository
                .GetAllAttached()
                .Include(aup => aup.Product)
                .AsNoTracking()
                .Where(aup => aup.ApplicationUserId.ToLower() == userId.ToLower())
                .Select(aup => new CartItemViewModel()
                {
                    ProductId = aup.ProductId.ToString(),
                    ProductName = aup.Product.Name,
                    Price = aup.Product.Price,
                    Quantity = aup.Quantity,
                    ImageUrl = aup.Product.ImageUrl ?? $"/images/{NoImageUrl}"
                })
                .ToArrayAsync();

            return userCart;
        }

        public async Task<bool> IsProductAddedToCart(string? productId, string? userId)
        {
            bool result = false;
            if (productId != null && userId != null)
            {
                bool isProductGuidValid = Guid.TryParse(productId, out Guid productGuid);
                if (isProductGuidValid)
                {
                    ApplicationUserProduct? userProductEntry = await this.cartRepository
                        .SingleOrDefaultAsync(aup => aup.ApplicationUserId.ToLower() == userId &&
                                                     aup.ProductId.ToString() == productGuid.ToString());
                    if (userProductEntry != null)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        public async Task<bool> RemoveProductFromCartAsync(string? productId, string? userId)
        {
            bool result = false;
            if (productId != null && userId != null)
            {
                bool isProductGuidValid = Guid.TryParse(productId, out Guid productGuid);
                if (isProductGuidValid)
                {
                    ApplicationUserProduct? userProductEntry = await this.cartRepository
                        .SingleOrDefaultAsync(aup => aup.ApplicationUserId.ToLower() == userId &&
                                                     aup.ProductId.ToString() == productGuid.ToString());
                    if (userProductEntry != null)
                    {
                        userProductEntry.Quantity = 0;
                        result = await this.cartRepository.DeleteAsync(userProductEntry);
                    }
                }
            }

            return result;
        }

        public async Task<bool> UpdateQuantityAsync(string userId, string productId, int quantity)
        {
            if (!Guid.TryParse(productId, out Guid productGuid))
                return false;

            var userProduct = await this.cartRepository
                .GetAllAttached()
                .Include(up => up.Product)
                .FirstOrDefaultAsync(up => up.ApplicationUserId == userId && up.ProductId == productGuid);

            if (userProduct == null)
                return false;

            // Проверка за наличност
            if (quantity > userProduct.Product.Quantity)
                return false;

            userProduct.Quantity = quantity;

            return await this.cartRepository.UpdateAsync(userProduct);
        }

        public async Task<bool> ClearUserCartAsync(string userId)
        {
            bool result = false;
            try
            {
                // Извличаме всички артикули в количката за даден потребител
                var cartItems = await cartRepository
                    .GetAllAttached()
                    .Where(c => c.ApplicationUserId.ToLower() == userId.ToLower())
                    .ToListAsync();

                if (cartItems.Any())
                {
                    foreach (var cartItem in cartItems) 
                    { 
                        await this.cartRepository.DeleteAsync(cartItem);
                    }
                    await cartRepository.SaveChangesAsync();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while clearing the cart: {ex.Message}");
                throw;
            }

            return result;
        }
    }
}
