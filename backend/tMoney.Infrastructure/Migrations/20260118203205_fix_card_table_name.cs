using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tMoney.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fix_card_table_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_credit_card_invoices_card_credit_card_id",
                table: "credit_card_invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_card_card_id",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_card",
                table: "card");

            migrationBuilder.RenameTable(
                name: "card",
                newName: "cards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cards",
                table: "cards",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_credit_card_invoices_cards_credit_card_id",
                table: "credit_card_invoices",
                column: "credit_card_id",
                principalTable: "cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_cards_card_id",
                table: "transactions",
                column: "card_id",
                principalTable: "cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_credit_card_invoices_cards_credit_card_id",
                table: "credit_card_invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_cards_card_id",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cards",
                table: "cards");

            migrationBuilder.RenameTable(
                name: "cards",
                newName: "card");

            migrationBuilder.AddPrimaryKey(
                name: "PK_card",
                table: "card",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_credit_card_invoices_card_credit_card_id",
                table: "credit_card_invoices",
                column: "credit_card_id",
                principalTable: "card",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_card_card_id",
                table: "transactions",
                column: "card_id",
                principalTable: "card",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
