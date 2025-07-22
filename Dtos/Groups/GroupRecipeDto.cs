
namespace meal_menu_api.Dtos.Groups
{
    public class GroupRecipeDto
    {
        public int Id { get; set; }

        public string SharedByName { get; set; } = null!;

        public RecipeDtoGet RecipeDto { get; set; } = null!;

        public DateTime CreatedAt { get; set; } 

        public DateTime UpdatedAt { get; set; } 
    }
}
