using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Admin.Interfaces;
using EventAgency.Web.ViewModels.Admin.EventManagement;
using EventAgency.Web.ViewModels.Event;
using Microsoft.EntityFrameworkCore;
using static EventAgency.GCommon.ApplicationConstants;

namespace EventAgency.Services.Core.Admin
{
    public class EventManagementService : IEventManagementService
    {
        private readonly IEventRepository eventRepository;

        public EventManagementService(IEventRepository eventRepository)
        {
            this.eventRepository = eventRepository;
        }
        public async Task<IEnumerable<EventManagementIndexViewModel>> GetAllEventsDataAsync()
        {
            IEnumerable<EventManagementIndexViewModel> allEvents = await this.eventRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .Select(c => new EventManagementIndexViewModel()
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Description = c.Description,
                    IsDeleted = c.IsDeleted,
                    ImageUrl = c.ImageUrl ?? $"/images/{NoImageUrl}"
                })
                .ToArrayAsync();

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

        public async Task<EventFormInputModel?> GetEditableEventByIdAsync(string? id)
        {
            EventFormInputModel? editableEvent = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid eventId);
            if (isIdValidGuid)
            {
                editableEvent = await this.eventRepository
                    .GetAllAttached()
                    .AsNoTracking()
                    .IgnoreQueryFilters()
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


        public Task<Tuple<bool, bool>> DeleteOrRestoreEventAsync(string? id)
        {
            throw new NotImplementedException();
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
