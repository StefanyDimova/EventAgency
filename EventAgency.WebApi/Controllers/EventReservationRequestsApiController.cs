using EventAgency.Data.Models;
using EventAgency.Services.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventAgency.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventReservationRequestsApiController : ControllerBase
    {
        private readonly IEventReservationRequestService eventReservationRequestService;

        public EventReservationRequestsApiController(IEventReservationRequestService eventReservationRequestService)
        {
                this.eventReservationRequestService = eventReservationRequestService;
        }

        [HttpPost]
        public async Task<ActionResult<EventReservationRequest>> CreateRequest(EventReservationRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }

            try
            {
                var createdRequest = await this.eventReservationRequestService.AddRequestAsync(request.RequestedDate.Date, request.EventType);

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

        [HttpGet("date/{date}")]
        public async Task<ActionResult<EventReservationRequest>> GetRequestByDate(DateTime date)
        {
            try
            {
                var currDate = date.Date;
                var request = await this.eventReservationRequestService.GetRequestByDateAsync(currDate);

                if (request == null)
                {
                    return NotFound("No reservation request found for the specified date.");
                }

                return Ok(request);
            }
            catch (Exception ex)
            {
                // Логирайте грешката или върнете съобщение за грешка
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
