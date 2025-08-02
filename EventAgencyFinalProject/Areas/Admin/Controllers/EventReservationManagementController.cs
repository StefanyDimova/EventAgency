using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Admin.EventReservationManagement;
using Microsoft.AspNetCore.Mvc;

namespace EventAgency.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventReservationManagementController : Controller
    {
        private readonly IEventReservationRequestService requestService;

        public EventReservationManagementController(IEventReservationRequestService requestService)
        {
            this.requestService = requestService;
        }

        public async Task<IActionResult> Manage()
        {
            var reservations = await requestService.GetPendingRequestsAsync(); 
            var model = reservations.Select(r => new ReservationRequestViewModel
            {
                Id = r.Id.ToString(),
                RequestedDate = r.RequestedDate,
                EventType = r.EventType,
                IsApproved = r.IsApproved
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(string id)
        {
            await requestService.ApproveRequestAsync(id);
            return RedirectToAction(nameof(Manage));
        }
    }
}
