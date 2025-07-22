using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace meal_menu_api.Migrations
{
    /// <inheritdoc />
    public partial class FixedSharedById : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupRecipes_AspNetUsers_SharedById",
                table: "GroupRecipes");

            migrationBuilder.DropIndex(
                name: "IX_GroupRecipes_SharedById",
                table: "GroupRecipes");

            migrationBuilder.DropColumn(
                name: "SharedById",
                table: "GroupRecipes");

            migrationBuilder.AlterColumn<string>(
                name: "SharedByUserId",
                table: "GroupRecipes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_GroupRecipes_SharedByUserId",
                table: "GroupRecipes",
                column: "SharedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRecipes_AspNetUsers_SharedByUserId",
                table: "GroupRecipes",
                column: "SharedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupRecipes_AspNetUsers_SharedByUserId",
                table: "GroupRecipes");

            migrationBuilder.DropIndex(
                name: "IX_GroupRecipes_SharedByUserId",
                table: "GroupRecipes");

            migrationBuilder.AlterColumn<string>(
                name: "SharedByUserId",
                table: "GroupRecipes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "SharedById",
                table: "GroupRecipes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupRecipes_SharedById",
                table: "GroupRecipes",
                column: "SharedById");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRecipes_AspNetUsers_SharedById",
                table: "GroupRecipes",
                column: "SharedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
