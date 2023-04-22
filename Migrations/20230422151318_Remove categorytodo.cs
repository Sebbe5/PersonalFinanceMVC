using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceMVC.Migrations
{
    /// <inheritdoc />
    public partial class Removecategorytodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_TodoCategory_TodoCategoryId",
                table: "Todos");

            migrationBuilder.DropTable(
                name: "TodoCategoryApplicationUser");

            migrationBuilder.DropTable(
                name: "TodoCategory");

            migrationBuilder.DropIndex(
                name: "IX_Todos_TodoCategoryId",
                table: "Todos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TodoCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TodoCategoryApplicationUser",
                columns: table => new
                {
                    ApplicationUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TodoCategoriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoCategoryApplicationUser", x => new { x.ApplicationUsersId, x.TodoCategoriesId });
                    table.ForeignKey(
                        name: "FK_TodoCategoryApplicationUser_AspNetUsers_ApplicationUsersId",
                        column: x => x.ApplicationUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TodoCategoryApplicationUser_TodoCategory_TodoCategoriesId",
                        column: x => x.TodoCategoriesId,
                        principalTable: "TodoCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Todos_TodoCategoryId",
                table: "Todos",
                column: "TodoCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoCategoryApplicationUser_TodoCategoriesId",
                table: "TodoCategoryApplicationUser",
                column: "TodoCategoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TodoCategory_TodoCategoryId",
                table: "Todos",
                column: "TodoCategoryId",
                principalTable: "TodoCategory",
                principalColumn: "Id");
        }
    }
}
