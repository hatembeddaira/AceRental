using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AceRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV36 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Reservations",
                newName: "Workflow");

            migrationBuilder.AddColumn<int>(
                name: "FinancialStatus",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LogisticStatus",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinancialStatus",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "LogisticStatus",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "Workflow",
                table: "Reservations",
                newName: "Status");
        }
    }
}
