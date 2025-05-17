using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace meal_menu_api.Migrations
{
    /// <inheritdoc />
    public partial class AddNewRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dinners_Recipes_RecipeId",
                table: "Dinners");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Units_UnitId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Dinners_RecipeId",
                table: "Dinners");

            migrationBuilder.CreateTable(
                name: "ShoppingLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DinnerScheduleId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingLists_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingLists_DinnerSchedules_DinnerScheduleId",
                        column: x => x.DinnerScheduleId,
                        principalTable: "DinnerSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingListIngredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShoppingListId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsChecked = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingListIngredients_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListIngredients_ShoppingListId",
                table: "ShoppingListIngredients",
                column: "ShoppingListId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_DinnerScheduleId",
                table: "ShoppingLists",
                column: "DinnerScheduleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_UserId",
                table: "ShoppingLists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Units_UnitId",
                table: "Ingredients",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Units_UnitId",
                table: "Ingredients");

            migrationBuilder.DropTable(
                name: "ShoppingListIngredients");

            migrationBuilder.DropTable(
                name: "ShoppingLists");

            migrationBuilder.CreateIndex(
                name: "IX_Dinners_RecipeId",
                table: "Dinners",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dinners_Recipes_RecipeId",
                table: "Dinners",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Units_UnitId",
                table: "Ingredients",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
