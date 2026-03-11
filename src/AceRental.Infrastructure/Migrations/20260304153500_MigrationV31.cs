using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AceRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PackItems",
                table: "PackItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PackItems",
                table: "PackItems",
                columns: new[] { "EquipmentId", "PackId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PackItems",
                table: "PackItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PackItems",
                table: "PackItems",
                column: "EquipmentId");
        }
    }
}
