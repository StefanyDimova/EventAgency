using EventAgency.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Data.Repository.Interfaces
{
    public interface IProductRepository : IRepository<Product, Guid>, IAsyncRepository<Product, Guid>
    {
    }
}
