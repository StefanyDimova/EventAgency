using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventAgency.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderAndOrderItemModelsAndTheirConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Order identifier"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "The user of order"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()", comment: "Date and time when the order was created"),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Total price of the order"),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "Delivery address associated with the order"),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false, comment: "Phone number for contact regarding the order"),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Payment method chosen for the order (e.g., 'Cash on Delivery')"),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indicates whether the order is confirmed by the admin"),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indicates whether the order is cancelled by the admin")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Order in the system");

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Order item identifier"),
                    ProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Product name in the order"),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false, comment: "URL of the product image"),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Foreign key for the associated order"),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The product in the order"),
                    Quantity = table.Column<int>(type: "int", nullable: false, comment: "Quantity of the product ordered"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Unit price of the product"),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Total price for this item (Price * Quantity)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
