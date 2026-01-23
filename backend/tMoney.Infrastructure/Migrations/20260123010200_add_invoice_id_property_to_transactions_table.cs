using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tMoney.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_invoice_id_property_to_transactions_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "invoice_id",
                table: "transactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_transactions_invoice_id",
                table: "transactions",
                column: "invoice_id");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_credit_card_invoices_invoice_id",
                table: "transactions",
                column: "invoice_id",
                principalTable: "credit_card_invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_credit_card_invoices_invoice_id",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_invoice_id",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "invoice_id",
                table: "transactions");
        }
    }
}
