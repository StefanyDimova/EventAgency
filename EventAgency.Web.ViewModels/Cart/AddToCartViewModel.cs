using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Web.ViewModels.Cart
{
    public class AddToCartViewModel
    {
        [Required(ErrorMessage = "Моля, въведете количество.")]
        [Range(1, int.MaxValue, ErrorMessage = "Количество трябва да е поне 1.")]
        public int Quantity { get; set; }

        public string ProductId { get; set; } = null!;
    }
}
