using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgencyFinalProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Data.Repository
{
    public class OrderRepository : BaseRepository<Order, Guid>, IOrderRepository
    {
        public OrderRepository(EventAgencyDbContext dbContext) : base(dbContext)
        {
        }
    }
}
