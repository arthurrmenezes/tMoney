using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tMoney.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_card_type_property_to_cards_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "card_type",
                table: "cards",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "cards",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "cards");

            migrationBuilder.AlterColumn<string>(
                name: "card_type",
                table: "cards",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
