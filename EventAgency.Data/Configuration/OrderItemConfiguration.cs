using EventAgency.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static EventAgency.Data.Common.EntityConstants.Product;

namespace EventAgency.Data.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> entity)
        {
            entity
                .HasKey(oi => oi.Id);

            entity.Property(oi => oi.Quantity)
                .IsRequired();

            entity.Property(oi => oi.Price)
                .HasColumnType("decimal(18, 2)");

            entity.Property(oi => oi.TotalPrice)
                .HasColumnType("decimal(18, 2)");

            entity.Property(oi => oi.ProductName)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            entity.Property(oi => oi.ImageUrl)
                .HasMaxLength(ImageUrlMaxLength);

            entity.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
        }
    }
}
