using System.ComponentModel.DataAnnotations;

using static EventAgency.Web.ViewModels.ValidationMessages.Category;
using static EventAgency.Data.Common.EntityConstants.Category;

namespace EventAgency.Web.ViewModels.Admin.ProductManagement
{
    public class AddProductCategoryDropDownModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = CategoryNameRequiredMessage)]
        [MinLength(NameMinLength, ErrorMessage = CategoryNameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = CategoryNameMaxLengthMessage)]
        public string Name { get; set; } = null!;
    }
}
