using EventAgency.Data.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace EventAgency.Data.Models
{
    [Comment("Product in the system")]
    public class Product
    {
        [Comment("Product identifier")]
        public Guid Id { get; set; }

        [Comment("Product name")]
        public string Name { get; set; } = null!;

        [Comment("Description of the product")]
        public string Description { get; set; } = null!;

        [Comment("Product price")]
        public decimal Price { get; set; }

        [Comment("Product quantity")]
        public int Quantity { get; set; }

        [Comment("Product's category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        [Comment("Product image url from the image store")]
        public string? ImageUrl { get; set; }

        [Comment("Shows if Product is deleted")]
        public bool IsDeleted { get; set; }
    }
}
