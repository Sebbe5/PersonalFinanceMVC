using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceMVC.Migrations
{
    /// <inheritdoc />
    public partial class Connecttodowithcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TodoCategoryId",
                table: "Todos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Todos_TodoCategoryId",
                table: "Todos",
                column: "TodoCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TodoCategory_TodoCategoryId",
                table: "Todos",
                column: "TodoCategoryId",
                principalTable: "TodoCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_TodoCategory_TodoCategoryId",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_TodoCategoryId",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "TodoCategoryId",
                table: "Todos");
        }
    }
}
