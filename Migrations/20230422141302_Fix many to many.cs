using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceMVC.Migrations
{
    /// <inheritdoc />
    public partial class Fixmanytomany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserTodoCategory_AspNetUsers_UsersId",
                table: "ApplicationUserTodoCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserTodoCategory_TodoCategory_PreferredTodoCategoriesId",
                table: "ApplicationUserTodoCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserTodoCategory",
                table: "ApplicationUserTodoCategory");

            migrationBuilder.RenameTable(
                name: "ApplicationUserTodoCategory",
                newName: "UserTodoCategory");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserTodoCategory_UsersId",
                table: "UserTodoCategory",
                newName: "IX_UserTodoCategory_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTodoCategory",
                table: "UserTodoCategory",
                columns: new[] { "PreferredTodoCategoriesId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserTodoCategory_AspNetUsers_UsersId",
                table: "UserTodoCategory",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTodoCategory_TodoCategory_PreferredTodoCategoriesId",
                table: "UserTodoCategory",
                column: "PreferredTodoCategoriesId",
                principalTable: "TodoCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTodoCategory_AspNetUsers_UsersId",
                table: "UserTodoCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTodoCategory_TodoCategory_PreferredTodoCategoriesId",
                table: "UserTodoCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTodoCategory",
                table: "UserTodoCategory");

            migrationBuilder.RenameTable(
                name: "UserTodoCategory",
                newName: "ApplicationUserTodoCategory");

            migrationBuilder.RenameIndex(
                name: "IX_UserTodoCategory_UsersId",
                table: "ApplicationUserTodoCategory",
                newName: "IX_ApplicationUserTodoCategory_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserTodoCategory",
                table: "ApplicationUserTodoCategory",
                columns: new[] { "PreferredTodoCategoriesId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserTodoCategory_AspNetUsers_UsersId",
                table: "ApplicationUserTodoCategory",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserTodoCategory_TodoCategory_PreferredTodoCategoriesId",
                table: "ApplicationUserTodoCategory",
                column: "PreferredTodoCategoriesId",
                principalTable: "TodoCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
