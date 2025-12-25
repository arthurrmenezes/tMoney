using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tMoney.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fix_installment_item_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "installment_items",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "installment_items",
                newName: "created_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "installment_items",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "installment_items",
                newName: "CreatedAt");
        }
    }
}
