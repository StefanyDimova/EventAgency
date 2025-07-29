using EventAgency.Web.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Services.Core.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartItemViewModel>> GetUserCartAsync(string userId);

        Task<bool> AddProductToUserCartAsync(string? productId, string? userId, int quantity);

        Task<bool> RemoveProductFromCartAsync(string? productId, string? userId);

        Task<bool> IsProductAddedToCart(string? productId, string? userId);

        Task<bool> UpdateQuantityAsync(string userId, string productId, int quantity);
    }
}
