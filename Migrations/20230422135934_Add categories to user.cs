using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceMVC.Migrations
{
    /// <inheritdoc />
    public partial class Addcategoriestouser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserTodoCategory",
                columns: table => new
                {
                    PreferredTodoCategoriesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserTodoCategory", x => new { x.PreferredTodoCategoriesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserTodoCategory_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserTodoCategory_TodoCategory_PreferredTodoCategoriesId",
                        column: x => x.PreferredTodoCategoriesId,
                        principalTable: "TodoCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserTodoCategory_UsersId",
                table: "ApplicationUserTodoCategory",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserTodoCategory");
        }
    }
}
