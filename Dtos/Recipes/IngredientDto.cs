
namespace meal_menu_api.Dtos
{
    public class IngredientDto
    {
        public int Id { get; set; }

        public string? Description { get; set; }

        public string Name { get; set; } = null!;

        public int Amount { get; set; }

        public string? Unit { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
