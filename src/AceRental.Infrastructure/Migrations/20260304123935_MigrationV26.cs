using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AceRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Reservation_ReservationId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_PackItem_Equipments_EquipmentId",
                table: "PackItem");

            migrationBuilder.DropForeignKey(
                name: "FK_PackItem_Packs_PackId",
                table: "PackItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Quote_Reservation_ReservationId",
                table: "Quote");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Clients_ClientId",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationItem_Equipments_EquipmentId",
                table: "ReservationItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationItem_Packs_PackId",
                table: "ReservationItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationItem_Reservation_ReservationId",
                table: "ReservationItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservationItem",
                table: "ReservationItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quote",
                table: "Quote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PackItem",
                table: "PackItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoice",
                table: "Invoice");

            migrationBuilder.RenameTable(
                name: "ReservationItem",
                newName: "ReservationItems");

            migrationBuilder.RenameTable(
                name: "Reservation",
                newName: "Reservations");

            migrationBuilder.RenameTable(
                name: "Quote",
                newName: "Quotes");

            migrationBuilder.RenameTable(
                name: "PackItem",
                newName: "PackItems");

            migrationBuilder.RenameTable(
                name: "Invoice",
                newName: "Invoices");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationItem_ReservationId",
                table: "ReservationItems",
                newName: "IX_ReservationItems_ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationItem_PackId",
                table: "ReservationItems",
                newName: "IX_ReservationItems_PackId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationItem_EquipmentId",
                table: "ReservationItems",
                newName: "IX_ReservationItems_EquipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_ReservationNumber",
                table: "Reservations",
                newName: "IX_Reservations_ReservationNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_ClientId",
                table: "Reservations",
                newName: "IX_Reservations_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Quote_ReservationId",
                table: "Quotes",
                newName: "IX_Quotes_ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_Quote_QuoteNumber",
                table: "Quotes",
                newName: "IX_Quotes_QuoteNumber");

            migrationBuilder.RenameIndex(
                name: "IX_PackItem_PackId",
                table: "PackItems",
                newName: "IX_PackItems_PackId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_ReservationId",
                table: "Invoices",
                newName: "IX_Invoices_ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_InvoiceNumber",
                table: "Invoices",
                newName: "IX_Invoices_InvoiceNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservationItems",
                table: "ReservationItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quotes",
                table: "Quotes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PackItems",
                table: "PackItems",
                column: "EquipmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Reservations_ReservationId",
                table: "Invoices",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PackItems_Equipments_EquipmentId",
                table: "PackItems",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PackItems_Packs_PackId",
                table: "PackItems",
                column: "PackId",
                principalTable: "Packs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_Reservations_ReservationId",
                table: "Quotes",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationItems_Equipments_EquipmentId",
                table: "ReservationItems",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationItems_Packs_PackId",
                table: "ReservationItems",
                column: "PackId",
                principalTable: "Packs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationItems_Reservations_ReservationId",
                table: "ReservationItems",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Clients_ClientId",
                table: "Reservations",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Reservations_ReservationId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_PackItems_Equipments_EquipmentId",
                table: "PackItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PackItems_Packs_PackId",
                table: "PackItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Reservations_ReservationId",
                table: "Quotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationItems_Equipments_EquipmentId",
                table: "ReservationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationItems_Packs_PackId",
                table: "ReservationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationItems_Reservations_ReservationId",
                table: "ReservationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Clients_ClientId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservationItems",
                table: "ReservationItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quotes",
                table: "Quotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PackItems",
                table: "PackItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices");

            migrationBuilder.RenameTable(
                name: "Reservations",
                newName: "Reservation");

            migrationBuilder.RenameTable(
                name: "ReservationItems",
                newName: "ReservationItem");

            migrationBuilder.RenameTable(
                name: "Quotes",
                newName: "Quote");

            migrationBuilder.RenameTable(
                name: "PackItems",
                newName: "PackItem");

            migrationBuilder.RenameTable(
                name: "Invoices",
                newName: "Invoice");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_ReservationNumber",
                table: "Reservation",
                newName: "IX_Reservation_ReservationNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_ClientId",
                table: "Reservation",
                newName: "IX_Reservation_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationItems_ReservationId",
                table: "ReservationItem",
                newName: "IX_ReservationItem_ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationItems_PackId",
                table: "ReservationItem",
                newName: "IX_ReservationItem_PackId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationItems_EquipmentId",
                table: "ReservationItem",
                newName: "IX_ReservationItem_EquipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Quotes_ReservationId",
                table: "Quote",
                newName: "IX_Quote_ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_Quotes_QuoteNumber",
                table: "Quote",
                newName: "IX_Quote_QuoteNumber");

            migrationBuilder.RenameIndex(
                name: "IX_PackItems_PackId",
                table: "PackItem",
                newName: "IX_PackItem_PackId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_ReservationId",
                table: "Invoice",
                newName: "IX_Invoice_ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_InvoiceNumber",
                table: "Invoice",
                newName: "IX_Invoice_InvoiceNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservationItem",
                table: "ReservationItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quote",
                table: "Quote",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PackItem",
                table: "PackItem",
                column: "EquipmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoice",
                table: "Invoice",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Reservation_ReservationId",
                table: "Invoice",
                column: "ReservationId",
                principalTable: "Reservation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PackItem_Equipments_EquipmentId",
                table: "PackItem",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PackItem_Packs_PackId",
                table: "PackItem",
                column: "PackId",
                principalTable: "Packs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quote_Reservation_ReservationId",
                table: "Quote",
                column: "ReservationId",
                principalTable: "Reservation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Clients_ClientId",
                table: "Reservation",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationItem_Reservation_ReservationId",
                table: "ReservationItem",
                column: "ReservationId",
                principalTable: "Reservation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
