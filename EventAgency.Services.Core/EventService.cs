using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Event;
using Microsoft.EntityFrameworkCore;
using static EventAgency.GCommon.ApplicationConstants;

namespace EventAgency.Services.Core
{
    public class EventService : IEventService

    {
        private readonly IEventRepository eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            this.eventRepository = eventRepository;
        }


        public async Task<IEnumerable<AllEventsViewModel>> GetAllEventsAsync()
        {
            IEnumerable<AllEventsViewModel> allEvents = await this.eventRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(newEvent => new AllEventsViewModel()
                {
                    Id = newEvent.Id.ToString(),
                    Name = newEvent.Name,
                    Description = newEvent.Description,
                    ImageUrl = newEvent.ImageUrl
                })
                .ToListAsync();


            foreach (AllEventsViewModel movie in allEvents)
            {
                if (String.IsNullOrEmpty(movie.ImageUrl))
                {
                    movie.ImageUrl = $"/images/{NoImageUrl}";
                }
            }

            return allEvents;
        }

        public async Task<DetailsEventViewModel> GetEventDetailsByIdAsync(string? id)
        {
            DetailsEventViewModel? eventDetails = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid eventId);

            if (isIdValidGuid)
            {
                eventDetails = await this.eventRepository
                    .GetAllAttached()
                    .AsNoTracking()
                    .Where(e => e.Id == eventId)
                    .Select(e => new DetailsEventViewModel()
                    {
                        Id = e.Id.ToString(),
                        Name = e.Name,
                        Description = e.Description,
                        ImageUrl = e.ImageUrl ?? $"/images/{NoImageUrl}"
                    })
                    .SingleOrDefaultAsync();
            }

            return eventDetails;
        }
    }
}
