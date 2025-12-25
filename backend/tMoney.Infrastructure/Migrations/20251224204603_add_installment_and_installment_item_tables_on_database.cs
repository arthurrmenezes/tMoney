using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tMoney.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_installment_and_installment_item_tables_on_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "installment_id",
                table: "transactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "installments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    total_installments = table.Column<int>(type: "integer", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    interest_rate = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    first_payment_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_installments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_installments_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "installment_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    installment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    paid_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_installment_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_installment_items_installments_installment_id",
                        column: x => x.installment_id,
                        principalTable: "installments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_transactions_installment_id",
                table: "transactions",
                column: "installment_id");

            migrationBuilder.CreateIndex(
                name: "IX_installment_items_installment_id",
                table: "installment_items",
                column: "installment_id");

            migrationBuilder.CreateIndex(
                name: "IX_installments_account_id",
                table: "installments",
                column: "account_id");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_installments_installment_id",
                table: "transactions",
                column: "installment_id",
                principalTable: "installments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_installments_installment_id",
                table: "transactions");

            migrationBuilder.DropTable(
                name: "installment_items");

            migrationBuilder.DropTable(
                name: "installments");

            migrationBuilder.DropIndex(
                name: "IX_transactions_installment_id",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "installment_id",
                table: "transactions");
        }
    }
}
