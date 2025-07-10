using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static EventAgency.Web.ViewModels.ValidationMessages.Event;

namespace EventAgency.Web.Controllers
{
    public class EventController : BaseController
    {

        private readonly IEventService eventService;

        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllEventsViewModel> allEvents = await this.eventService
                .GetAllEventsAsync();

            return View(allEvents);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(EventFormInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                await this.eventService.AddEventAsync(inputModel);
                return this.RedirectToAction(nameof(Index));
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
        [AllowAnonymous]
        public async Task<IActionResult> Details(string? id)
        {
            try
            {
                DetailsEventViewModel? eventDetails = await this.eventService
                    .GetEventDetailsByIdAsync(id);

                if (eventDetails == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(eventDetails);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
