using System.ComponentModel.DataAnnotations;
using static EventAgency.Data.Common.EntityConstants.Category;
using static EventAgency.Web.ViewModels.ValidationMessages.Category;

namespace EventAgency.Web.ViewModels.Admin.CategoryManagement
{
    public class SubCategoryManagementViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = CategoryNameRequiredMessage)]
        [MinLength(NameMinLength, ErrorMessage = CategoryNameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = CategoryNameMaxLengthMessage)]
        public string Name { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
