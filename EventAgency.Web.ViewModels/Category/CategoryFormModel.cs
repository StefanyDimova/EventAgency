using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EventAgency.Web.ViewModels.Category
{
    public class CategoryFormModel
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        public int? ParentCategoryId { get; set; }

        // За dropdown списък в изгледа
        public IEnumerable<SelectListItem> AvailableCategories { get; set; } = new List<SelectListItem>();
    }
}