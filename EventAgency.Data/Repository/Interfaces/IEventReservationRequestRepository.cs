using EventAgency.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Data.Repository.Interfaces
{
    public interface IEventReservationRequestRepository : 
        IRepository<EventReservationRequest, Guid>, 
        IAsyncRepository<EventReservationRequest, Guid>
    {
        Task<EventReservationRequest?> GetRequestByDateAsync(DateTime date);

        Task<IEnumerable<EventReservationRequest>> GetAllPendingRequestsAsync();

        Task AddRequestAsync(EventReservationRequest request);

        Task<IEnumerable<EventReservationRequest>> GetAllApprovedAsync();
    }
}
