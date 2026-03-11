using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AceRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration28 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packs_Equipments_EquipmentId",
                table: "Packs");

            migrationBuilder.DropIndex(
                name: "IX_Packs_EquipmentId",
                table: "Packs");

            migrationBuilder.DropColumn(
                name: "EquipmentId",
                table: "Packs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentId",
                table: "Packs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Packs_EquipmentId",
                table: "Packs",
                column: "EquipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Packs_Equipments_EquipmentId",
                table: "Packs",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id");
        }
    }
}
