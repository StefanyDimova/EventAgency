using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Web.ViewModels.Admin.EventReservationManagement
{
    public class ReservationRequestViewModel
    {
        public string Id { get; set; }

        public DateTime RequestedDate { get; set; }

        public string EventType { get; set; }

        public bool IsApproved { get; set; }
    }
}
