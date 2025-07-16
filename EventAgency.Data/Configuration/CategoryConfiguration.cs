using EventAgency.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static EventAgency.Data.Common.EntityConstants.Category;

namespace EventAgency.Data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            entity
                .HasKey(c => c.Id);

            entity
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            entity
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .Property(p => p.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            entity.HasQueryFilter(p => p.IsDeleted == false);

            entity.HasData(SeedCategories());
        }

        public List<Category> SeedCategories()
        {
            List<Category> categories = new List<Category>()
            {
               new Category { Id = 1, Name = "Декорации" },
               new Category { Id = 2, Name = "Балони" },
               new Category { Id = 3, Name = "Надписи" },
               new Category { Id = 4, Name = "Покани" },
               new Category { Id = 5, Name = "Подаръци за гости" },
               new Category { Id = 6, Name = "Оборудване под наем" },
               new Category { Id = 7, Name = "Банери" },
               new Category { Id = 8, Name = "Фолиеви Балони", ParentCategoryId = 2 },
               new Category { Id = 9, Name = "Латексови Балони", ParentCategoryId = 2 },
               new Category { Id = 10, Name = "Комплекти", ParentCategoryId = 2 },
               new Category { Id = 11, Name = "Аксесоари за балони", ParentCategoryId = 2 },
            };

            return categories;
        }
    }
}
