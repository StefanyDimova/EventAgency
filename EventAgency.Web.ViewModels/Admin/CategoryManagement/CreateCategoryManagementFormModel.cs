using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

using static EventAgency.Web.ViewModels.ValidationMessages.Category;
using static EventAgency.Data.Common.EntityConstants.Category;

namespace EventAgency.Web.ViewModels.Admin.CategoryManagement
{
    public class CreateCategoryManagementFormModel
    {
        [Required(ErrorMessage = CategoryNameRequiredMessage)]
        [MinLength(NameMinLength, ErrorMessage = CategoryNameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = CategoryNameMaxLengthMessage)]
        public string Name { get; set; } = null!;

        public int? ParentCategoryId { get; set; }
        public IEnumerable<SelectListItem> AvailableCategories { get; set; } = new List<SelectListItem>();
    }
}
