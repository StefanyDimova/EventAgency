using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventAgency.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingEventModelAndSeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Event identifier"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Event name"),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "Event description"),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true, comment: "Event image"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Shows if event is deleted")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                },
                comment: "Event in the system");

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("85dbe8eb-bedf-4ac4-8ecb-1a0e4e747349"), "Честването на рождения ден е специално събитие, което задължително трябва да се сподели с най-близките хора. Без значение възрастта всеки се радва да празнува и наистина е хубаво, когато хората идват, за да ви поздравят и да споделят този ден с вас. Ако пък ви предстои важен етап от живота, като навършване на 30, 40, 50 или дори на 60, тогава задължително трябва да се отпразнува с цялата тайфа. Обикновенно рожденният ден включва вечеря, игри, музика и много други, а гостите са от семейството и приятели.", "https://emotionsfactory.bg/cdn/shop/articles/feautured_a43badcf-8f09-4258-a042-2cab709e0259_1080x.jpg?v=1563550055", "Рожден Ден" },
                    { new Guid("b6f5b136-1600-49a6-91c5-c6ef7933099c"), "Сватбата е церемония, в която двама души сключват брак или подобен институционализиран съюз. Сватбените традиции и обичаи варират в различните култури, етнически групи, религии, държави и социални прослойки. Повечето сватбени церемонии включват размяната на брачни клетви, златни халки, получаването на подаръци (материални, нематериални и символични) и публичното признаване на брака от лице, имащо законовото право да го обяви за официален. Често се носи специално сватбено облекло, а церемонията е последвана от прием/празненство. Допълнително по време на самата сватбена церемония може да има музика, поезия, молитви, четения от свети писания и други традиционни прояви. При извеждането на булката се пее песен. Родителите на младоженеца са наричани от булката — свекър и свекърва. Братът на младоженеца е наричан от булката девер.", "https://spisaniebulka.com/wp-content/uploads/2024/01/676-230818-172129-PRINT.jpg", "Сватба" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
