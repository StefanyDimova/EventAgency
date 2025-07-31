namespace EventAgency.Web.ViewModels
{
    public class ValidationMessages
    {
        public static class Event
        {
            public const string NameRequiredMessage = "Name is required.";
            public const string NameMinLengthMessage = "Name must be at least 3 characters.";
            public const string NameMaxLengthMessage = "Name cannot exceed 100 characters.";

            public const string DescriptionRequiredMessage = "Description is required.";
            public const string DescriptionMinLengthMessage = "Description must be at least 10 characters.";
            public const string DescriptionMaxLengthMessage = "Description cannot exceed 1000 characters.";

            public const string ImageUrlMaxLengthMessage = "Image URL cannot exceed 2048 characters.";

            public const string ServiceCreateError =
                "Fatal error occurred while adding your event! Please try again later!";
        }

        public static class Product
        {
            public const string ProductNameRequiredMessage = "Name of product is required.";
            public const string ProductNameMinLengthMessage = "Name of product must be at least 2 characters.";
            public const string ProductNameMaxLengthMessage = "Name of product cannot exceed 50 characters.";

            public const string ProductDescriptionRequiredMessage = "Description of product is required.";
            public const string ProductDescriptionMinLengthMessage = "Description of product must be at least 10 characters.";
            public const string ProductDescriptionMaxLengthMessage = "Description of product cannot exceed 1000 characters.";

            public const string ImageUrlMaxLengthMessage = "Image URL cannot exceed 2048 characters.";
        }

        public static class Category
        {
            public const string CategoryNameRequiredMessage = "Name of category is required.";
            public const string CategoryNameMinLengthMessage = "Name of category must be at least 3 characters.";
            public const string CategoryNameMaxLengthMessage = "Name of category cannot exceed 30 characters.";
        }
    }
}
