using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventAgency.Data.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeletingOfApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserProducts_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserProducts_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserProducts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserProducts_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserProducts_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserProducts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
