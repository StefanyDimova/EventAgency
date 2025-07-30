using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Admin.ProductManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Services.Core.Admin.Interfaces
{
    public interface IProductManagementService : IProductService
    {
        Task<IEnumerable<ProductManagementIndexViewModel>> GetProductManagementDataAsync();
    }
}
