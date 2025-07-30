using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
