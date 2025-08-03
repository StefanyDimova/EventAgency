using EventAgency.Services.Core.Admin.Interfaces;
using EventAgency.Web.ViewModels.Admin.EventReservationManagement;
using Microsoft.AspNetCore.Mvc;

namespace EventAgency.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventReservationManagementController : BaseAdminController
    {
        private readonly IEventReservationManagementService requestService;

        public EventReservationManagementController(IEventReservationManagementService requestService)
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
                IsApproved = r.IsApproved,
                UserEmail = r.UserEmail ?? "Unknown"
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
