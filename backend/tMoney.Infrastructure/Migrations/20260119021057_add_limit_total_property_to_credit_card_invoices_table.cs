using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tMoney.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_limit_total_property_to_credit_card_invoices_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "limit_total",
                table: "credit_card_invoices",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "limit_total",
                table: "credit_card_invoices");
        }
    }
}
