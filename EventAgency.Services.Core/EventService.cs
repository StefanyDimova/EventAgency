using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Event;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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


        public async Task AddEventAsync(EventFormInputModel inputModel)
        {
            Event newEvent = new Event()
            {
                Name = inputModel.Name,
                Description = inputModel.Description,
                ImageUrl = inputModel.ImageUrl
            };

            await this.eventRepository.AddAsync(newEvent);
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

        public async Task<EventFormInputModel?> GetEditableEventByIdAsync(string? id)
        {
            EventFormInputModel? editableEvent = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid eventId);
            if (isIdValidGuid)
            {
                editableEvent = await this.eventRepository
                    .GetAllAttached()
                    .AsNoTracking()
                    .Where(e => e.Id == eventId)
                    .Select(e => new EventFormInputModel()
                    {
                        Name = e.Name,
                        Description = e.Description,
                        ImageUrl = e.ImageUrl ?? $"/images/{NoImageUrl}"
                    })
                    .SingleOrDefaultAsync();
            }

            return editableEvent;
        }

        public async Task<bool> EditEventAsync(EventFormInputModel inputModel)
        {
            Event? editableEvent = await this.FindEventByStringId(inputModel.Id);

            bool result = false;
            if (editableEvent == null)
            {
                return false;
            }

            editableEvent.Name = inputModel.Name;
            editableEvent.Description = inputModel.Description;
            editableEvent.ImageUrl = inputModel.ImageUrl ?? $"/images/{NoImageUrl}";

            result = await this.eventRepository.UpdateAsync(editableEvent);

            return result;
        }



        private async Task<Event?> FindEventByStringId(string? id)
        {
            Event? newevent = null;

            if (!string.IsNullOrWhiteSpace(id))
            {
                bool isGuidValid = Guid.TryParse(id, out Guid eventGuid);
                if (isGuidValid)
                {
                    newevent = await this.eventRepository.GetByIdAsync(eventGuid);
                }
            }

            return newevent;
        }
    }
}
