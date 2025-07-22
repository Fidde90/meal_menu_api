using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace meal_menu_api.Migrations
{
    /// <inheritdoc />
    public partial class AlterdGroupRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupRecipes_AspNetUsers_SharedByUserId",
                table: "GroupRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_GroupRecipes_GroupRecipeEntityId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_GroupRecipes_GroupRecipeEntityId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Steps_GroupRecipes_GroupRecipeEntityId",
                table: "Steps");

            migrationBuilder.DropIndex(
                name: "IX_Steps_GroupRecipeEntityId",
                table: "Steps");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_GroupRecipeEntityId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Images_GroupRecipeEntityId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_GroupRecipes_SharedByUserId",
                table: "GroupRecipes");

            migrationBuilder.DropColumn(
                name: "GroupRecipeEntityId",
                table: "Steps");

            migrationBuilder.DropColumn(
                name: "GroupRecipeEntityId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "GroupRecipeEntityId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "GroupRecipes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "GroupRecipes");

            migrationBuilder.DropColumn(
                name: "Ppl",
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
                name: "IX_GroupRecipes_RecipeId",
                table: "GroupRecipes",
                column: "RecipeId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRecipes_Recipes_RecipeId",
                table: "GroupRecipes",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupRecipes_AspNetUsers_SharedById",
                table: "GroupRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupRecipes_Recipes_RecipeId",
                table: "GroupRecipes");

            migrationBuilder.DropIndex(
                name: "IX_GroupRecipes_RecipeId",
                table: "GroupRecipes");

            migrationBuilder.DropIndex(
                name: "IX_GroupRecipes_SharedById",
                table: "GroupRecipes");

            migrationBuilder.DropColumn(
                name: "SharedById",
                table: "GroupRecipes");

            migrationBuilder.AddColumn<int>(
                name: "GroupRecipeEntityId",
                table: "Steps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupRecipeEntityId",
                table: "Ingredients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupRecipeEntityId",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SharedByUserId",
                table: "GroupRecipes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "GroupRecipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "GroupRecipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Ppl",
                table: "GroupRecipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Steps_GroupRecipeEntityId",
                table: "Steps",
                column: "GroupRecipeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_GroupRecipeEntityId",
                table: "Ingredients",
                column: "GroupRecipeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_GroupRecipeEntityId",
                table: "Images",
                column: "GroupRecipeEntityId");

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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_GroupRecipes_GroupRecipeEntityId",
                table: "Images",
                column: "GroupRecipeEntityId",
                principalTable: "GroupRecipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_GroupRecipes_GroupRecipeEntityId",
                table: "Ingredients",
                column: "GroupRecipeEntityId",
                principalTable: "GroupRecipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_GroupRecipes_GroupRecipeEntityId",
                table: "Steps",
                column: "GroupRecipeEntityId",
                principalTable: "GroupRecipes",
                principalColumn: "Id");
        }
    }
}
