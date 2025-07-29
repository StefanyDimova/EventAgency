using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Data.Models
{
    public class EventReservationRequest
    {
        public Guid Id { get; set; }
        public DateTime RequestedDate { get; set; }
        public string EventType { get; set; } = null!;
        public bool IsApproved { get; set; } 
    }
}
