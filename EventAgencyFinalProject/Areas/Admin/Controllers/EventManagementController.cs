using EventAgency.Services.Core.Admin.Interfaces;
using EventAgency.Web.ViewModels.Admin.EventManagement;
using EventAgency.Web.ViewModels.Event;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
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

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            try
            {
                EventFormInputModel? editableEvent = await this.eventManagementService
                    .GetEditableEventByIdAsync(id);
                if (editableEvent == null)
                {
                    return this.RedirectToAction("Home", "Error");
                }

                return this.View(editableEvent);
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                // TODO: Add JS bars to indicate such errors
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Manage));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EventFormInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                bool editSuccess = await this.eventManagementService.EditEventAsync(inputModel);
                if (!editSuccess)
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 404 });
                }

                return this.RedirectToAction(nameof(Manage));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Manage));
            }
        }
    }
}
