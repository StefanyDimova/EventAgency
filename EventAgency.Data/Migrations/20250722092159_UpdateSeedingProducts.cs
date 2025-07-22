using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventAgency.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedingProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price", "Quantity" },
                values: new object[,]
                {
                    { new Guid("83f1b3af-f580-4913-86a9-94ac941f589d"), 8, "Фолиев балон - Емоджи, подходящ за всякакъв вид партита. ", "https://slonbalonparty.com/wp-content/uploads/2023/05/53921.jpg", "Балон -Емоджи /фолио/", 3m, 15 },
                    { new Guid("92bbd767-698f-440f-a98e-92346204bf49"), 14, "Надписът ще бъде изработен с име или друг текст по Ваше желание.\r\nТемата и докорациите също могат да бъдат променяни според Вашето жаление.", "https://www.party-market.bg/uploads/thumbs/500x500/a1e6dfda92ce84a8f0525bf34f9089f1.jpg", "Надпис - Мечета", 2m, 10 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("83f1b3af-f580-4913-86a9-94ac941f589d"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("92bbd767-698f-440f-a98e-92346204bf49"));
        }
    }
}
