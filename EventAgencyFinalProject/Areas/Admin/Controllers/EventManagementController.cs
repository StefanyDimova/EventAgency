using EventAgency.Services.Core.Admin.Interfaces;
using EventAgency.Web.ViewModels.Admin.EventManagement;
using EventAgency.Web.ViewModels.Event;
using Microsoft.AspNetCore.Mvc;
using static EventAgency.Web.ViewModels.ValidationMessages.Event;

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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventFormInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                await this.eventManagementService.AddEventAsync(inputModel);
                return this.RedirectToAction(nameof(Manage));
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                Console.WriteLine(e.Message);

                this.ModelState.AddModelError(string.Empty, ServiceCreateError);
                return this.View(inputModel);
            }
        }
    }
}
