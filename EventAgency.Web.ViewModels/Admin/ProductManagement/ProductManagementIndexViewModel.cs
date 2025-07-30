using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Web.ViewModels.Admin.ProductManagement
{
    public class ProductManagementIndexViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; } = null!;
        public string? SubCategory { get; set; }
        public bool IsDeleted { get; set; }
        public string? ImageUrl { get; set; }
    }
}
