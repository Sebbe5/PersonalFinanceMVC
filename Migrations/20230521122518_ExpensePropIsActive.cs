using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceMVC.Migrations
{
    /// <inheritdoc />
    public partial class ExpensePropIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Expenses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Expenses");
        }
    }
}
