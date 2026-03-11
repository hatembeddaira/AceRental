using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AceRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PackItem",
                table: "PackItem");

            migrationBuilder.DropIndex(
                name: "IX_PackItem_EquipmentId",
                table: "PackItem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PackItem",
                table: "PackItem",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PackItem_PackId",
                table: "PackItem",
                column: "PackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PackItem",
                table: "PackItem");

            migrationBuilder.DropIndex(
                name: "IX_PackItem_PackId",
                table: "PackItem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PackItem",
                table: "PackItem",
                columns: new[] { "PackId", "EquipmentId" });

            migrationBuilder.CreateIndex(
                name: "IX_PackItem_EquipmentId",
                table: "PackItem",
                column: "EquipmentId");
        }
    }
}
