using EventAgency.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Services.Core.Interfaces
{
    public interface IEventReservationRequestService
    {
        Task<EventReservationRequest> AddRequestAsync(DateTime requestedDate, string eventType);
        Task<IEnumerable<EventReservationRequest>> GetPendingRequestsAsync();
        Task<EventReservationRequest?> GetRequestByDateAsync(DateTime date);
    }
}
