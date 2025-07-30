using EventAgency.Web.ViewModels.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Services.Core.Interfaces
{
    public interface IEventService 
    {
        Task<IEnumerable<AllEventsViewModel>> GetAllEventsAsync();

        Task<DetailsEventViewModel> GetEventDetailsByIdAsync(string? id);

        Task<bool> SoftDeleteEventAsync(string? id);
        Task<bool> DeleteEventAsync(string? id);
        Task<DeleteEventViewModel?> GetEventDeleteDetailsByIdAsync(string? id);
    }
}
