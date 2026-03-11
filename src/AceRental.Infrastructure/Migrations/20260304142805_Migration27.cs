using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AceRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackItems_Equipments_EquipmentId",
                table: "PackItems");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentId",
                table: "Packs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentId1",
                table: "PackItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Packs_EquipmentId",
                table: "Packs",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PackItems_EquipmentId1",
                table: "PackItems",
                column: "EquipmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PackItems_Equipments_EquipmentId1",
                table: "PackItems",
                column: "EquipmentId1",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Packs_Equipments_EquipmentId",
                table: "Packs",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackItems_Equipments_EquipmentId1",
                table: "PackItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Packs_Equipments_EquipmentId",
                table: "Packs");

            migrationBuilder.DropIndex(
                name: "IX_Packs_EquipmentId",
                table: "Packs");

            migrationBuilder.DropIndex(
                name: "IX_PackItems_EquipmentId1",
                table: "PackItems");

            migrationBuilder.DropColumn(
                name: "EquipmentId",
                table: "Packs");

            migrationBuilder.DropColumn(
                name: "EquipmentId1",
                table: "PackItems");

            migrationBuilder.AddForeignKey(
                name: "FK_PackItems_Equipments_EquipmentId",
                table: "PackItems",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
