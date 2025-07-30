using EventAgency.Services.Core.Admin.Interfaces;
using EventAgency.Web.ViewModels.Admin.EventManagement;
using EventAgency.Web.ViewModels.Event;
using Microsoft.AspNetCore.Mvc;
using static EventAgency.Web.ViewModels.ValidationMessages.Event;

using static EventAgency.GCommon.ApplicationConstants;

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

        [HttpGet]
        public async Task<IActionResult> ToggleDelete(string? id)
        {
            Tuple<bool, bool> opResult = await this.eventManagementService
                .DeleteOrRestoreEventAsync(id);
            bool success = opResult.Item1;
            bool isRestored = opResult.Item2;

            if (!success)
            {
                TempData[ErrorMessageKey] = "Event could not be found and updated!";
            }
            else
            {
                string operation = isRestored ? "restored" : "deleted";

                TempData[SuccessMessageKey] = $"Event {operation} successfully!";
            }

            return this.RedirectToAction(nameof(Manage));
        }
    }
}
