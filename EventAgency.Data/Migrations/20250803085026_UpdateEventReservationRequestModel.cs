using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventAgency.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEventReservationRequestModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "EventReservationRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "EventReservationRequests");
        }
    }
}
