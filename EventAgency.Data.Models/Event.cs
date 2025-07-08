using Microsoft.EntityFrameworkCore;

namespace EventAgency.Data.Models
{
    [Comment("Event in the system")]
    public class Event
    {
        [Comment("Event identifier")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Comment("Event name")]
        public string Name { get; set; } = null!;

        [Comment("Event description")]
        public string Description { get; set; } = null!;

        [Comment("Event image")]
        public string? ImageUrl { get; set; } = null!;

        [Comment("Shows if event is deleted")]
        public bool IsDeleted { get; set; }
    }
}
