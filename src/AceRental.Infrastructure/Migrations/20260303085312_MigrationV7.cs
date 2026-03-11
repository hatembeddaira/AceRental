using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AceRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ReservationItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ReservationItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ReservationItem",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ReservationItem",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ReservationItem",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ReservationItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Reservation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Reservation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Reservation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Reservation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Reservation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Quote",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Quote",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Quote",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Quote",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Quote",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Packs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Packs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Packs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Packs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Packs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Packs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PackItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PackItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "PackItem",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "PackItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PackItem",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PackItem",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "PackItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Invoice",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Invoice",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Invoice",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Equipments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Equipments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Equipments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Equipments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Equipments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Equipments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Clients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Clients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Clients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ReservationItem");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ReservationItem");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ReservationItem");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ReservationItem");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ReservationItem");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ReservationItem");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Quote");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Quote");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Quote");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Quote");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Quote");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Packs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Packs");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Packs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Packs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Packs");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Packs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PackItem");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PackItem");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "PackItem");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PackItem");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PackItem");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PackItem");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PackItem");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Clients");
        }
    }
}
