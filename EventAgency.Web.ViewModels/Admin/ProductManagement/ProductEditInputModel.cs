using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Web.ViewModels.Admin.ProductManagement
{
    public class ProductEditInputModel
    {
        public string Id { get; set; }
            = string.Empty;

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IEnumerable<AddProductCategoryDropDownModel>? Categories { get; set; }

        public IEnumerable<AddProductCategoryDropDownModel>? SubCategories { get; set; }

        [Required]
        public int? SubCategoryId { get; set; }

    }
}
