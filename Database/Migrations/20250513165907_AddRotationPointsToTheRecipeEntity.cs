using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace meal_menu_api.Migrations
{
    /// <inheritdoc />
    public partial class AddRotationPointsToTheRecipeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RotationPoints",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RotationPoints",
                table: "Recipes");
        }
    }
}
