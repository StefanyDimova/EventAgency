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
        public Task<bool> AddEventAsync(EventFormInputModel? inputModel)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, bool>> DeleteOrRestoreEventAsync(string? id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditEventAsync(EventFormInputModel? inputModel)
        {
            throw new NotImplementedException();
        }


        public Task<EventFormInputModel?> GetEventEditFormModelAsync(string? id)
        {
            throw new NotImplementedException();
        }
    }
}
