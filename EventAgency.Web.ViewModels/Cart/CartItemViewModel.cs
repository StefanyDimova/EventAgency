using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Web.ViewModels.Cart
{
    public class CartItemViewModel
    {
        public string ProductId { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }


    }
}
