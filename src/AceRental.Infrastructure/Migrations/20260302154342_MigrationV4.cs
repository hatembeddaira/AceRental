using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AceRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationItem_Equipments_EquipmentId",
                table: "ReservationItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationItem_Packs_PackId",
                table: "ReservationItem");

            migrationBuilder.AddColumn<Guid>(
                name: "ReservationId1",
                table: "ReservationItem",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId1",
                table: "Reservation",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReservationItem_ReservationId1",
                table: "ReservationItem",
                column: "ReservationId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ClientId1",
                table: "Reservation",
                column: "ClientId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Clients_ClientId1",
                table: "Reservation",
                column: "ClientId1",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationItem_Equipments_EquipmentId",
                table: "ReservationItem",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationItem_Packs_PackId",
                table: "ReservationItem",
                column: "PackId",
                principalTable: "Packs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationItem_Reservation_ReservationId1",
                table: "ReservationItem",
                column: "ReservationId1",
                principalTable: "Reservation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Clients_ClientId1",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationItem_Equipments_EquipmentId",
                table: "ReservationItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationItem_Packs_PackId",
                table: "ReservationItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationItem_Reservation_ReservationId1",
                table: "ReservationItem");

            migrationBuilder.DropIndex(
                name: "IX_ReservationItem_ReservationId1",
                table: "ReservationItem");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_ClientId1",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "ReservationId1",
                table: "ReservationItem");

            migrationBuilder.DropColumn(
                name: "ClientId1",
                table: "Reservation");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationItem_Equipments_EquipmentId",
                table: "ReservationItem",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationItem_Packs_PackId",
                table: "ReservationItem",
                column: "PackId",
                principalTable: "Packs",
                principalColumn: "Id");
        }
    }
}
