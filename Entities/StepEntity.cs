using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace meal_menu_api.Entities
{
    public class StepEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Recipe))]
        public int RecipeId { get; set; }

        public RecipeEntity Recipe { get; set; } = null!;

        [Required]
        [MinLength(2)]
        public string Description { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
