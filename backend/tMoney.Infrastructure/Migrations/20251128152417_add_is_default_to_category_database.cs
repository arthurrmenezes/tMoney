using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tMoney.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_is_default_to_category_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                table: "categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_default",
                table: "categories");
        }
    }
}
