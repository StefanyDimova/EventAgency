using EventAgency.Data.Models;
using EventAgency.Services.Core.Admin.Interfaces;
using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.EventReservation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventAgency.WebApi.Controllers
{
    [Route("api/EventReservationRequestsApi")]
    [ApiController]
    [AllowAnonymous]
    public class EventReservationRequestsApiController : ControllerBase
    {
        private readonly IEventReservationManagementService eventReservationRequestService;
        private readonly IEventReservationRequestService eventReservationRequest;

        public EventReservationRequestsApiController(IEventReservationManagementService eventReservationRequestService, IEventReservationRequestService eventReservationRequest)
        {
                this.eventReservationRequestService = eventReservationRequestService;
                this.eventReservationRequest = eventReservationRequest;
        }

        [HttpPost]
        public async Task<ActionResult<EventReservationRequest>> CreateRequest([FromBody] EventReservationRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.UserEmail))
            {
                return BadRequest("UserEmail is required.");
            }

            try
            {
                var createdRequest = await this.eventReservationRequest
                    .AddRequestAsync(request.RequestedDate.Date, request.EventType, request.UserEmail);

                return CreatedAtAction(nameof(CreateRequest), new { id = createdRequest.Id }, createdRequest);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<EventReservationRequest>>> GetPendingRequests()
        {
            try
            {
                var pendingRequests = await this.eventReservationRequestService.GetPendingRequestsAsync();
                return Ok(pendingRequests);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

    }
}
