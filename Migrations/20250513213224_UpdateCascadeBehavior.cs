using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace meal_menu_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCascadeBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dinners_Recipes_RecipeId",
                table: "Dinners");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "Dinners",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Dinners_Recipes_RecipeId",
                table: "Dinners",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dinners_Recipes_RecipeId",
                table: "Dinners");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "Dinners",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Dinners_Recipes_RecipeId",
                table: "Dinners",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
