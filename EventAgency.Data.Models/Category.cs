using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        // главна категория , която може да е null , защото тази която създаваме в момента може да е тази parentCategory
        public int? ParentCategoryId { get; set; }

        public virtual Category? ParentCategory { get; set; }

        // подкатегории
        public virtual ICollection<Category> SubCategories { get; set; }
                = new HashSet<Category>();

        public virtual ICollection<Product> Products { get; set; } 
                = new HashSet<Product>();

        public bool IsDeleted { get; set; }
    }
}
