using System.ComponentModel.DataAnnotations;
using static EventAgency.GCommon.ApplicationConstants;
using static EventAgency.Web.ViewModels.ValidationMessages.Event;
using static EventAgency.Data.Common.EntityConstants.Event;

namespace EventAgency.Web.ViewModels.Event
{
    public class EventFormInputModel
    {

        public string Id { get; set; }
            = string.Empty;


        [Required(ErrorMessage = NameRequiredMessage)]
        [MinLength(NameMinLength, ErrorMessage = NameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = NameMaxLengthMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = DescriptionRequiredMessage)]
        [MinLength(DescriptionMinLength, ErrorMessage = DescriptionMinLengthMessage)]
        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthMessage)]
        public string Description { get; set; } = null!;

        [MaxLength(ImageUrlMaxLength, ErrorMessage = ImageUrlMaxLengthMessage)]
        public string? ImageUrl { get; set; }
            = $"/images/{NoImageUrl}";
    }
}
