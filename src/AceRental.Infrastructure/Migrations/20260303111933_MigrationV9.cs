using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AceRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationItem_Reservation_ReservationId1",
                table: "ReservationItem");

            migrationBuilder.DropIndex(
                name: "IX_ReservationItem_ReservationId1",
                table: "ReservationItem");

            migrationBuilder.DropColumn(
                name: "ReservationId1",
                table: "ReservationItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReservationId1",
                table: "ReservationItem",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReservationItem_ReservationId1",
                table: "ReservationItem",
                column: "ReservationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationItem_Reservation_ReservationId1",
                table: "ReservationItem",
                column: "ReservationId1",
                principalTable: "Reservation",
                principalColumn: "Id");
        }
    }
}
