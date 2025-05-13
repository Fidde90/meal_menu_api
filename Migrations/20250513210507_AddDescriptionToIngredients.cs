using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace meal_menu_api.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToIngredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Ingredients");
        }
    }
}
