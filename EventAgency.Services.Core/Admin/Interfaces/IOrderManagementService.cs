using EventAgency.Web.ViewModels.Admin.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Services.Core.Admin.Interfaces
{
    public interface IOrderManagementService
    {
        Task<IEnumerable<OrderManagementViewModel>> GetAllOrdersForAdminAsync();
    }
}
