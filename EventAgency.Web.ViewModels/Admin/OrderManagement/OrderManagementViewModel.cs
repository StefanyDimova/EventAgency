using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Web.ViewModels.Admin.OrderManagement
{
    public class OrderManagementViewModel
    {
        public string Id { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public DateTime CreatedAt { get; set; } 
        public decimal TotalPrice { get; set; } 
        public bool IsConfirmed { get; set; }   
        public bool IsCancelled { get; set; }   
    }
}
