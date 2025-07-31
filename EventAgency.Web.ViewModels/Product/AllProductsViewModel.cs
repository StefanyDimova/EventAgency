using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Web.ViewModels.Product
{
    public class AllProductsViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; } // В лева

        public int Quantity { get; set; }
        public decimal PriceInEuro => Math.Round(Price / 1.95583m, 2);
    }
}
