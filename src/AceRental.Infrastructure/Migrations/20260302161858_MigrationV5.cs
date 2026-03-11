using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AceRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "QuoteNumber",
                table: "Quote",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalTTC",
                table: "Quote",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalTTC",
                table: "Quote");

            migrationBuilder.AlterColumn<string>(
                name: "QuoteNumber",
                table: "Quote",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
