using EventAgency.Data.Models;

namespace EventAgency.Data.Repository.Interfaces
{
    public interface IEventRepository : IRepository<Event, Guid>, IAsyncRepository<Event, Guid>
    {
    }
}
