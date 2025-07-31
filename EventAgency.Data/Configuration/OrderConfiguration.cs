using EventAgency.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static EventAgency.Data.Common.EntityConstants.Order;

namespace EventAgency.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entity)
        {
            entity.HasKey(o => o.Id);

            entity.Property(o => o.UserId)
            .IsRequired();

            entity.Property(o => o.Address)
            .IsRequired()
            .HasMaxLength(AddressMaxLength);

            entity.Property(o => o.Phone)
            .IsRequired()
            .HasMaxLength(PhoneMaxLength);

            entity.Property(o => o.PaymentMethod)
            .IsRequired();

            entity.Property(o => o.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

            entity.Property(o => o.TotalPrice)
               .HasColumnType("decimal(18, 2)");

            entity.Property(o => o.IsConfirmed)
                .HasDefaultValue(false);

            entity.Property(o => o.IsCancelled)
                .HasDefaultValue(false);

            entity.HasMany(o => o.OrderItems)
               .WithOne(oi => oi.Order)
               .HasForeignKey(oi => oi.OrderId)
               .OnDelete(DeleteBehavior.Cascade);


            entity.HasOne(o => o.User)
               .WithMany() 
               .HasForeignKey(o => o.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
