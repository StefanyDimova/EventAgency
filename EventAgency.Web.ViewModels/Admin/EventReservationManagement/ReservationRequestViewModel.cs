using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Web.ViewModels.Admin.EventReservationManagement
{
    public class ReservationRequestViewModel
    {
        public string Id { get; set; } = null!;

        public DateTime RequestedDate { get; set; }

        public string EventType { get; set; } = null!;

        public bool IsApproved { get; set; }

        public string? UserEmail { get; set; }
    }
}
