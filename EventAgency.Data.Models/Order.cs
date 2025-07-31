using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventAgency.Data.Models
{
    [Comment("Order in the system")]
    public class Order
    {
        [Comment("Order identifier")]
        public Guid Id { get; set; }

        [Comment("The user of order")]
        public string UserId { get; set; } = null!;
        public virtual IdentityUser User { get; set; } = null!;
        [Comment("Date and time when the order was created")]
        public DateTime CreatedAt { get; set; }

        [Comment("Total price of the order")]
        public decimal TotalPrice { get; set; }

        [Comment("Delivery address associated with the order")]
        public string Address { get; set; } = null!;

        [Comment("Phone number for contact regarding the order")]
        public string Phone { get; set; } = null!;

        [Comment("Payment method chosen for the order (e.g., 'Cash on Delivery')")]
        public string PaymentMethod { get; set; } = null!;

        [Comment("Indicates whether the order is confirmed by the admin")]
        public bool IsConfirmed { get; set; }

        [Comment("Indicates whether the order is cancelled by the admin")]
        public bool IsCancelled { get; set; }

        [Comment("Collection of products in the order")]
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }
}
