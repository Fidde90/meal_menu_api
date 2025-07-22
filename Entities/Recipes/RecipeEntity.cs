using meal_menu_api.Entities.Account;
using meal_menu_api.Entities.Groups;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace meal_menu_api.Entities.Recipes
{
    public class RecipeEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        public AppUser User { get; set; } = null!;

        [Required]
        [MinLength(2)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(5)]
        public string Description { get; set; } = null!;

        public int Ppl {  get; set; }

        public int RotationPoints { get; set; } = 0;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<ImageEntity> Images { get; set; } = [];

        public List<IngredientEntity> Ingredients { get; set; } = [];

        public List<StepEntity> Steps { get; set; } = [];

        public ICollection<GroupRecipeEntity> SharedWithGroups { get; set; } = [];
    }
}
