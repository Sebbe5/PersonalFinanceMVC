using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceMVC.Migrations
{
    /// <inheritdoc />
    public partial class Removedbudgetlisttoidentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_AspNetUsers_UserID",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_UserID",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Budgets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Budgets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_UserID",
                table: "Budgets",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_AspNetUsers_UserID",
                table: "Budgets",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
