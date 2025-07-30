using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Web.ViewModels.Admin.CategoryManagement
{
    public class SubCategoryManagementViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
