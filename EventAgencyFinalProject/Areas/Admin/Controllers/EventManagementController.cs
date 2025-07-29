using EventAgency.Services.Core.Admin.Interfaces;
using EventAgency.Web.ViewModels.Admin.EventManagement;
using Microsoft.AspNetCore.Mvc;

namespace EventAgency.Web.Areas.Admin.Controllers
{
    public class EventManagementController : BaseAdminController
    {
        private readonly IEventManagementService eventManagementService;

        public EventManagementController(IEventManagementService eventManagementService)
        {
            this.eventManagementService = eventManagementService;
        }
        public async Task<IActionResult> Manage()
        {
            IEnumerable<EventManagementIndexViewModel> allEvents = await this.eventManagementService
                .GetAllEventsDataAsync();
            return View(allEvents);
        }
    }
}
