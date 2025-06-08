using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace meal_menu_api.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupRecipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "GroupRecipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerGroupId = table.Column<int>(type: "int", nullable: false),
                    OwnerUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ppl = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GroupEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRecipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupRecipes_GroupMembers_OwnerGroupId_OwnerUserId",
                        columns: x => new { x.OwnerGroupId, x.OwnerUserId },
                        principalTable: "GroupMembers",
                        principalColumns: new[] { "GroupId", "UserId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupRecipes_Groups_GroupEntityId",
                        column: x => x.GroupEntityId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                });

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
                name: "IX_GroupRecipes_GroupEntityId",
                table: "GroupRecipes",
                column: "GroupEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupRecipes_OwnerGroupId_OwnerUserId",
                table: "GroupRecipes",
                columns: new[] { "OwnerGroupId", "OwnerUserId" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_GroupRecipes_GroupRecipeEntityId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_GroupRecipes_GroupRecipeEntityId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Steps_GroupRecipes_GroupRecipeEntityId",
                table: "Steps");

            migrationBuilder.DropTable(
                name: "GroupRecipes");

            migrationBuilder.DropIndex(
                name: "IX_Steps_GroupRecipeEntityId",
                table: "Steps");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_GroupRecipeEntityId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Images_GroupRecipeEntityId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "GroupRecipeEntityId",
                table: "Steps");

            migrationBuilder.DropColumn(
                name: "GroupRecipeEntityId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "GroupRecipeEntityId",
                table: "Images");
        }
    }
}
