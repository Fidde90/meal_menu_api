using meal_menu_api.Entities.Recipes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace meal_menu_api.Entities.Groups
{
    public class GroupRecipeEntity
    {
        [Key]
        public int Id { get; set; }

        public int OwnerGroupId { get; set; }

        public string OwnerUserId { get; set; } = null!;

        public GroupMemberEntity RecipeOwner { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Ppl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<ImageEntity> Images { get; set; } = [];

        public List<IngredientEntity> Ingredients { get; set; } = [];

        public List<StepEntity> Steps { get; set; } = [];
    }
}
