using Microsoft.AspNetCore.Mvc;

namespace EventAgency.Web.Controllers
{
    public class EventController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
