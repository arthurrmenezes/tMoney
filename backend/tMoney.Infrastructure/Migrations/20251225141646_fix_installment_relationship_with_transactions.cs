using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tMoney.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fix_installment_relationship_with_transactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_installments_installment_id",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_installment_id",
                table: "transactions");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_installment_id",
                table: "transactions",
                column: "installment_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_installments_installment_id",
                table: "transactions",
                column: "installment_id",
                principalTable: "installments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_installments_installment_id",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_installment_id",
                table: "transactions");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_installment_id",
                table: "transactions",
                column: "installment_id");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_installments_installment_id",
                table: "transactions",
                column: "installment_id",
                principalTable: "installments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
