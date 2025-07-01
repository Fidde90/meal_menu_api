using System.ComponentModel.DataAnnotations;

namespace meal_menu_api.Models.Forms
{
    public class CreateGroupRecipeModel
    {
        public List<int> GroupIds { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int RecipeId { get; set; } = -1;
    }
}
