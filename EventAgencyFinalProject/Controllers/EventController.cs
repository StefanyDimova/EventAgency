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
            try
            {
                IEnumerable<AllEventsViewModel> allEvents = await this.eventService.GetAllEventsAsync();
                return View(allEvents);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
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
                    return RedirectToAction("Error", "Home", new { statusCode = 404 });
                }

                return this.View(eventDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }
        }

    }
}
