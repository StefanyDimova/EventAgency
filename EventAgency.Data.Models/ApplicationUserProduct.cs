using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventAgency.Data.Models
{
    [Comment("User Cart entry in the system.")]
    public class ApplicationUserProduct
    {
        [Comment("Foreign key to the referenced AspNetUser. Part of the entity composite PK.")]
        public string ApplicationUserId { get; set; } = null!;
        public virtual IdentityUser ApplicationUser { get; set; } = null!;

        [Comment("Foreign key to the referenced Product. Part of the entity composite PK.")]
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public virtual Product Product { get; set; } = null!;

        [Comment("Shows if ApplicationUserProduct entry is deleted")]
        public bool IsDeleted { get; set; }

        public decimal TotalPrice => Product.Price * Quantity;
    }
}
