namespace EventAgency.Web.ViewModels.Admin.CategoryManagement
{
    public class CategoryManagementViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public List<SubCategoryManagementViewModel> SubCategories { get; set; } = new List<SubCategoryManagementViewModel>();
    }
}
