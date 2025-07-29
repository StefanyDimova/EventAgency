using EventAgency.Web.ViewModels.Admin.EventManagement;
using EventAgency.Web.ViewModels.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Services.Core.Admin.Interfaces
{
    public interface IEventManagementService
    {
        Task<IEnumerable<EventManagementIndexViewModel>> GetAllEventsDataAsync();

        Task AddEventAsync(EventFormInputModel inputModel);

        Task<EventFormInputModel?> GetEventEditFormModelAsync(string? id);

        Task<bool> EditEventAsync(EventFormInputModel? inputModel);

        Task<Tuple<bool, bool>> DeleteOrRestoreEventAsync(string? id);
    }
}
