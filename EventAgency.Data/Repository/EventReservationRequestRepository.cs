using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgencyFinalProject.Data;
using Microsoft.EntityFrameworkCore;

namespace EventAgency.Data.Repository
{
    public class EventReservationRequestRepository : BaseRepository<EventReservationRequest, Guid>, IEventReservationRequestRepository
    {
        public EventReservationRequestRepository(EventAgencyDbContext dbContext) : base(dbContext)
        {
        }

        public async Task AddRequestAsync(EventReservationRequest request)
        {
            await this.AddAsync(request);
        }

        public async Task<IEnumerable<EventReservationRequest>> GetAllPendingRequestsAsync()
        {           
               return await this.GetAllAttached()
                .Where(r => !r.IsApproved)
                .ToListAsync();
        }

        public async Task<EventReservationRequest?> GetRequestByDateAsync(DateTime date)
        {
            // Преобразуваме дата без време за сравнение
            var result = await this.dbContext.EventReservationRequests
                .Where(r => r.RequestedDate.Date == date.Date && !r.IsApproved)
                .FirstOrDefaultAsync(); // Връща първото съвпадение или null
            return result;
        }

    }
}
