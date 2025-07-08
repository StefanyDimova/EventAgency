using EventAgency.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static EventAgency.Data.Common.EntityConstants.Event;

namespace EventAgency.Data.Configuration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> entity)
        {
            // Define the primary key of the Event entity
            entity
                .HasKey(e => e.Id);


            // Define constraints for the Name column
            entity
               .Property(e => e.Name)
               .IsRequired()
               .HasMaxLength(NameMaxLength);

            // Define constraints for the Description column
            entity
                .Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(DescriptionMaxLength);

            // Define constraints for the ImageUrl column
            entity
                .Property(e => e.ImageUrl)
                .IsRequired(false)
                .HasMaxLength(ImageUrlMaxLength);

            // Define constraints for the IsDeleted column
            entity
                .Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            //Filter out only the active (non-deleted) entries
            entity
                .HasQueryFilter(e => e.IsDeleted == false);

            //Seed events data with migration
            entity.HasData(this.SeedEvents());
        }

        public List<Event> SeedEvents()
        {
            List<Event> events = new List<Event>()
            {
                new Event()
                {
                    Id = Guid.Parse("b6f5b136-1600-49a6-91c5-c6ef7933099c"),
                    Name = "Сватба",
                    Description = "Сватбата е церемония, в която двама души сключват брак или подобен институционализиран съюз. Сватбените традиции и обичаи варират в различните култури, етнически групи, религии, държави и социални прослойки. Повечето сватбени церемонии включват размяната на брачни клетви, златни халки, получаването на подаръци (материални, нематериални и символични) и публичното признаване на брака от лице, имащо законовото право да го обяви за официален. Често се носи специално сватбено облекло, а церемонията е последвана от прием/празненство. Допълнително по време на самата сватбена церемония може да има музика, поезия, молитви, четения от свети писания и други традиционни прояви. При извеждането на булката се пее песен. Родителите на младоженеца са наричани от булката — свекър и свекърва. Братът на младоженеца е наричан от булката девер.",
                    ImageUrl = "https://spisaniebulka.com/wp-content/uploads/2024/01/676-230818-172129-PRINT.jpg"
                },
                new Event()
                {
                    Id = Guid.Parse("85dbe8eb-bedf-4ac4-8ecb-1a0e4e747349"),
                    Name = "Рожден Ден",
                    Description = "Честването на рождения ден е специално събитие, което задължително трябва да се сподели с най-близките хора. Без значение възрастта всеки се радва да празнува и наистина е хубаво, когато хората идват, за да ви поздравят и да споделят този ден с вас. Ако пък ви предстои важен етап от живота, като навършване на 30, 40, 50 или дори на 60, тогава задължително трябва да се отпразнува с цялата тайфа. Обикновенно рожденният ден включва вечеря, игри, музика и много други, а гостите са от семейството и приятели.",
                    ImageUrl = "https://emotionsfactory.bg/cdn/shop/articles/feautured_a43badcf-8f09-4258-a042-2cab709e0259_1080x.jpg?v=1563550055"
                }
            };

            return events;
        }

    }
}
