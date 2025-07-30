using EventAgency.Web.ViewModels.Admin.EventManagement;
using EventAgency.Web.ViewModels.Event;

namespace EventAgency.Services.Core.Admin.Interfaces
{
    public interface IEventManagementService
    {
        Task<IEnumerable<EventManagementIndexViewModel>> GetAllEventsDataAsync();

        Task AddEventAsync(EventFormInputModel inputModel);

        Task<EventFormInputModel?> GetEditableEventByIdAsync(string? id);

        Task<bool> EditEventAsync(EventFormInputModel inputModel);

        Task<Tuple<bool, bool>> DeleteOrRestoreEventAsync(string? id);
    }
}
