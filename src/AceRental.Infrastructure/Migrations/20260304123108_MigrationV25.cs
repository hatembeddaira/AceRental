using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AceRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ReservationNumber",
                table: "Reservation",
                column: "ReservationNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservation_ReservationNumber",
                table: "Reservation");
        }
    }
}
