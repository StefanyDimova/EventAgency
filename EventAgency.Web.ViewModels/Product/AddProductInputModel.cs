using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EventAgency.Web.ViewModels.Product
{
    public class AddProductInputModel
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        public string? ImageUrl { get; set; }

        public IEnumerable<AddProductCategoryDropDownModel>? Categories { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IEnumerable<AddProductCategoryDropDownModel>? SubCategories { get; set; }

        [Required]
        public int SubCategoryId { get; set; }
    }
}
