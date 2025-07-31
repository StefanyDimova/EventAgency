using System.ComponentModel.DataAnnotations;

namespace EventAgency.Web.ViewModels.Admin.ProductManagement
{
    public class ProductEditInputModel : AddProductInputModel
    {
        [Required]
        public string Id { get; set; }
            = string.Empty;

        public IEnumerable<AddProductCategoryDropDownModel>? SubCategories { get; set; }

        [Required]
        public int? SubCategoryId { get; set; }

    }
}
