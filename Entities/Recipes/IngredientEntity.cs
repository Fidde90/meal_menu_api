using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace meal_menu_api.Entities.Recipes
{
    public class IngredientEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Unit))]
        public int UnitId { get; set; }
        public UnitEntity Unit { get; set; } = null!;

        [ForeignKey(nameof(Recipe))]
        public int RecipeId { get; set; }
        public RecipeEntity Recipe { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        [MinLength(2)]
        public string Name { get; set; } = null!;

        public int Amount { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
