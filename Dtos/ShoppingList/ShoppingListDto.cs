using meal_menu_api.Models.Enums;

namespace meal_menu_api.Dtos
{
    public class ShoppingListDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Notes { get; set; }

        public ShoppingListStatus Status { get; set; }

        public List<ShoppingListIngredientDto> Ingredients { get; set; } = [];

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
