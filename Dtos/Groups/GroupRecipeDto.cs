
namespace meal_menu_api.Dtos.Groups
{
    public class GroupRecipeDto
    {
        public int Id { get; set; }

        public string SharedByName { get; set; } = null!;

        public int RecipeId { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Ppl { get; set; }

        public DateTime CreatedAt { get; set; } 

        public DateTime UpdatedAt { get; set; } 

        public List<ImageDto> Images { get; set; } = [];

        public List<IngredientDto> Ingredients { get; set; } = [];

        public List<StepDto> Steps { get; set; } = [];
    }
}
