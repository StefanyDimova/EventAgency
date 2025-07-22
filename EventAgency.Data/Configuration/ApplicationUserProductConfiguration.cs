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
    public class ApplicationUserProductConfiguration : IEntityTypeConfiguration<ApplicationUserProduct>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserProduct> entity)
        {
            // Define composite Primary Key of the Mapping Entity
            entity
                .HasKey(aup => new { aup.ApplicationUserId, aup.ProductId });

            entity
                .Property(aup => aup.ApplicationUserId)
                .IsRequired();

            entity
               .Property(aup => aup.IsDeleted)
               .HasDefaultValue(false);

            entity
                .HasOne(aup => aup.ApplicationUser)
                .WithMany() // We do not have navigation property from the IdentityUser side
                .HasForeignKey(aup => aup.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relation between ApplicationUserProduct and Product
            entity
                .HasOne(aup => aup.Product)
                .WithMany(p => p.UserCart)
                .HasForeignKey(aup => aup.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasQueryFilter(aup => aup.Product.IsDeleted == false &&
                                       aup.IsDeleted == false);

        }
    }
}
