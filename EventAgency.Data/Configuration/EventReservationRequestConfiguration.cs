using EventAgency.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Data.Configuration
{
    public class EventReservationRequestConfiguration : IEntityTypeConfiguration<EventReservationRequest>
    {
        public void Configure(EntityTypeBuilder<EventReservationRequest> entity)
        {
            entity
                .HasKey(err => err.Id);

            entity
                .Property(err => err.EventType)
                .IsRequired();

            entity
                .Property(err => err.IsApproved)
                .HasDefaultValue(false);

            entity
                .Property(err => err.RequestedDate)
                .IsRequired();
        }
    }
}
