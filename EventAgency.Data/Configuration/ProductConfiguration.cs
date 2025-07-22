using EventAgency.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using static EventAgency.Data.Common.EntityConstants.Product;

namespace EventAgency.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            // Define the primary key of the Product entity
            entity
                .HasKey(p => p.Id);

            entity
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            entity
                .Property(p => p.Description)
                .HasMaxLength(DescriptionMaxLength);

            entity
                .Property(p => p.Quantity)
                .IsRequired();

            entity
                .Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity
                .Property(p => p.ImageUrl)
                .HasMaxLength(ImageUrlMaxLength);

            entity
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(p => p.SubCategory)
                .WithMany() 
                .HasForeignKey(p => p.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasQueryFilter(p => p.IsDeleted == false);

            entity.HasData(SeedProducts());

        }

        public List<Product> SeedProducts()
        {
            List<Product> products = new List<Product>()
            {
               new Product { Id = Guid.Parse("26c24284-94b5-4923-8dfb-d07519cf4d35") ,
                              Name = "Балони металик GEMAR – 12 см Сребро",
                              Description = "Балоните са подходящи за украса за Рожден ден ,Юбилей ,Кръщене , Сватба ,Абитуриентски бал и др .Балоните са произведени от натурален латекс и са 100 % биологически разградими!",
                              ImageUrl = "https://slonbalonparty.com/wp-content/uploads/2024/02/pearl-2.jpg",
                              Price = 10,
                              Quantity = 20,
                              CategoryId = 9
                              },
               new Product { Id = Guid.Parse("50670638-d177-40c4-a699-ba8193fd6c4a") ,
                              Name = "Балони металик GEMAR – 12 см Синьо",
                              Description = "Балоните са подходящи за украса за Рожден ден ,Юбилей ,Кръщене , Сватба ,Абитуриентски бал и др .Балоните са произведени от натурален латекс и са 100 % биологически разградими!",
                              ImageUrl = "https://slonbalonparty.com/wp-content/uploads/2024/02/blue-5.jpg",
                              Price = 10,
                              Quantity = 20,
                              CategoryId = 9
                              },
               new Product { Id = Guid.Parse("83f1b3af-f580-4913-86a9-94ac941f589d") ,
                              Name = "Балон -Емоджи /фолио/",
                              Description = "Фолиев балон - Емоджи, подходящ за всякакъв вид партита. ",
                              ImageUrl = "https://slonbalonparty.com/wp-content/uploads/2023/05/53921.jpg",
                              Price = 3,
                              Quantity = 15,
                              CategoryId = 8
                              },
               new Product { Id = Guid.Parse("92bbd767-698f-440f-a98e-92346204bf49") ,
                              Name = "Надпис - Мечета",
                              Description = "Надписът ще бъде изработен с име или друг текст по Ваше желание.\r\nТемата и докорациите също могат да бъдат променяни според Вашето жаление.",
                              ImageUrl = "https://www.party-market.bg/uploads/thumbs/500x500/a1e6dfda92ce84a8f0525bf34f9089f1.jpg",
                              Price = 2,
                              Quantity = 10,
                              CategoryId = 14
                              },

            };

            return products;
        }
    }
}
