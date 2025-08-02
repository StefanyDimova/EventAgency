using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Services.Core
{
    public class EventReservationRequestService : IEventReservationRequestService
    {
        private readonly IEventReservationRequestRepository eventReservationRequestRepository;

        public EventReservationRequestService(IEventReservationRequestRepository eventReservationRequestRepository)
        {
            this.eventReservationRequestRepository = eventReservationRequestRepository;
        }

        public async Task<EventReservationRequest> AddRequestAsync(DateTime requestedDate, string eventType)
        {
            EventReservationRequest request = new EventReservationRequest
            {
                RequestedDate = requestedDate,
                EventType = eventType,
                IsApproved = false // Заявката започва като непотвърдена
            };

            await this.eventReservationRequestRepository.AddRequestAsync(request);
            return request;
        }

        // Извличане на всички непотвърдени заявки
        public async Task<IEnumerable<EventReservationRequest>> GetPendingRequestsAsync()
        {
            IEnumerable<EventReservationRequest> pendingRequests = await this.eventReservationRequestRepository.GetAllPendingRequestsAsync();
            return pendingRequests;
        }

        // Извличане на заявка по дата
        public async Task<EventReservationRequest?> GetRequestByDateAsync(DateTime date)
        {
            // Извикваме метода на репозитория за извличане на заявката по дата
            return await eventReservationRequestRepository.GetRequestByDateAsync(date);
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
