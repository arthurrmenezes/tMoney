using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tMoney.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class create_card_table_in_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var defaultCardId = new Guid("11111111-1111-1111-1111-111111111111");

            migrationBuilder.AddColumn<Guid>(
                name: "card_id",
                table: "transactions",
                type: "uuid",
                nullable: false,
                defaultValue: defaultCardId);

            migrationBuilder.CreateTable(
                name: "card",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    card_type = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    limit = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    close_day = table.Column<int>(type: "integer", nullable: true),
                    due_day = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "credit_card_invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    credit_card_id = table.Column<Guid>(type: "uuid", nullable: false),
                    month = table.Column<int>(type: "integer", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    close_day = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    due_day = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    amount_paid = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_card_invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_credit_card_invoices_card_credit_card_id",
                        column: x => x.credit_card_id,
                        principalTable: "card",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql($@"
                INSERT INTO card (
                    ""Id"", ""account_id"", ""name"", ""card_type"", ""created_at""
                ) VALUES (
                    '{defaultCardId}', 
                    (SELECT ""id"" FROM accounts LIMIT 1), -- Pega a primeira conta existente para ser dona desse cartão
                    'Cartão Legado', 
                    'Debit', 
                    NOW()
                );
            ");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_card_id",
                table: "transactions",
                column: "card_id");

            migrationBuilder.CreateIndex(
                name: "IX_credit_card_invoices_credit_card_id",
                table: "credit_card_invoices",
                column: "credit_card_id");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_card_card_id",
                table: "transactions",
                column: "card_id",
                principalTable: "card",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_card_card_id",
                table: "transactions");

            migrationBuilder.DropTable(
                name: "credit_card_invoices");

            migrationBuilder.DropTable(
                name: "card");

            migrationBuilder.DropIndex(
                name: "IX_transactions_card_id",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "card_id",
                table: "transactions");
        }
    }
}
