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
    }
}
