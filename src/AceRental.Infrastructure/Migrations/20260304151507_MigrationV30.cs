using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AceRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackItems_Equipments_EquipmentId1",
                table: "PackItems");

            migrationBuilder.DropIndex(
                name: "IX_PackItems_EquipmentId1",
                table: "PackItems");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackItems_Equipments_EquipmentId",
                table: "PackItems");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentId1",
                table: "PackItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
        }
    }
}
