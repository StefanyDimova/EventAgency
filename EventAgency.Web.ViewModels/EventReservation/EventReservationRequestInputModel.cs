using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Web.ViewModels.EventReservation
{
    public class EventReservationRequestInputModel
    {
        public DateTime RequestedDate { get; set; }

        public string EventType { get; set; } = null!;
    }

}
