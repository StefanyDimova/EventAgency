using Microsoft.AspNetCore.Mvc;

namespace EventAgency.Web.Controllers
{
    public class CalendarController : BaseController
    {
        public IActionResult Calendar()
        {
            return View("Calendar");
        }
    }
}
