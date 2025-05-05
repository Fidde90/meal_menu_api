using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace meal_menu_api.Entities
{
    public class ImageEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ImageUrl { get; set; } = null!;

        [ForeignKey(nameof(Recipe))]
        public int RecipeId { get; set; }
        public RecipeEntity Recipe { get; set; } = null!;
    }
}
