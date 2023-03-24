using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceMVC.Migrations
{
    /// <inheritdoc />
    public partial class OvverrideMoney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Money",
                table: "Expenses",
                type: "Money",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Money",
                table: "Expenses",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Money");
        }
    }
}
