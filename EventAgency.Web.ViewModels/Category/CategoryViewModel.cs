using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Web.ViewModels.Category
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public List<SubcategoryViewModel> SubCategories { get; set; } = new();
    }
}
