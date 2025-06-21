using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace meal_menu_api.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenamedColumsInGroupRecipeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupRecipes_GroupMembers_OwnerGroupId_OwnerUserId",
                table: "GroupRecipes");

            migrationBuilder.RenameColumn(
                name: "OwnerUserId",
                table: "GroupRecipes",
                newName: "RecipeOwnerId");

            migrationBuilder.RenameColumn(
                name: "OwnerGroupId",
                table: "GroupRecipes",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupRecipes_OwnerGroupId_OwnerUserId",
                table: "GroupRecipes",
                newName: "IX_GroupRecipes_GroupId_RecipeOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRecipes_GroupMembers_GroupId_RecipeOwnerId",
                table: "GroupRecipes",
                columns: new[] { "GroupId", "RecipeOwnerId" },
                principalTable: "GroupMembers",
                principalColumns: new[] { "GroupId", "UserId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupRecipes_GroupMembers_GroupId_RecipeOwnerId",
                table: "GroupRecipes");

            migrationBuilder.RenameColumn(
                name: "RecipeOwnerId",
                table: "GroupRecipes",
                newName: "OwnerUserId");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "GroupRecipes",
                newName: "OwnerGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupRecipes_GroupId_RecipeOwnerId",
                table: "GroupRecipes",
                newName: "IX_GroupRecipes_OwnerGroupId_OwnerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRecipes_GroupMembers_OwnerGroupId_OwnerUserId",
                table: "GroupRecipes",
                columns: new[] { "OwnerGroupId", "OwnerUserId" },
                principalTable: "GroupMembers",
                principalColumns: new[] { "GroupId", "UserId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
