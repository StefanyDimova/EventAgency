using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EventAgency.Web.ViewModels.Admin.CategoryManagement
{
    public class CreateCategoryManagementFormModel
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        public int? ParentCategoryId { get; set; }
        public IEnumerable<SelectListItem> AvailableCategories { get; set; } = new List<SelectListItem>();
    }
}
