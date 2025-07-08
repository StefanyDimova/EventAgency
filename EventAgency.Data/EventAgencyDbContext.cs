using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventAgencyFinalProject.Data
{
    public class EventAgencyDbContext : IdentityDbContext
    {
        public EventAgencyDbContext(DbContextOptions<EventAgencyDbContext> options)
            : base(options)
        {
        }
    }
}
