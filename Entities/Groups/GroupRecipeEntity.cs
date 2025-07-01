using meal_menu_api.Entities.Account;
using meal_menu_api.Entities.Recipes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace meal_menu_api.Entities.Groups
{
    public class GroupRecipeEntity
    {
        [Key]
        public int Id { get; set; }

        public int GroupId { get; set; }

        public GroupEntity Group { get; set; } = null!;

        public string SharedByUserId { get; set; } = null!;

        [ForeignKey(nameof(SharedByUserId))]
        public AppUser SharedBy { get; set; } = null!;

        public int RecipeId { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Ppl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<ImageEntity> Images { get; set; } = [];

        public List<IngredientEntity> Ingredients { get; set; } = [];

        public List<StepEntity> Steps { get; set; } = [];
    }
}