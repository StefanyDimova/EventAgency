using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

using static EventAgency.Web.ViewModels.ValidationMessages.Product;
using static EventAgency.Data.Common.EntityConstants.Product;

namespace EventAgency.Web.ViewModels.Admin.ProductManagement
{
    public class AddProductInputModel
    {
        [Required(ErrorMessage = ProductNameRequiredMessage)]
        [MinLength(NameMinLength, ErrorMessage = ProductNameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = ProductNameMaxLengthMessage)]
        [Display(Name = "Име на продукта")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = ProductDescriptionRequiredMessage)]
        [MinLength(DescriptionMinLength, ErrorMessage = ProductDescriptionMinLengthMessage)]
        [MaxLength(DescriptionMaxLength, ErrorMessage = ProductDescriptionMaxLengthMessage)]
        [Display(Name = "Описание")]
        public string Description { get; set; } = null!;

        [Required]
        [Range(0.01, double.MaxValue)]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "Количество")]
        public int Quantity { get; set; }

        [MaxLength(ImageUrlMaxLength, ErrorMessage = ImageUrlMaxLengthMessage)]
        [Display(Name = "Снимка")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Категории")]

        public IEnumerable<AddProductCategoryDropDownModel>? Categories { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Display(Name = "Подкатегории")]
        public IEnumerable<AddProductCategoryDropDownModel>? SubCategories { get; set; }

        [Required]
        public int SubCategoryId { get; set; }
    }
}
