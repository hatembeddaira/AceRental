using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AceRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV34 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ReservationHistorys");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ReservationHistorys");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ReservationHistorys");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ReservationHistorys");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ReservationHistorys",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ReservationHistorys",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ReservationHistorys",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ReservationHistorys",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
