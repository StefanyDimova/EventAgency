using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Data.Common
{
    public class EntityConstants
    {
        public static class Event
        {

            /// <summary>
            /// Event Name should be at least 3 characters and up to 100 characters.
            /// </summary>
            public const int NameMinLength = 3;

            /// <summary>
            /// Event Name should be able to store text with length up to 100 characters.
            /// </summary>
            public const int NameMaxLength = 100;

            /// <summary>
            /// Event Description must be at least 10 characters.
            /// </summary>
            public const int DescriptionMinLength = 10;

            /// <summary>
            /// Event Description should be able to store text with length up to 1000 characters.
            /// </summary>
            public const int DescriptionMaxLength = 1000;

            /// <summary>
            /// Maximum allowed length for image URL.
            /// </summary>
            public const int ImageUrlMaxLength = 2048;
        }

        public static class Product
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 1000;

            public const int ImageUrlMaxLength = 2048;
        }

        public static class Category
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;


        }
    }
}
