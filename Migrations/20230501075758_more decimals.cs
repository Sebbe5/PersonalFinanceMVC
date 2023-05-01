using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceMVC.Migrations
{
    /// <inheritdoc />
    public partial class moredecimals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ExpectedAnnualInterest",
                table: "Investments",
                type: "Decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ExpectedAnnualInterest",
                table: "Investments",
                type: "Decimal(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);
        }
    }
}
