using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventAgency.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedFirstProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price", "Quantity" },
                values: new object[,]
                {
                    { new Guid("26c24284-94b5-4923-8dfb-d07519cf4d35"), 9, "Балоните са подходящи за украса за Рожден ден ,Юбилей ,Кръщене , Сватба ,Абитуриентски бал и др .Балоните са произведени от натурален латекс и са 100 % биологически разградими!", "https://slonbalonparty.com/wp-content/uploads/2024/02/pearl-2.jpg", "Балони металик GEMAR – 12 см Сребро", 10m, 20 },
                    { new Guid("50670638-d177-40c4-a699-ba8193fd6c4a"), 9, "Балоните са подходящи за украса за Рожден ден ,Юбилей ,Кръщене , Сватба ,Абитуриентски бал и др .Балоните са произведени от натурален латекс и са 100 % биологически разградими!", "https://slonbalonparty.com/wp-content/uploads/2024/02/blue-5.jpg", "Балони металик GEMAR – 12 см Синьо", 10m, 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("26c24284-94b5-4923-8dfb-d07519cf4d35"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("50670638-d177-40c4-a699-ba8193fd6c4a"));
        }
    }
}
