using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Data.Models
{
    public class OrderItem
    {
        [Comment("Order item identifier")]
        public Guid Id { get; set; }

        [Comment("Product name in the order")]
        public string ProductName { get; set; } = null!;

        [Comment("URL of the product image")]
        public string ImageUrl { get; set; } = null!;

        [Comment("Foreign key for the associated order")]
        public Guid OrderId { get; set; }


        [Comment("Navigation property for the associated order")]
        public virtual Order Order { get; set; } = null!; 

        [Comment("The product in the order")]
        public Guid ProductId { get; set; }


        [Comment("Navigation property for the associated product")]
        public virtual Product Product { get; set; } = null!;  

        [Comment("Quantity of the product ordered")]
        public int Quantity { get; set; } 

        [Comment("Unit price of the product")]
        public decimal Price { get; set; }  

        [Comment("Total price for this item (Price * Quantity)")]
        public decimal TotalPrice { get; set; }  


    }

}
