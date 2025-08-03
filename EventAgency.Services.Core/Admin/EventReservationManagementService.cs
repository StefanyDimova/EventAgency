using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Admin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Services.Core.Admin
{
    public class EventReservationManagementService : IEventReservationManagementService
    {
        private readonly IEventReservationRequestRepository eventReservationRequestRepository;

        public EventReservationManagementService(IEventReservationRequestRepository eventReservationRequestRepository)
        {
            this.eventReservationRequestRepository = eventReservationRequestRepository;
        }
        // Извличане на всички непотвърдени заявки
        public async Task<IEnumerable<EventReservationRequest>> GetPendingRequestsAsync()
        {
            IEnumerable<EventReservationRequest> pendingRequests = await this.eventReservationRequestRepository.GetAllPendingRequestsAsync();
            return pendingRequests;
        }

        public async Task ApproveRequestAsync(string id)
        {
            Guid newGuid = Guid.TryParse(id, out Guid parsedGuid) ? parsedGuid : Guid.Empty;
            var request = await eventReservationRequestRepository.GetByIdAsync(parsedGuid);

            if (request == null)
            {
                throw new ArgumentException("Заявката не е намерена.");
            }

            request.IsApproved = true;
            await eventReservationRequestRepository.SaveChangesAsync();
        }
    }
}
