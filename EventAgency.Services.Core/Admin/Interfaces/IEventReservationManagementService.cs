using EventAgency.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Services.Core.Admin.Interfaces
{
    public interface IEventReservationManagementService
    {
        Task<IEnumerable<EventReservationRequest>> GetPendingRequestsAsync();

        Task ApproveRequestAsync(string id);
    }
}
