using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceMVC.Migrations
{
    /// <inheritdoc />
    public partial class Tryingtoconnectbudgetswithusers : Migration
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

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Budgets",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Budgets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Budgets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_ApplicationUserId",
                table: "Budgets",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_AspNetUsers_ApplicationUserId",
                table: "Budgets",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_AspNetUsers_ApplicationUserId",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_ApplicationUserId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Budgets");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Budgets",
                newName: "UserID");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Budgets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
