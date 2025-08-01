using EventAgency.Web.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Web.ViewModels.Admin.OrderManagement
{
    public class OrderManagementDetailsViewModel
    {
        public string OrderId { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsCancelled { get; set; }
        public List<OrderManagementItemViewModel> OrderItems { get; set; } 
            = new List<OrderManagementItemViewModel>();
    }
}
