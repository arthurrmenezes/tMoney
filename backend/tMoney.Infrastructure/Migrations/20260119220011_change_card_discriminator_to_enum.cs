using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tMoney.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class change_card_discriminator_to_enum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE cards
                ALTER COLUMN card_type TYPE integer
                USING CASE 
                WHEN card_type = 'Debit' THEN 0 
                WHEN card_type = 'Credit' THEN 1
                ELSE 0 -- Fallback seguro
                END;
            ");

            migrationBuilder.AlterColumn<int>(
                name: "card_type",
                table: "cards",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
