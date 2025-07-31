using System.ComponentModel.DataAnnotations;

using static EventAgency.Data.Common.EntityConstants.Order;

namespace EventAgency.Web.ViewModels.Order
{
    public class OrderCheckoutViewModel
    {
        [Required]
        [Display(Name = "Първо име")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Display(Name = "Фамилно име")]
        public string LastName { get; set; } = null!;

        [Required]
        [MinLength(AddressMinLength)]
        [MaxLength(AddressMaxLength)]
        [Display(Name = "Адрес за доставка")]
        public string Address { get; set; } = null!;

        [Required]
        [Display(Name = "Телефонен номер")]
        public string Phone { get; set; } = null!;

        [Required]
        [Display(Name = "Метод на плащане")]
        public string PaymentMethod { get; set; } = null!;

        [Required]
        [Display(Name = "Обща стойност")]
        public decimal TotalPriceBGN { get; set; }

        public decimal TotalPriceEUR { get; set; }

        // Списък с продуктите, които потребителят е добавил в количката
        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();
    }
}
