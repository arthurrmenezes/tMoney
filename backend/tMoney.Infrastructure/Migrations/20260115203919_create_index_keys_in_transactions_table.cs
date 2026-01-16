using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tMoney.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class create_index_keys_in_transactions_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_transactions_account_id_created_at",
                table: "transactions",
                columns: new[] { "account_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_transactions_account_id_status_date",
                table: "transactions",
                columns: new[] { "account_id", "status", "date" });

            migrationBuilder.CreateIndex(
                name: "IX_transactions_status_date",
                table: "transactions",
                columns: new[] { "status", "date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_transactions_account_id_created_at",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_account_id_status_date",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_status_date",
                table: "transactions");
        }
    }
}
