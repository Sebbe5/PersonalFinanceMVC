using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceMVC.Migrations
{
    /// <inheritdoc />
    public partial class Allownulls2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_TodoCategory_TodoCategoryId",
                table: "Todos");

            migrationBuilder.AlterColumn<int>(
                name: "TodoCategoryId",
                table: "Todos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TodoCategory_TodoCategoryId",
                table: "Todos",
                column: "TodoCategoryId",
                principalTable: "TodoCategory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_TodoCategory_TodoCategoryId",
                table: "Todos");

            migrationBuilder.AlterColumn<int>(
                name: "TodoCategoryId",
                table: "Todos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TodoCategory_TodoCategoryId",
                table: "Todos",
                column: "TodoCategoryId",
                principalTable: "TodoCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
