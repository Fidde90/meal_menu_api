using System.ComponentModel.DataAnnotations;

namespace meal_menu_api.Entities
{
    public class RecipeEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(5)]
        public string Description { get; set; } = null!;

        public int Ppl {  get; set; }

        public List<ImageEntity> Images { get; set; } = new List<ImageEntity>();

        public List<IngredientEntity> Ingredients { get; set; } = new List<IngredientEntity>();

        public List<StepEntity> Steps { get; set; } = new List<StepEntity>();   
    }
}
