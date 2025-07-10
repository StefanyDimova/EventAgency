using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgencyFinalProject.Data;

namespace EventAgency.Data.Repository
{
    public class EventRepository : BaseRepository<Event, Guid>, IEventRepository
    {
        public EventRepository(EventAgencyDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
