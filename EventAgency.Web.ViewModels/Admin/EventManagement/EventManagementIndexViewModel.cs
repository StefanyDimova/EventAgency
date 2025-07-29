using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Web.ViewModels.Admin.EventManagement
{
    public class EventManagementIndexViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsDeleted { get; set; } 
    }
}
