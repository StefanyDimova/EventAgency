using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Web.ViewModels.Product
{
    public class ProductDetailsViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = null!;

        public int Quantity { get; set; } // наличност
    }
}
